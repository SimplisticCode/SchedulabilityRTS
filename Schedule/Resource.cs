using System;
using System.Collections.Generic;
using System.Linq;

namespace Schedule
{
    public class Resource
    {
        public string Name { get; set; }
        public int priority { get; set; }
        public List<TaskUsingResource> tasks{get;set;}

        public int calculateBlockingTimeForTask(Task task)
        {
            var lowerPriorityTasksUsedByResurce = tasks.Where(o => o.Task.StaticPriority <= task.StaticPriority && o.Task != task).ToList();
            if (tasks.Any(o => o.Task == task) && lowerPriorityTasksUsedByResurce.Any())
            {
                return lowerPriorityTasksUsedByResurce.Max(o => o.UsageTime);
            }

            return 0;
        }


        public void UseNPCS()
        {
            priority = Int32.MaxValue;
        }
        
        public void UsePriorityCeiling()
        {
            priority = tasks.Max(o => o.Task.StaticPriority);
        }
    }
}