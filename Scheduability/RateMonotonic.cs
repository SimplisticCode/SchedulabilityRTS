using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduability
{
    public class RateMonotonic
    {
        public bool IsTaskScheduleable(List<Task> taskSet)
        {
            var schedulable = false;
            AssignPriorities(taskSet);
            var utilization = taskSet.Sum(o => o.Utilization);
            if (IsSystemOverloaded(utilization))
            {
                return false;
            }
            schedulable = LayUtilizationBasedTest(taskSet, utilization);
            if (schedulable)
            {
                return true;
            }
            else
            {
                schedulable = PerformResponseTimeAnalysis(taskSet);
            }

            return schedulable;

        }

        private static bool IsSystemOverloaded(decimal utilization)
        {
            return utilization > 1;
        }

        private bool PerformResponseTimeAnalysis(List<Task> taskSet)
        {
            foreach (var task in taskSet)
            {
                var interruptingTasks = taskSet.SkipWhile(o => o == task).Where(o => o.Priority < task.Priority).ToList();
                var responseTime = CalculateResponseTime(task, interruptingTasks);
                task.ResponseTime = responseTime;
            }

            return (taskSet.TrueForAll(o => o.Period >= o.ResponseTime)) ;
        }

        private int CalculateResponseTime(Task task, List<Task> taskWithHigherPriority)
        {
            if (IsHighestPriorityTask(taskWithHigherPriority))
            {
                return task.ExecutionTime;
            }
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

        private static bool IsHighestPriorityTask(List<Task> taskWithHigherPriority)
        {
            return !taskWithHigherPriority.Any();
        }

        private bool LayUtilizationBasedTest(List<Task> taskSet, decimal utilization)
        {
            var n = taskSet.Count;
            var upperUtilizationBound = n * (Math.Pow(2, ((double) 1 / (double) n)) - 1);
            return !(upperUtilizationBound < (double) utilization);
        }

        private void AssignPriorities(List<Task> taskSet)
        {
            var priority = 1;
            foreach (var task in taskSet.OrderBy(o => o.Period))
            {
                task.Priority = priority;
                priority++;
            }
        }
    }
    
}