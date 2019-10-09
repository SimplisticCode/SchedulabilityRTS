using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Schedule.Data;

namespace Schedule
{
    public class TaskFileReader
    {
        public static ReadInDto ReadInTasksFromFile(string filename)
        {
            var result = new ReadInDto();
            var tasks = new List<Task>();

            using (var file = new StreamReader(filename))
            {
                var ln = file.ReadLine();
                var numberOfTasks = int.Parse(ln.Split(",").First());
                result.NumberOfPropertiesSpecificed = int.Parse(ln.Split(",").Skip(1).First());
 
                for (int i = 0; i < numberOfTasks; i++)
                {
                    var taskFromString = createTaskFromString(file.ReadLine(), result.NumberOfPropertiesSpecificed);
                    tasks.Add(taskFromString);
                }
            }
            tasks.ForEach(o => o.CalcUtilization());
            result.tasks = tasks;
            return result;
        }

        public static void CreateTaskFile(List<Task> tasks, string filename)
        {
            using (var file = new StreamWriter(filename))
            {
                file.WriteLine($"{tasks.Count} 5");
                foreach (var task in tasks)
                {
                    file.WriteLine($"{task.ExecutionTime} {task.Period} {task.Deadline} {task.StaticPriority} {task.DynamicPriority}");
                }
            } 
        }
        
        private static Task createTaskFromString(string readLine, int numberOfProperties)
        {
            var arr = readLine.Split(",");
            var task = new Task
            {
                ExecutionTime = Convert.ToInt32(arr[0]),
                Period = Convert.ToInt32(arr[1]),
                Deadline = Convert.ToInt32(arr[2])
            };
            if (numberOfProperties > 3)
            {
                task.StaticPriority = Convert.ToInt32(arr[3]);
            }

            if (numberOfProperties > 4)
            {
                task.DynamicPriority = Convert.ToInt32(arr[4]);
            }
            return task;
        }
    }
    

    public class ReadInDto
    {
        public List<Task> tasks { get; set; }
        public int NumberOfPropertiesSpecificed { get; set; }
    }
}