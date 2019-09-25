using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Schedule;

namespace ScheduleFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("You have now started the coolest program ever!");
            Console.WriteLine("The ScheduleFinder 2000 is here to help you assign the right priorities to your tasks to make sure tht all deadlines are met!");
            Console.WriteLine("Type in the path to your task and let me assign priorities to the task:");
            var path = Console.ReadLine();
            while (!File.Exists(path))
            {
                Console.WriteLine("The path you provided is not valid. Can you please provide a valid path:");
                path = Console.ReadLine();
            }
            Console.WriteLine("Should schedule allow preemption? (y/n)");

            var allowPreemption = true;
            
            var readInTasks = TaskFileReader.ReadInTasksFromFile(path);
            var taskSet = readInTasks.tasks; 
            var isTaskScheduable =  ResponseTimeAnalysis.PerformScheduabilityStudy(taskSet, readInTasks.NumberOfPropertiesSpecificed);


            var fileName = "Test";
            GenerateResultFile(taskSet, isTaskScheduable, fileName);

            Console.WriteLine(isTaskScheduable ? "The task set is scheduleable" : "No feasible assignment found.");
            foreach (var task in taskSet)
            {
                Console.WriteLine(task.ToString());
            } 
            

        }

        private static void GenerateResultFile(List<Task> taskSet, bool isTaskScheduable, string fileName)
        {
            using (var outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine(isTaskScheduable ? "The task set is scheduleable" : "No feasible assignment found.");
                foreach (var task in taskSet)
                {
                    outputFile.WriteLine(task.ToString());
                }
            }
        }
    }
}