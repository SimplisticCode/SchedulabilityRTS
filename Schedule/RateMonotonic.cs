using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Schedule.Data;

namespace Schedule
{
    public class RateMonotonic
    {
        private List<Task> taskSet;
        private bool IsSchedulable;

        public RateMonotonic(List<Task> taskSet)
        {
            this.taskSet = taskSet;
            IsSchedulable = false;
            Debug.Assert(this.taskSet.TrueForAll(o => o.Offset == 0), "All task should have the same offset");
        }

        public bool PerformRateMonotonicAnalysis(string fileName)
        {
            AssignPriorities(taskSet);
            var utilization = taskSet.Sum(o => o.Utilization);
            IsSchedulable = LayUtilizationBasedTest(taskSet, utilization);
            IsSchedulable = PerformResponseTimeAnalysis(taskSet);

            GenerateReport(fileName);
            return IsSchedulable;

        }
        
        private void GenerateReport(string fileName)
        {
            string docPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            using (var outputFile = new StreamWriter(docPath))
            {
                var schedulableString = IsSchedulable? "schedulable":"not schedulable";
                outputFile.WriteLine($"Task set is {schedulableString}");
                var systemUtilization = taskSet.Sum(o => o.Utilization);
                outputFile.WriteLine($"Task set has a total system utilization of {systemUtilization}");
                if (systemUtilization > 1)
                {
                    outputFile.WriteLine($"The system is overloaded");
                }

                var upperSystemUtilization = UpperUtilizationBound(taskSet.Count);
                
                outputFile.WriteLine($"Performing Liu and Layland's test:");
                outputFile.WriteLine($"The upper utilization based on Lie and Layland's test: {upperSystemUtilization}.");
                outputFile.WriteLine(upperSystemUtilization >= (double) systemUtilization
                    ? $"The system is schedulable based on Lie and Layland's test."
                    : $"The system is not schedulable based on Lie and Layland's test.  But a feasible schedule may still exist.");

                outputFile.WriteLine($"Performing Response time analysis:");
                foreach (var task in taskSet)
                {
                    var acceptableString = task.ResponseTime <= task.Period ? "acceptable" : "not acceptable";
                    outputFile.WriteLine($"Task {task.Id} has a response time of {task.ResponseTime} - it has deadline/period of {task.Period}. The response time is {acceptableString}");
                }
            }
        }

        private bool PerformResponseTimeAnalysis(List<Task> taskSet)
        {
            foreach (var task in taskSet)
            {
                var interruptingTasks = taskSet.SkipWhile(o => o == task).Where(o => o.StaticPriority < task.StaticPriority).ToList();
                var responseTime = CalculateResponseTime(task, interruptingTasks);
                task.ResponseTime = responseTime;
            }

            return (taskSet.TrueForAll(o => o.Period >= o.ResponseTime)) ;
        }

        private int CalculateResponseTime(Task task, IReadOnlyCollection<Task> taskWithHigherPriority)
        {
            var rn = 0;
            var r = task.ExecutionTime;
            var isFirstTime = true;

            while (r != rn)
            {
                if(!isFirstTime)
                    r = rn;
                isFirstTime = false;
                
                rn = task.ExecutionTime;
                foreach (var interruptingTask in taskWithHigherPriority)
                {
                    rn += (int)Math.Ceiling(decimal.Divide(r, interruptingTask.Period)) * interruptingTask.ExecutionTime;
                }

                if (rn > task.Period)
                {
                    return 100;
                }
            }

            return r;
        }


        private bool LayUtilizationBasedTest(List<Task> taskSet, decimal utilization)
        {
            var upperUtilizationBound = UpperUtilizationBound(taskSet.Count);
            return !(upperUtilizationBound < (double) utilization);
        }

        private static double UpperUtilizationBound(int n)
        {
            var upperUtilizationBound = n * (Math.Pow(2, ((double) 1 / (double) n)) - 1);
            return upperUtilizationBound;
        }

        private void AssignPriorities(List<Task> taskSet)
        {
            var priority = 1;
            foreach (var task in taskSet.OrderBy(o => o.Period))
            {
                task.StaticPriority = priority;
                priority++;
            }
        }
    }
    
}