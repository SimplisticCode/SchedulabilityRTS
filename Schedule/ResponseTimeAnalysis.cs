using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Schedule
{
    public class ResponseTimeAnalysis
    {
        //Static and dynamic priority
        public void runTimeModel(List<Task> taskSet)
        {
            foreach (var task in taskSet.OrderBy(o => o.StaticPriority))
            {
                //task.WorstTimeResponseTime = 
            }
        }

        //Blocking time is if a lower priority task is running and cannot be preempted
        public static void blockingTime(List<Task> taskSet)
        {
            foreach (var task in taskSet.OrderBy(o => o.StaticPriority))
            {
                var blockingTasks = taskSet.Where(o =>
                    task.StaticPriority > o.StaticPriority && o.DynamicPriority >= task.StaticPriority);
                task.BlockingTime = blockingTasks.Any() ? blockingTasks.Max(o => o.ExecutionTime) : 0;
            }
        }

        public static void WorstCaseStartTime(List<Task> taskSet)
        {
            foreach (var task in taskSet.OrderBy(o => o.StaticPriority))
            {
                task.StartTimeWorstCase = task.BlockingTime;
                foreach (var taskWithHigherPriority in taskSet.Where(o => o.StaticPriority > task.StaticPriority))
                {
                    task.StartTimeWorstCase += (1 + 0) * taskWithHigherPriority.ExecutionTime;
                }
            }
        }

        public static void WorstCaseFinishTime(List<Task> taskSet)
        {
            foreach (var task in taskSet.OrderBy(o => o.StaticPriority))
            {
                task.FinishTimeWorstCase = task.ExecutionTime + task.StartTimeWorstCase;
                foreach (var taskWithHigherPriority in taskSet.Where(o => o.StaticPriority > task.DynamicPriority))
                {
                    task.FinishTimeWorstCase += (Math.Ceiling(Decimal.Divide(, taskWithHigherPriority.Period))) -
                                                (1 + Math.Floor(Decimal.Divide(task.StartTimeWorstCase,
                                                     taskWithHigherPriority.Period))) *
                                                taskWithHigherPriority.ExecutionTime;
                }
            }
        }

        public static void WorstCaseResponseTime(Task task, List<Task> taskSet)
        {
            var done = false;
            var q = 1;
            var m = 0;
            var S = new Dictionary<int, int>();
            var F = new Dictionary<int, int>();
            while (!done)
            {
                S.Add(q, WorstCaseStartTime(task, taskSet, q));
                F.Add(q, WorstCaseFinishTime(task, taskSet, q));
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

        private static int WorstCaseFinishTime(Task task, List<Task> taskSet, int i)
        {
                task.FinishTimeWorstCase = task.ExecutionTime + task.StartTimeWorstCase;
                foreach (var taskWithHigherPriority in taskSet.Where(o => o.StaticPriority > task.DynamicPriority))
                {
                    task.FinishTimeWorstCase += (Math.Ceiling(Decimal.Divide(, taskWithHigherPriority.Period))) -
                                                (1 + Math.Floor(Decimal.Divide(task.StartTimeWorstCase,
                                                     taskWithHigherPriority.Period))) *
                                                taskWithHigherPriority.ExecutionTime;
                }
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

        private static int WorstCaseStartTime(Task task, List<Task> taskSet, int q)
        {
            task.StartTimeWorstCase = task.BlockingTime + (q - 1) * task.ExecutionTime ;
            foreach (var taskWithHigherPriority in taskSet.Where(o => o.StaticPriority > task.StaticPriority))
            {
                task.StartTimeWorstCase += (1/* + Math.Floor(Decimal.Divide(, taskWithHigherPriority.Period))*/) * taskWithHigherPriority.ExecutionTime;
            }
        }
    }