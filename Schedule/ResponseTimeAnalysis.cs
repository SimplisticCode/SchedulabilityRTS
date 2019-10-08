using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Schedule.Data;

namespace Schedule
{
    public class ResponseTimeAnalysis
    {
        //Static and dynamic priority

        //Blocking time is if a lower priority task is running and cannot be preempted
        public static void BlockingTime(List<Task> taskSet)
        {
            foreach (var task in taskSet.OrderBy(o => o.StaticPriority))
            {
                var blockingTasks = taskSet.Where(o =>
                    task.StaticPriority > o.StaticPriority && o.DynamicPriority >= task.StaticPriority);
                task.BlockingTime = blockingTasks.Any() ? blockingTasks.Max(o => o.ExecutionTime) : 0;
            }
        }

        public static void WorstCaseStartTimeAnalysis(List<Task> taskSet)
        {
            foreach (var task in taskSet.OrderBy(o => o.StaticPriority))
            {
                task.StartTimeWorstCase = WorstCaseStartTime(task, taskSet, 1);
            }
        }

        public static void WorstCaseFinishTime(List<Task> taskSet)
        {
            foreach (var task in taskSet.OrderBy(o => o.StaticPriority))
            {
                task.FinishTimeWorstCase = WorstCaseFinishTime(task, taskSet);
            }
        }

        public static void WorstCaseResponseTimeAnalysis(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                WorstCaseResponseTime(task, tasks);
            }
        }

        private static void WorstCaseResponseTime(Task task, List<Task> taskSet)
        {
            var done = false;
            var q = 1;
            var m = 0;
            var S = new Dictionary<int, int>();
            var F = new Dictionary<int, int>();
            while (!done)
            {
                S.Add(q, WorstCaseStartTime(task, taskSet, q));
                F.Add(q, WorstCaseFinishTime(task, taskSet));
                if (task.FinishTimeWorstCase <= q * task.Period)
                {
                    done = true;
                    m = q;
                }
                else
                {
                    q += 1;
                }
            }

            task.WorstCaseRunTime = findMax(F, task.Period);
        }

        private static int findMax(Dictionary<int, int> finishTimes, int taskPeriod)
        {
            var max = 0;
            foreach (var (q, finishTime) in finishTimes.ToList())
            {
                var f = finishTime - (q - 1) * taskPeriod;
                if (f > max)
                {
                    max = f;
                }
            }

            return max;
        }

        private static int WorstCaseFinishTime(Task task, List<Task> taskSet)
        {
            var finishTime = task.ExecutionTime + task.StartTimeWorstCase;
            var new_finishTime = finishTime;
            do
            {
                finishTime = new_finishTime;
                new_finishTime = task.ExecutionTime + task.StartTimeWorstCase;
                foreach (var taskWithHigherPriority in taskSet.Where(o => o.StaticPriority > task.DynamicPriority))
                {
                    new_finishTime += (int) (Math.Ceiling(Decimal.Divide(finishTime, taskWithHigherPriority.Period)) -
                                             (1 + Math.Floor(Decimal.Divide(task.StartTimeWorstCase,
                                                  taskWithHigherPriority.Period)))) *
                                      taskWithHigherPriority.ExecutionTime;
                }
            } while (finishTime != new_finishTime);

            return finishTime;
        }

        private static int WorstCaseStartTime(Task task, List<Task> taskSet, int q)
        {
            var startTime = task.BlockingTime + (q - 1) * task.ExecutionTime;
            var new_StartTime = startTime;

            do
            {
                startTime = new_StartTime;
                new_StartTime = task.BlockingTime + (q - 1) * task.ExecutionTime;
                foreach (var taskWithHigherPriority in taskSet.Where(o => o.StaticPriority > task.StaticPriority))
                {
                    new_StartTime += (int) (1 + Math.Floor(Decimal.Divide(startTime,
                                                taskWithHigherPriority.Period))) *
                                     taskWithHigherPriority.ExecutionTime;
                }
            } while (startTime != new_StartTime);


            return startTime;
        }

        public static void ChangeDynamicPrioritiesToMeetDeadlines(List<Task> tasks)
        {
            PriorityAssigner.AssignDynamicPriorities(tasks); //Dynamic Priority should be at least the same as the static priority
            var maximumPriority = tasks.Max(o => o.StaticPriority);
            var canSystemBeMadeScheduable = true;
            while (!FeasibilityUsingResponseTimeAnalysis(tasks) && canSystemBeMadeScheduable)
            {
                var tasksThatMissTheirDeadlines = tasks.Where(o => o.WorstCaseRunTime > o.Deadline).ToList();
                foreach (var task in tasksThatMissTheirDeadlines)
                {
                    if (task.DynamicPriority < maximumPriority)
                    {
                        task.DynamicPriority += 1;
                    }
                    else
                    {
                        canSystemBeMadeScheduable = false;
                    }
                }
            }
        }

        /// <summary>
        /// This function takes a list of tasks and assign them with a static priority after the Deadline Monotonic principle
        /// It changes the dynamic priorities of the task to see if the task set can be made scheduleable
        /// It thereafter calculate the different times 
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static bool DoesFeasibleScheduleExist(List<Task> tasks)
        {
            PriorityAssigner.DeadlineMonotonicPriorityAssignment(tasks);
            ChangeDynamicPrioritiesToMeetDeadlines(tasks);
            return tasks.TrueForAll(o => o.WorstCaseRunTime <= o.Deadline);
        }


        public static bool FeasibilityStudy(List<Task> tasks)
        {
            ChangeDynamicPrioritiesToMeetDeadlines(tasks);
            return tasks.TrueForAll(o => o.WorstCaseRunTime <= o.Deadline);
        }

        public static bool FeasibilityUsingResponseTimeAnalysis(List<Task> tasks)
        {
            BlockingTime(tasks);
            WorstCaseStartTimeAnalysis(tasks);
            WorstCaseFinishTime(tasks);
            WorstCaseResponseTimeAnalysis(tasks);
            return tasks.TrueForAll(o => o.WorstCaseRunTime <= o.Deadline);
        }

        public static bool PerformScheduabilityStudy(List<Task> taskSet, int numberOfPropertiesSpecificed)
        {
            var isTaskScheduable = false;
            switch (numberOfPropertiesSpecificed)
            {
                case 5:
                    isTaskScheduable = FeasibilityUsingResponseTimeAnalysis(taskSet);
                    break;
                case 4:
                    isTaskScheduable = FeasibilityStudy(taskSet);
                    break;
                default:
                    isTaskScheduable = DoesFeasibleScheduleExist(taskSet);
                    break;
            }

            return isTaskScheduable;
        }
    }
}