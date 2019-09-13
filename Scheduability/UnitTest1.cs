using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Scheduability
{
    public class UnitTest1
    {
        [Fact]
        public void TestA()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 3, Period = 6, Id = 'A'},
                new Task {ExecutionTime = 1, Period = 12, Id = 'B'},
                new Task {ExecutionTime = 10, Period = 24, Id = 'C'}
            };
            
            taskSet.ForEach(o => o.CalcUtilization());
            
            var rateMonotonic = new RateMonotonic();
            var isTaskScheduleable = rateMonotonic.IsTaskScheduleable(taskSet);
            var fileName = "Task1.txt";
            GenerateReport(taskSet, isTaskScheduleable, fileName);
        }
        
        [Fact]
        public void TestB()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 2, Period = 5, Id = 'A'},
                new Task {ExecutionTime = 4, Period = 10, Id = 'B'},
                new Task {ExecutionTime = 3, Period = 15, Id = 'C'}
            };
            
            taskSet.ForEach(o => o.CalcUtilization());
            
            var rateMonotonic = new RateMonotonic();
            var isTaskScheduleable = rateMonotonic.IsTaskScheduleable(taskSet);
            var fileName = "Task2.txt";
            GenerateReport(taskSet, isTaskScheduleable, fileName);
        }
        
        [Fact]
        public void TestC()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 4, Period = 8, Id = 'A'},
                new Task {ExecutionTime = 2, Period = 10, Id = 'B'},
                new Task {ExecutionTime = 20, Period = 80, Id = 'C'}
            };
            
            taskSet.ForEach(o => o.CalcUtilization());
            
            var rateMonotonic = new RateMonotonic();
            var isTaskScheduable = rateMonotonic.IsTaskScheduleable(taskSet);
            var fileName = "Task3.txt";
            GenerateReport(taskSet, isTaskScheduable, fileName);
        }

        [Fact]
        public void Test1()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 2, Period = 8, Id = 'A'},
                new Task {ExecutionTime = 2, Period = 16, Id = 'B'},
                new Task {ExecutionTime = 12, Period = 24, Id = 'C'},
                new Task {ExecutionTime = 6, Period = 48, Id = 'D'}
            };
            
            taskSet.ForEach(o => o.CalcUtilization());
            
            var rateMonotonic = new RateMonotonic();
            var isTaskScheduable = rateMonotonic.IsTaskScheduleable(taskSet);
            var fileName = "Task4.txt";
            GenerateReport(taskSet, isTaskScheduable, fileName);
        }
        
        private void GenerateReport(List<Task> taskSet, bool isTaskScheduable, string fileName)
        {
            string docPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            using (var outputFile = new StreamWriter(docPath))
            {
                var scheduableString = isTaskScheduable? "scheduable":"not scheduable";
                outputFile.WriteLine($"Task set is {scheduableString}");
                outputFile.WriteLine($"Task set has a total system utilization of {taskSet.Sum(o => o.Utilization)}");

                foreach (var task in taskSet)
                {
                    var acceptableString = task.ResponseTime <= task.Period ? "acceptable" : "not acceptable";
                    outputFile.WriteLine($"Task {task.Id} has a response time of {task.ResponseTime} - it has deadline/period of {task.Period}. The response time is acceptable {acceptableString}");
                }
            }
        }
    }
}