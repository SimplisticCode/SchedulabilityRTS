using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Schedule;
using Schedule.Data;
using Xunit;

namespace Scheduability
{
    public class ScheduleTest
    {
        [Fact]
        public void TestA()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 3, Period = 6, Id = "A"},
                new Task {ExecutionTime = 1, Period = 12, Id = "B"},
                new Task {ExecutionTime = 10, Period = 24, Id = "C"}
            };
            
            taskSet.ForEach(o => o.CalcUtilization());
            
            var rateMonotonic = new RateMonotonic(taskSet);
            var fileName = "Task1.txt";

            rateMonotonic.PerformRateMonotonicAnalysis(fileName);
        }
        
        [Fact]
        public void TestB()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 2, Period = 5, Id = "A"},
                new Task {ExecutionTime = 4, Period = 10, Id = "B"},
                new Task {ExecutionTime = 3, Period = 15, Id = "C"}
            };
            
            taskSet.ForEach(o => o.CalcUtilization());
            var fileName = "Task2.txt";

            var rateMonotonic = new RateMonotonic(taskSet);
            rateMonotonic.PerformRateMonotonicAnalysis(fileName);
        }
        
        [Fact]
        public void TestC()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 4, Period = 8, Id = "A"},
                new Task {ExecutionTime = 2, Period = 10, Id = "B"},
                new Task {ExecutionTime = 20, Period = 80, Id = "C"}
            };
            
            taskSet.ForEach(o => o.CalcUtilization());
            var fileName = "Task3.txt";
            var rateMonotonic = new RateMonotonic(taskSet);
            rateMonotonic.PerformRateMonotonicAnalysis(fileName);
        }

        [Fact]
        public void Test1()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 2, Period = 8, Id = "A"},
                new Task {ExecutionTime = 2, Period = 16, Id = "B"},
                new Task {ExecutionTime = 12, Period = 24, Id = "C"},
                new Task {ExecutionTime = 6, Period = 48, Id = "D"}
            };
            var fileName = "Task4.txt";
 
            taskSet.ForEach(o => o.CalcUtilization());
            
            var rateMonotonic = new RateMonotonic(taskSet);
            rateMonotonic.PerformRateMonotonicAnalysis(fileName);
        }
        
        
        
        [Fact]
        public void Test_BlockingTime()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 5, Period = 50, Id = "1", Deadline = 15, StaticPriority = 9, DynamicPriority = 9},
                new Task {ExecutionTime = 5, Period = 60, Id = "2", Deadline = 25, StaticPriority = 8, DynamicPriority = 8},
                new Task {ExecutionTime = 7, Period = 80, Id = "3", Deadline = 30, StaticPriority = 7, DynamicPriority = 7},
                new Task {ExecutionTime = 7, Period = 200, Id = "4", Deadline = 40, StaticPriority =6, DynamicPriority = 6},
                new Task {ExecutionTime = 10, Period = 200, Id = "5", Deadline = 50, StaticPriority = 5, DynamicPriority = 5},
                new Task {ExecutionTime = 8, Period = 200, Id = "6", Deadline = 60, StaticPriority = 4, DynamicPriority = 4},
                new Task {ExecutionTime = 12, Period = 220, Id = "7", Deadline = 70, StaticPriority = 3, DynamicPriority = 3},
                new Task {ExecutionTime = 10, Period = 230, Id = "8", Deadline = 70, StaticPriority = 2, DynamicPriority = 2},
                new Task {ExecutionTime = 15, Period = 240, Id = "9", Deadline = 100, StaticPriority = 1, DynamicPriority =1}
            };
            taskSet.ForEach(o => o.CalcUtilization());
            
            ResponseTimeAnalysis.BlockingTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == "1").BlockingTime, 0);
            Assert.Equal(0, taskSet.Single(o => o.Id == "2").BlockingTime);
            Assert.Equal(0, taskSet.Single(o => o.Id == "3").BlockingTime);
            Assert.Equal(0, taskSet.Single(o => o.Id == "4").BlockingTime);
            Assert.Equal(taskSet.Single(o => o.Id == "5").BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "6").BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "7").BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "8").BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "9").BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == "1").StartTimeWorstCase, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "2").StartTimeWorstCase, 5);
            Assert.Equal(taskSet.Single(o => o.Id == "3").StartTimeWorstCase, 10);
            Assert.Equal(taskSet.Single(o => o.Id == "4").StartTimeWorstCase, 17);
            Assert.Equal(taskSet.Single(o => o.Id == "5").StartTimeWorstCase, 24);
            Assert.Equal(taskSet.Single(o => o.Id == "6").StartTimeWorstCase, 34);
            Assert.Equal(taskSet.Single(o => o.Id == "7").StartTimeWorstCase, 42);
            Assert.Equal(taskSet.Single(o => o.Id == "8").StartTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == "9").StartTimeWorstCase, 74);
            
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == "1").FinishTimeWorstCase, 5);
            Assert.Equal(taskSet.Single(o => o.Id == "2").FinishTimeWorstCase, 10);
            Assert.Equal(taskSet.Single(o => o.Id == "3").FinishTimeWorstCase, 17);
            Assert.Equal(taskSet.Single(o => o.Id == "4").FinishTimeWorstCase, 24);
            Assert.Equal(taskSet.Single(o => o.Id == "5").FinishTimeWorstCase, 34);
            Assert.Equal(taskSet.Single(o => o.Id == "6").FinishTimeWorstCase, 42);
            Assert.Equal(taskSet.Single(o => o.Id == "7").FinishTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == "8").FinishTimeWorstCase, 74);
            Assert.Equal(taskSet.Single(o => o.Id == "9").FinishTimeWorstCase, 96); 
            
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == "1").WorstCaseRunTime, 5);
            Assert.Equal(taskSet.Single(o => o.Id == "2").WorstCaseRunTime, 10);
            Assert.Equal(taskSet.Single(o => o.Id == "3").WorstCaseRunTime, 17);
            Assert.Equal(taskSet.Single(o => o.Id == "4").WorstCaseRunTime, 24);
            Assert.Equal(taskSet.Single(o => o.Id == "5").WorstCaseRunTime, 34);
            Assert.Equal(taskSet.Single(o => o.Id == "6").WorstCaseRunTime, 42);
            Assert.Equal(taskSet.Single(o => o.Id == "7").WorstCaseRunTime, 59);
            Assert.Equal(taskSet.Single(o => o.Id == "8").WorstCaseRunTime, 74);
            Assert.Equal(taskSet.Single(o => o.Id == "9").WorstCaseRunTime, 96); 
            
            ResponseTimeAnalysis.ChangeDynamicPrioritiesToMeetDeadlines(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == "1").DynamicPriority, 9);
            Assert.Equal(taskSet.Single(o => o.Id == "2").DynamicPriority, 8);
            Assert.Equal(taskSet.Single(o => o.Id == "3").DynamicPriority, 7);
            Assert.Equal(taskSet.Single(o => o.Id == "4").DynamicPriority, 6);
            Assert.Equal(taskSet.Single(o => o.Id == "5").DynamicPriority, 5);
            Assert.Equal(taskSet.Single(o => o.Id == "6").DynamicPriority, 4);
            Assert.Equal(taskSet.Single(o => o.Id == "7").DynamicPriority, 8);
            Assert.Equal(taskSet.Single(o => o.Id == "8").DynamicPriority, 8);
            Assert.Equal(taskSet.Single(o => o.Id == "9").DynamicPriority, 1);  
        }

        [Fact]
        public void Test_BlockingTime1()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 5, Period = 50, Id = "1", Deadline = 15, StaticPriority = 9, DynamicPriority = 9},
                new Task {ExecutionTime = 5, Period = 60, Id = "2", Deadline = 25, StaticPriority = 8, DynamicPriority = 9},
                new Task {ExecutionTime = 7, Period = 80, Id = "3", Deadline = 30, StaticPriority = 7, DynamicPriority = 9},
                new Task {ExecutionTime = 7, Period = 200, Id = "4", Deadline = 40, StaticPriority =6, DynamicPriority = 9},
                new Task {ExecutionTime = 10, Period = 200, Id = "5", Deadline = 50, StaticPriority = 5, DynamicPriority = 9},
                new Task {ExecutionTime = 8, Period = 200, Id = "6", Deadline = 60, StaticPriority = 4, DynamicPriority = 9},
                new Task {ExecutionTime = 12, Period = 220, Id = "7", Deadline = 70, StaticPriority = 3, DynamicPriority = 9},
                new Task {ExecutionTime = 10, Period = 230, Id = "8", Deadline = 70, StaticPriority = 2, DynamicPriority = 9},
                new Task {ExecutionTime = 15, Period = 240, Id = "9", Deadline = 100, StaticPriority = 1, DynamicPriority =9}
            };
            taskSet.ForEach(o => o.CalcUtilization());
            
            ResponseTimeAnalysis.BlockingTime(taskSet);
            
            Assert.Equal(15, taskSet.Single(o => o.Id == "1").BlockingTime);
            Assert.Equal(taskSet.Single(o => o.Id == "2").BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "3").BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "4").BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "5").BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "6").BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "7").BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "8").BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "9").BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == "1").StartTimeWorstCase, 15);
            Assert.Equal(taskSet.Single(o => o.Id == "2").StartTimeWorstCase, 20);
            Assert.Equal(taskSet.Single(o => o.Id == "3").StartTimeWorstCase, 25);
            Assert.Equal(taskSet.Single(o => o.Id == "4").StartTimeWorstCase, 32);
            Assert.Equal(taskSet.Single(o => o.Id == "5").StartTimeWorstCase, 39);
            Assert.Equal(taskSet.Single(o => o.Id == "6").StartTimeWorstCase, 49);
            Assert.Equal(taskSet.Single(o => o.Id == "7").StartTimeWorstCase, 67);
            Assert.Equal(taskSet.Single(o => o.Id == "8").StartTimeWorstCase, 79);
            Assert.Equal(taskSet.Single(o => o.Id == "9").StartTimeWorstCase, 74);
            
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == "1").FinishTimeWorstCase, 20);
            Assert.Equal(taskSet.Single(o => o.Id == "2").FinishTimeWorstCase, 25);
            Assert.Equal(taskSet.Single(o => o.Id == "3").FinishTimeWorstCase, 32);
            Assert.Equal(taskSet.Single(o => o.Id == "4").FinishTimeWorstCase, 39);
            Assert.Equal(taskSet.Single(o => o.Id == "5").FinishTimeWorstCase, 49);
            Assert.Equal(taskSet.Single(o => o.Id == "6").FinishTimeWorstCase, 57);
            Assert.Equal(taskSet.Single(o => o.Id == "7").FinishTimeWorstCase, 79);
            Assert.Equal(taskSet.Single(o => o.Id == "8").FinishTimeWorstCase, 89);
            Assert.Equal(taskSet.Single(o => o.Id == "9").FinishTimeWorstCase, 89); 
            
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == "1").WorstCaseRunTime, 20);
            Assert.Equal(taskSet.Single(o => o.Id == "2").WorstCaseRunTime, 25);
            Assert.Equal(taskSet.Single(o => o.Id == "3").WorstCaseRunTime, 32);
            Assert.Equal(taskSet.Single(o => o.Id == "4").WorstCaseRunTime, 39);
            Assert.Equal(taskSet.Single(o => o.Id == "5").WorstCaseRunTime, 49);
            Assert.Equal(taskSet.Single(o => o.Id == "6").WorstCaseRunTime, 57);
            Assert.Equal(taskSet.Single(o => o.Id == "7").WorstCaseRunTime, 79);
            Assert.Equal(taskSet.Single(o => o.Id == "8").WorstCaseRunTime, 89);
            Assert.Equal(taskSet.Single(o => o.Id == "9").WorstCaseRunTime, 89); 
        } 
        
        [Fact]
        public void Test_BlockingTime2()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 5, Period = 50, Id = "1", Deadline = 15, StaticPriority = 9, DynamicPriority = 9},
                new Task {ExecutionTime = 5, Period = 60, Id = "2", Deadline = 25, StaticPriority = 8, DynamicPriority = 8},
                new Task {ExecutionTime = 7, Period = 80, Id = "3", Deadline = 30, StaticPriority = 7, DynamicPriority = 7},
                new Task {ExecutionTime = 7, Period = 200, Id = "4", Deadline = 40, StaticPriority =6, DynamicPriority = 6},
                new Task {ExecutionTime = 10, Period = 200, Id = "5", Deadline = 50, StaticPriority = 5, DynamicPriority = 5},
                new Task {ExecutionTime = 8, Period = 200, Id = "6", Deadline = 60, StaticPriority = 4, DynamicPriority = 4},
                new Task {ExecutionTime = 12, Period = 220, Id = "7", Deadline = 70, StaticPriority = 3, DynamicPriority = 8},
                new Task {ExecutionTime = 10, Period = 230, Id = "8", Deadline = 70, StaticPriority = 2, DynamicPriority = 8},
                new Task {ExecutionTime = 15, Period = 240, Id = "9", Deadline = 100, StaticPriority = 1, DynamicPriority =1}
            };
            taskSet.ForEach(o => o.CalcUtilization());
            
            ResponseTimeAnalysis.BlockingTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == "1").BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "2").BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == "3").BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == "4").BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == "5").BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == "6").BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == "7").BlockingTime, 10);
            Assert.Equal(taskSet.Single(o => o.Id == "8").BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "9").BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == "1").StartTimeWorstCase, 0);
            Assert.Equal(taskSet.Single(o => o.Id == "2").StartTimeWorstCase, 17);
            Assert.Equal(taskSet.Single(o => o.Id == "3").StartTimeWorstCase, 22);
            Assert.Equal(taskSet.Single(o => o.Id == "4").StartTimeWorstCase, 29);
            Assert.Equal(taskSet.Single(o => o.Id == "5").StartTimeWorstCase, 36);
            Assert.Equal(taskSet.Single(o => o.Id == "6").StartTimeWorstCase, 46);
            Assert.Equal(taskSet.Single(o => o.Id == "7").StartTimeWorstCase, 57);
            Assert.Equal(taskSet.Single(o => o.Id == "8").StartTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == "9").StartTimeWorstCase, 74);
            
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == "1").FinishTimeWorstCase, 5);
            Assert.Equal(taskSet.Single(o => o.Id == "2").FinishTimeWorstCase, 22);
            Assert.Equal(taskSet.Single(o => o.Id == "3").FinishTimeWorstCase, 29);
            Assert.Equal(taskSet.Single(o => o.Id == "4").FinishTimeWorstCase, 36);
            Assert.Equal(taskSet.Single(o => o.Id == "5").FinishTimeWorstCase, 46);
            Assert.Equal(taskSet.Single(o => o.Id == "6").FinishTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == "7").FinishTimeWorstCase, 69);
            Assert.Equal(taskSet.Single(o => o.Id == "8").FinishTimeWorstCase, 69);
            Assert.Equal(taskSet.Single(o => o.Id == "9").FinishTimeWorstCase, 96); 
            
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == "1").WorstCaseRunTime, 5);
            Assert.Equal(taskSet.Single(o => o.Id == "2").WorstCaseRunTime, 22);
            Assert.Equal(taskSet.Single(o => o.Id == "3").WorstCaseRunTime, 29);
            Assert.Equal(taskSet.Single(o => o.Id == "4").WorstCaseRunTime, 36);
            Assert.Equal(taskSet.Single(o => o.Id == "5").WorstCaseRunTime, 46);
            Assert.Equal(taskSet.Single(o => o.Id == "6").WorstCaseRunTime, 59);
            Assert.Equal(taskSet.Single(o => o.Id == "7").WorstCaseRunTime, 69);
            Assert.Equal(taskSet.Single(o => o.Id == "8").WorstCaseRunTime, 69);
            Assert.Equal(taskSet.Single(o => o.Id == "9").WorstCaseRunTime, 96); 
        }


        [Fact]
        public void TaskFileReader1()
        {
            var fileName = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"Task1Import.txt");
            var resultReadInDto  = TaskFileReader.ReadInTasksFromFile(fileName);
            var taskSet = resultReadInDto.tasks;
            Assert.Equal(taskSet.Count, 3);
            Assert.Equal(taskSet[0].ExecutionTime, 20);
            Assert.Equal(taskSet[0].Period, 70);
            Assert.Equal(taskSet[0].Deadline, 50);
            Assert.Equal(taskSet[0].StaticPriority, 3);
            Assert.Equal(taskSet[0].DynamicPriority, 3);
            
            
            Assert.Equal(taskSet[1].ExecutionTime, 20);
            Assert.Equal(taskSet[1].Period, 80);
            Assert.Equal(taskSet[1].Deadline, 80);
            Assert.Equal(taskSet[1].StaticPriority, 2);
            Assert.Equal(taskSet[1].DynamicPriority, 3);
            
            Assert.Equal(taskSet[2].ExecutionTime, 35);
            Assert.Equal(taskSet[2].Period, 200);
            Assert.Equal(taskSet[2].Deadline, 100);
            Assert.Equal(taskSet[2].StaticPriority, 1);
            Assert.Equal(taskSet[2].DynamicPriority, 2);


           var isScheduable = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
           var exist = ResponseTimeAnalysis.DoesFeasibleScheduleExist(taskSet);
           Assert.Equal(isScheduable, exist);
           Assert.True(isScheduable);
        }


        [Fact]
        public void PreemptionDeadline()
        {
            var fileName = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"Task2Import.txt");
            var resultReadInDto  = TaskFileReader.ReadInTasksFromFile(fileName);
            var taskSet = resultReadInDto.tasks;
            ResponseTimeAnalysis.ChangeDynamicPrioritiesToMeetDeadlines(taskSet);
            var isScheduleable = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
            var exist = ResponseTimeAnalysis.DoesFeasibleScheduleExist(taskSet);
            Assert.Equal(isScheduleable, exist);
            Assert.True(isScheduleable);
        }
        
        
        [Fact]
        public void TaskFileReader2()
        {
            var fileName = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"Task3Import.txt");
            var resultReadInDto  = TaskFileReader.ReadInTasksFromFile(fileName);
            var taskSet = resultReadInDto.tasks;
            Assert.Equal(taskSet.Count, 2);
            Assert.Equal(taskSet[0].ExecutionTime, 52);
            Assert.Equal(taskSet[0].Period, 100);
            Assert.Equal(taskSet[0].Deadline, 110);
            Assert.Equal(0, taskSet[0].StaticPriority);
            Assert.Equal(taskSet[0].DynamicPriority, 0);
            
            
            Assert.Equal(52, taskSet[1].ExecutionTime);
            Assert.Equal(140, taskSet[1].Period);
            Assert.Equal(154, taskSet[1].Deadline);
            Assert.Equal(0, taskSet[1].StaticPriority);
            Assert.Equal(0, taskSet[1].DynamicPriority);

            var exist = ResponseTimeAnalysis.PerformScheduabilityStudy(taskSet, resultReadInDto.NumberOfPropertiesSpecificed);

            var id = 1;
            foreach (var task in taskSet)
            {
                task.Id = (id++).ToString();
            }
            EarliestDeadlineFirst.Simulate(taskSet);
        }
        
        [Fact]
        public void OnlyScheduleableUsingPreemption()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 20, Period = 70, Deadline = 30, StaticPriority = 3, Id = "A"},
                new Task {ExecutionTime = 20, Period = 80, Deadline = 80, StaticPriority = 2, Id = "B"},
                new Task {ExecutionTime = 35, Period = 200, Deadline = 115, StaticPriority = 1, Id = "C"},
            };
            
            //non-preemptive scheduling
            taskSet.ForEach(o => o.DynamicPriority = taskSet.Max(x=> x.StaticPriority));

            var scheduleableUsingNonPreemption = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
            Assert.False(scheduleableUsingNonPreemption);
            
            //preemptive scheduling - reset dynamic priorities
            taskSet.ForEach(o => o.DynamicPriority = o.StaticPriority);
            var scheduleableUsingPreemption = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
            Assert.True(scheduleableUsingPreemption);

            var resultFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"preempt.txt");

            TaskFileReader.CreateTaskFile(taskSet,resultFile);
        }
        
   
        [Fact]
        public void OnlyScheduleableUsingNONPreemption()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 20, Period = 70, Deadline = 55, StaticPriority = 3, Id = "A"},
                new Task {ExecutionTime = 20, Period = 80, Deadline = 80, StaticPriority = 2, Id = "B"},
                new Task {ExecutionTime = 35, Period = 200, Deadline = 100, StaticPriority = 1, Id = "C"},
            };
            

            //preemptive scheduling - reset dynamic priorities
            taskSet.ForEach(o => o.DynamicPriority = o.StaticPriority);
            var scheduleableUsingPreemption = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
            Assert.False(scheduleableUsingPreemption);
            
            //non-preemptive scheduling
            taskSet.ForEach(o => o.DynamicPriority = taskSet.Max(x=> x.StaticPriority));

            var scheduleableUsingNonPreemption = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
            Assert.True(scheduleableUsingNonPreemption);


            var resultFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"nonpreempt.txt");

            TaskFileReader.CreateTaskFile(taskSet,resultFile);

        }
        
        
        
        [Fact]
        public void TaskBlockingByResource()
        {
            
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 2, Period = 5, Id = "A", StaticPriority = 5},
                new Task {ExecutionTime = 4, Period = 20, Id = "B", StaticPriority = 4},
                new Task {ExecutionTime = 4, Period = 20, Id = "C", StaticPriority = 3},
                new Task {ExecutionTime = 5, Period = 200, Id = "D", StaticPriority = 2},
                new Task {ExecutionTime = 5, Period = 200, Id = "E", StaticPriority = 1}
            };
            
            var resources = new List<Resource>();

            var X = new Resource
            {
                Name = "X",
                tasks = new List<TaskUsingResource>
                {
                    new TaskUsingResource
                    {
                        Task = taskSet.Find(o => o.Id == "B"),
                        UsageTime = 2
                    },
                    new TaskUsingResource
                    {
                        Task = taskSet.Find(o => o.Id == "D"),
                        UsageTime = 3
                    },
                    new TaskUsingResource
                    {
                        Task = taskSet.Find(o => o.Id == "E"),
                        UsageTime = 1
                    }
                }
            };
            
            var Y = new Resource
            {
                Name = "Y",
                tasks = new List<TaskUsingResource>
                {
                    new TaskUsingResource
                    {
                        Task = taskSet.Find(o => o.Id == "C"),
                        UsageTime = 4
                    }
                }
            };
            
            var Z = new Resource
            {
                Name = "Z",
                tasks = new List<TaskUsingResource>
                {
                    new TaskUsingResource
                    {
                        Task = taskSet.Find(o => o.Id == "D"),
                        UsageTime = 1
                    },
                    new TaskUsingResource
                    {
                        Task = taskSet.Find(o => o.Id == "E"),
                        UsageTime = 2
                    }
                }
            };
            
            resources.Add(X);
            resources.Add(Y);
            resources.Add(Z);

            SystemWithSharedResources s = new SystemWithSharedResources
            {
                Tasks = taskSet,
                Resources = resources
            };

            foreach (var task in taskSet)
            {
                var blockingTimeNCPS = s.BlockingTimeOnAResourceNCPS(task);
                task.BlockingTime = blockingTimeNCPS;
            }
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
 
            foreach (var task in taskSet)
            {
                var blockingTimeNCPS = s.BlockingTimeOnAResourcePriorityCeiling(task);
                task.BlockingTime = blockingTimeNCPS;
            }
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
            var i = 0;
        }
    }
}