using System.Collections.Generic;
using System.Linq;
using Schedule.Data;

namespace Schedule
{
    public class PriorityAssigner
    {
        public static void AssignDynamicPriorities(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.DynamicPriority < task.StaticPriority)
                {
                    task.DynamicPriority = task.StaticPriority;
                }
            }
        }

        public static void DeadlineMonotonicPriorityAssignment(List<Task> tasks)
        {
            var i = 1;
            foreach (var task in tasks.OrderByDescending(o => o.Deadline))
            {
                task.StaticPriority = i++;
            }
        }
    }
}