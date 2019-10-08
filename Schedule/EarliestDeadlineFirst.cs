using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Schedule.Data;

namespace Schedule
{
    public class EarliestDeadlineFirst
    {
        public static void Simulate(List<Task> taskSet)
        {
            var hyperPeriod = Calculator.Calculator.FindHyperPeriod(taskSet);
            var tasks = generateTaskInHyperPeriod(taskSet, hyperPeriod);
            var resultFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"Schedule.txt");
            runSimulation(tasks, hyperPeriod, resultFile);
        }

        private static void runSimulation(List<Task> tasks, long hyperPeriod, string resultFile)
        {
            var queueOfReadyTask = new List<Task>();
            var doneTask = new List<Task>();
            var time = 0;
            using (var writer = new StreamWriter(resultFile))
            {
                writer.WriteLine("Started simulation of EDF");
                while (time <= hyperPeriod)
                {
                    var taskReleased = tasks.Where(o => o.Offset == time).ToList();
                    foreach (var task in taskReleased)
                    {
                        task.DynamicPriority = task.Deadline;
                        tasks.Remove(task);
                        queueOfReadyTask.Add(task);
                    }

                    queueOfReadyTask = queueOfReadyTask.OrderBy(o => o.Deadline).ToList();
                    if (queueOfReadyTask.Any())
                    {
                        queueOfReadyTask.First().ExecutionTime--;
                        writer.WriteLine($"Time: {time} execution task is {queueOfReadyTask.First().Id} with a priority/deadline of {queueOfReadyTask.First().Deadline}");
                        doneTask.AddRange(queueOfReadyTask.Where(o => o.ExecutionTime == 0));
                        queueOfReadyTask.RemoveAll(o => o.ExecutionTime == 0);
                        if (queueOfReadyTask.Any(o => o.Deadline < time))
                        {
                            throw new Exception("Task set is not scheduleable");
                        }
                    }
                    
                    time++;
                } 
                writer.WriteLine("Simulation finished and the task set is scheduleable");
            }
        }

        private static List<Task> generateTaskInHyperPeriod(List<Task> tasks, long hyperPeriod)
        {
            var result = new List<Task>();
            foreach (var task in tasks)
            {
                var numberOfTaskInHyperPeriod = Math.Ceiling(Decimal.Divide(hyperPeriod, task.Period));
                for (int i = 0; i < numberOfTaskInHyperPeriod; i++)
                {
                    result.Add(new Task
                    {
                        Offset = i * task.Period + task.Offset,
                        Deadline = i * task.Period + task.Offset + task.Deadline,
                        ExecutionTime = task.ExecutionTime,
                        Id = task.Id
                    });
                }
            }

            return result.OrderBy(o => o.Offset).ToList();
        }
    }
}