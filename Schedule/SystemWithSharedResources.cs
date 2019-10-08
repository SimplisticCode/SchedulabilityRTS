using System.Collections.Generic;
using System.Linq;
using Schedule.Data;

namespace Schedule
{
    public class SystemWithSharedResources
    {
        public List<Task> Tasks { get; set; }
        public List<Resource> Resources { get; set; }

        public int BlockingTimeOnAResourcePriorityCeiling(Task task)
        {
            var maxBlockingTime = 0;
            var lowerPriorityTask = Tasks.Where(o => o.StaticPriority <= task.StaticPriority && o != task);

            Resources.ForEach(o => o.UsePriorityCeiling());
            var resourcesTaskCanBeBlockedOn = Resources.Where(o => o.priority >= task.StaticPriority);
            foreach (var resource in resourcesTaskCanBeBlockedOn.ToList())
            {
                var potentialTaskBlocking = resource.tasks.Where(o => lowerPriorityTask.Contains(o.Task)).ToList();
                if (potentialTaskBlocking.Any())
                {
                    var blockingTime = potentialTaskBlocking.Max(o => o.UsageTime);
                    if (blockingTime > maxBlockingTime)
                    {
                        maxBlockingTime = blockingTime;
                    }
                }
            }

            return maxBlockingTime;
        }

        public int BlockingTimeOnAResourceNCPS(Task task)
        {
            var maxBlockingTime = 0;
            var lowerPriorityTask = Tasks.Where(o => o.StaticPriority <= task.StaticPriority && o != task);

            foreach (var resource in Resources)
            {
                var potentialTaskBlocking = resource.tasks.Where(o => lowerPriorityTask.Contains(o.Task)).ToList();
                if (potentialTaskBlocking.Any())
                {
                    var blockingTime = potentialTaskBlocking.Max(o => o.UsageTime);
                    if (blockingTime > maxBlockingTime)
                    {
                        maxBlockingTime = blockingTime;
                    }
                }
            }

            return maxBlockingTime;
        }
    }
}