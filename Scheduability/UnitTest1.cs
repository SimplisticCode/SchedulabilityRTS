using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Schedule;
using Xunit;

namespace Scheduability
{
    public class UnitTest1
    {
        [Fact]
        public void TestA()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 3, Period = 6, Id = 'A'},
                new Task {ExecutionTime = 1, Period = 12, Id = 'B'},
                new Task {ExecutionTime = 10, Period = 24, Id = 'C'}
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
                new Task {ExecutionTime = 2, Period = 5, Id = 'A'},
                new Task {ExecutionTime = 4, Period = 10, Id = 'B'},
                new Task {ExecutionTime = 3, Period = 15, Id = 'C'}
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
                new Task {ExecutionTime = 4, Period = 8, Id = 'A'},
                new Task {ExecutionTime = 2, Period = 10, Id = 'B'},
                new Task {ExecutionTime = 20, Period = 80, Id = 'C'}
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
                new Task {ExecutionTime = 2, Period = 8, Id = 'A'},
                new Task {ExecutionTime = 2, Period = 16, Id = 'B'},
                new Task {ExecutionTime = 12, Period = 24, Id = 'C'},
                new Task {ExecutionTime = 6, Period = 48, Id = 'D'}
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
                new Task {ExecutionTime = 5, Period = 50, Id = '1', Deadline = 15, StaticPriority = 9, DynamicPriority = 9},
                new Task {ExecutionTime = 5, Period = 60, Id = '2', Deadline = 25, StaticPriority = 8, DynamicPriority = 8},
                new Task {ExecutionTime = 7, Period = 80, Id = '3', Deadline = 30, StaticPriority = 7, DynamicPriority = 7},
                new Task {ExecutionTime = 7, Period = 200, Id = '4', Deadline = 40, StaticPriority =6, DynamicPriority = 6},
                new Task {ExecutionTime = 10, Period = 200, Id = '5', Deadline = 50, StaticPriority = 5, DynamicPriority = 5},
                new Task {ExecutionTime = 8, Period = 200, Id = '6', Deadline = 60, StaticPriority = 4, DynamicPriority = 4},
                new Task {ExecutionTime = 12, Period = 220, Id = '7', Deadline = 70, StaticPriority = 3, DynamicPriority = 3},
                new Task {ExecutionTime = 10, Period = 230, Id = '8', Deadline = 70, StaticPriority = 2, DynamicPriority = 2},
                new Task {ExecutionTime = 15, Period = 240, Id = '9', Deadline = 100, StaticPriority = 1, DynamicPriority =1}
            };
            taskSet.ForEach(o => o.CalcUtilization());
            
            ResponseTimeAnalysis.BlockingTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '2').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '3').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '4').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '5').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '6').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '7').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '8').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '9').BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').StartTimeWorstCase, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '2').StartTimeWorstCase, 5);
            Assert.Equal(taskSet.Single(o => o.Id == '3').StartTimeWorstCase, 10);
            Assert.Equal(taskSet.Single(o => o.Id == '4').StartTimeWorstCase, 17);
            Assert.Equal(taskSet.Single(o => o.Id == '5').StartTimeWorstCase, 24);
            Assert.Equal(taskSet.Single(o => o.Id == '6').StartTimeWorstCase, 34);
            Assert.Equal(taskSet.Single(o => o.Id == '7').StartTimeWorstCase, 42);
            Assert.Equal(taskSet.Single(o => o.Id == '8').StartTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == '9').StartTimeWorstCase, 74);
            
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == '1').FinishTimeWorstCase, 5);
            Assert.Equal(taskSet.Single(o => o.Id == '2').FinishTimeWorstCase, 10);
            Assert.Equal(taskSet.Single(o => o.Id == '3').FinishTimeWorstCase, 17);
            Assert.Equal(taskSet.Single(o => o.Id == '4').FinishTimeWorstCase, 24);
            Assert.Equal(taskSet.Single(o => o.Id == '5').FinishTimeWorstCase, 34);
            Assert.Equal(taskSet.Single(o => o.Id == '6').FinishTimeWorstCase, 42);
            Assert.Equal(taskSet.Single(o => o.Id == '7').FinishTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == '8').FinishTimeWorstCase, 74);
            Assert.Equal(taskSet.Single(o => o.Id == '9').FinishTimeWorstCase, 96); 
            
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == '1').WorstCaseRunTime, 5);
            Assert.Equal(taskSet.Single(o => o.Id == '2').WorstCaseRunTime, 10);
            Assert.Equal(taskSet.Single(o => o.Id == '3').WorstCaseRunTime, 17);
            Assert.Equal(taskSet.Single(o => o.Id == '4').WorstCaseRunTime, 24);
            Assert.Equal(taskSet.Single(o => o.Id == '5').WorstCaseRunTime, 34);
            Assert.Equal(taskSet.Single(o => o.Id == '6').WorstCaseRunTime, 42);
            Assert.Equal(taskSet.Single(o => o.Id == '7').WorstCaseRunTime, 59);
            Assert.Equal(taskSet.Single(o => o.Id == '8').WorstCaseRunTime, 74);
            Assert.Equal(taskSet.Single(o => o.Id == '9').WorstCaseRunTime, 96); 
            
            ResponseTimeAnalysis.ChangeDynamicPrioritiesToMeetDeadlines(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == '1').DynamicPriority, 9);
            Assert.Equal(taskSet.Single(o => o.Id == '2').DynamicPriority, 8);
            Assert.Equal(taskSet.Single(o => o.Id == '3').DynamicPriority, 7);
            Assert.Equal(taskSet.Single(o => o.Id == '4').DynamicPriority, 6);
            Assert.Equal(taskSet.Single(o => o.Id == '5').DynamicPriority, 5);
            Assert.Equal(taskSet.Single(o => o.Id == '6').DynamicPriority, 4);
            Assert.Equal(taskSet.Single(o => o.Id == '7').DynamicPriority, 8);
            Assert.Equal(taskSet.Single(o => o.Id == '8').DynamicPriority, 8);
            Assert.Equal(taskSet.Single(o => o.Id == '9').DynamicPriority, 1);  
        }

        [Fact]
        public void Test_BlockingTime1()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 5, Period = 50, Id = '1', Deadline = 15, StaticPriority = 9, DynamicPriority = 9},
                new Task {ExecutionTime = 5, Period = 60, Id = '2', Deadline = 25, StaticPriority = 8, DynamicPriority = 9},
                new Task {ExecutionTime = 7, Period = 80, Id = '3', Deadline = 30, StaticPriority = 7, DynamicPriority = 9},
                new Task {ExecutionTime = 7, Period = 200, Id = '4', Deadline = 40, StaticPriority =6, DynamicPriority = 9},
                new Task {ExecutionTime = 10, Period = 200, Id = '5', Deadline = 50, StaticPriority = 5, DynamicPriority = 9},
                new Task {ExecutionTime = 8, Period = 200, Id = '6', Deadline = 60, StaticPriority = 4, DynamicPriority = 9},
                new Task {ExecutionTime = 12, Period = 220, Id = '7', Deadline = 70, StaticPriority = 3, DynamicPriority = 9},
                new Task {ExecutionTime = 10, Period = 230, Id = '8', Deadline = 70, StaticPriority = 2, DynamicPriority = 9},
                new Task {ExecutionTime = 15, Period = 240, Id = '9', Deadline = 100, StaticPriority = 1, DynamicPriority =9}
            };
            taskSet.ForEach(o => o.CalcUtilization());
            
            ResponseTimeAnalysis.BlockingTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '2').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '3').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '4').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '5').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '6').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '7').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '8').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '9').BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').StartTimeWorstCase, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '2').StartTimeWorstCase, 20);
            Assert.Equal(taskSet.Single(o => o.Id == '3').StartTimeWorstCase, 25);
            Assert.Equal(taskSet.Single(o => o.Id == '4').StartTimeWorstCase, 32);
            Assert.Equal(taskSet.Single(o => o.Id == '5').StartTimeWorstCase, 39);
            Assert.Equal(taskSet.Single(o => o.Id == '6').StartTimeWorstCase, 49);
            Assert.Equal(taskSet.Single(o => o.Id == '7').StartTimeWorstCase, 67);
            Assert.Equal(taskSet.Single(o => o.Id == '8').StartTimeWorstCase, 79);
            Assert.Equal(taskSet.Single(o => o.Id == '9').StartTimeWorstCase, 74);
            
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == '1').FinishTimeWorstCase, 20);
            Assert.Equal(taskSet.Single(o => o.Id == '2').FinishTimeWorstCase, 25);
            Assert.Equal(taskSet.Single(o => o.Id == '3').FinishTimeWorstCase, 32);
            Assert.Equal(taskSet.Single(o => o.Id == '4').FinishTimeWorstCase, 39);
            Assert.Equal(taskSet.Single(o => o.Id == '5').FinishTimeWorstCase, 49);
            Assert.Equal(taskSet.Single(o => o.Id == '6').FinishTimeWorstCase, 57);
            Assert.Equal(taskSet.Single(o => o.Id == '7').FinishTimeWorstCase, 79);
            Assert.Equal(taskSet.Single(o => o.Id == '8').FinishTimeWorstCase, 89);
            Assert.Equal(taskSet.Single(o => o.Id == '9').FinishTimeWorstCase, 89); 
            
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == '1').WorstCaseRunTime, 20);
            Assert.Equal(taskSet.Single(o => o.Id == '2').WorstCaseRunTime, 25);
            Assert.Equal(taskSet.Single(o => o.Id == '3').WorstCaseRunTime, 32);
            Assert.Equal(taskSet.Single(o => o.Id == '4').WorstCaseRunTime, 39);
            Assert.Equal(taskSet.Single(o => o.Id == '5').WorstCaseRunTime, 49);
            Assert.Equal(taskSet.Single(o => o.Id == '6').WorstCaseRunTime, 57);
            Assert.Equal(taskSet.Single(o => o.Id == '7').WorstCaseRunTime, 79);
            Assert.Equal(taskSet.Single(o => o.Id == '8').WorstCaseRunTime, 89);
            Assert.Equal(taskSet.Single(o => o.Id == '9').WorstCaseRunTime, 89); 
        } 
        
        [Fact]
        public void Test_BlockingTime2()
        {
            var taskSet = new List<Task>
            {
                new Task {ExecutionTime = 5, Period = 50, Id = '1', Deadline = 15, StaticPriority = 9, DynamicPriority = 9},
                new Task {ExecutionTime = 5, Period = 60, Id = '2', Deadline = 25, StaticPriority = 8, DynamicPriority = 8},
                new Task {ExecutionTime = 7, Period = 80, Id = '3', Deadline = 30, StaticPriority = 7, DynamicPriority = 7},
                new Task {ExecutionTime = 7, Period = 200, Id = '4', Deadline = 40, StaticPriority =6, DynamicPriority = 6},
                new Task {ExecutionTime = 10, Period = 200, Id = '5', Deadline = 50, StaticPriority = 5, DynamicPriority = 5},
                new Task {ExecutionTime = 8, Period = 200, Id = '6', Deadline = 60, StaticPriority = 4, DynamicPriority = 4},
                new Task {ExecutionTime = 12, Period = 220, Id = '7', Deadline = 70, StaticPriority = 3, DynamicPriority = 8},
                new Task {ExecutionTime = 10, Period = 230, Id = '8', Deadline = 70, StaticPriority = 2, DynamicPriority = 8},
                new Task {ExecutionTime = 15, Period = 240, Id = '9', Deadline = 100, StaticPriority = 1, DynamicPriority =1}
            };
            taskSet.ForEach(o => o.CalcUtilization());
            
            ResponseTimeAnalysis.BlockingTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '2').BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == '3').BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == '4').BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == '5').BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == '6').BlockingTime, 12);
            Assert.Equal(taskSet.Single(o => o.Id == '7').BlockingTime, 10);
            Assert.Equal(taskSet.Single(o => o.Id == '8').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '9').BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').StartTimeWorstCase, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '2').StartTimeWorstCase, 17);
            Assert.Equal(taskSet.Single(o => o.Id == '3').StartTimeWorstCase, 22);
            Assert.Equal(taskSet.Single(o => o.Id == '4').StartTimeWorstCase, 29);
            Assert.Equal(taskSet.Single(o => o.Id == '5').StartTimeWorstCase, 36);
            Assert.Equal(taskSet.Single(o => o.Id == '6').StartTimeWorstCase, 46);
            Assert.Equal(taskSet.Single(o => o.Id == '7').StartTimeWorstCase, 57);
            Assert.Equal(taskSet.Single(o => o.Id == '8').StartTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == '9').StartTimeWorstCase, 74);
            
            ResponseTimeAnalysis.WorstCaseFinishTime(taskSet);
            Assert.Equal(taskSet.Single(o => o.Id == '1').FinishTimeWorstCase, 5);
            Assert.Equal(taskSet.Single(o => o.Id == '2').FinishTimeWorstCase, 22);
            Assert.Equal(taskSet.Single(o => o.Id == '3').FinishTimeWorstCase, 29);
            Assert.Equal(taskSet.Single(o => o.Id == '4').FinishTimeWorstCase, 36);
            Assert.Equal(taskSet.Single(o => o.Id == '5').FinishTimeWorstCase, 46);
            Assert.Equal(taskSet.Single(o => o.Id == '6').FinishTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == '7').FinishTimeWorstCase, 69);
            Assert.Equal(taskSet.Single(o => o.Id == '8').FinishTimeWorstCase, 69);
            Assert.Equal(taskSet.Single(o => o.Id == '9').FinishTimeWorstCase, 96); 
            
            ResponseTimeAnalysis.WorstCaseResponseTimeAnalysis(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').WorstCaseRunTime, 5);
            Assert.Equal(taskSet.Single(o => o.Id == '2').WorstCaseRunTime, 22);
            Assert.Equal(taskSet.Single(o => o.Id == '3').WorstCaseRunTime, 29);
            Assert.Equal(taskSet.Single(o => o.Id == '4').WorstCaseRunTime, 36);
            Assert.Equal(taskSet.Single(o => o.Id == '5').WorstCaseRunTime, 46);
            Assert.Equal(taskSet.Single(o => o.Id == '6').WorstCaseRunTime, 59);
            Assert.Equal(taskSet.Single(o => o.Id == '7').WorstCaseRunTime, 69);
            Assert.Equal(taskSet.Single(o => o.Id == '8').WorstCaseRunTime, 69);
            Assert.Equal(taskSet.Single(o => o.Id == '9').WorstCaseRunTime, 96); 
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
            Assert.Equal(taskSet[0].StaticPriority, 0);
            Assert.Equal(taskSet[0].DynamicPriority, 0);
            
            
            Assert.Equal(taskSet[1].ExecutionTime, 52);
            Assert.Equal(taskSet[1].Period, 140);
            Assert.Equal(taskSet[1].Deadline, 154);
            Assert.Equal(taskSet[1].StaticPriority, 0);
            Assert.Equal(taskSet[1].DynamicPriority, 0);

            var exist = ResponseTimeAnalysis.PerformScheduabilityStudy(taskSet, resultReadInDto.NumberOfPropertiesSpecificed);
            var a = 1;
        }
        
        [Fact]
        public void OnlyScheduleableUsingPreemption()
        {
            var fileName = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"preempt.txt");
            var resultReadInDto  = TaskFileReader.ReadInTasksFromFile(fileName);
            var taskSet = resultReadInDto.tasks;
            
            //non-preemptive scheduling
            taskSet.ForEach(o => o.DynamicPriority = taskSet.Max(x=> x.StaticPriority));

            var scheduleableUsingNonPreemption = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
            Assert.False(scheduleableUsingNonPreemption);
            
            //preemptive scheduling - reset dynamic priorities
            taskSet.ForEach(o => o.DynamicPriority = 1);
            var scheduleableUsingPreemption = ResponseTimeAnalysis.FeasibilityStudy(taskSet);
            Assert.True(scheduleableUsingPreemption);

        }
        
   
        [Fact]
        public void OnlyScheduleableUsingNONPreemption()
        {
            var fileName = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"nonpreempt.txt");
            var resultReadInDto  = TaskFileReader.ReadInTasksFromFile(fileName);
            var taskSet = resultReadInDto.tasks;
            
            //non-preemptive scheduling
            taskSet.ForEach(o => o.DynamicPriority = taskSet.Max(x=> x.StaticPriority));

            var scheduleableUsingNonPreemption = ResponseTimeAnalysis.FeasibilityUsingResponseTimeAnalysis(taskSet);
            Assert.True(scheduleableUsingNonPreemption);
            
            //preemptive scheduling - reset dynamic priorities
            taskSet.ForEach(o => o.DynamicPriority = 1);
            var scheduleableUsingPreemption = ResponseTimeAnalysis.FeasibilityStudy(taskSet);
            Assert.False(scheduleableUsingPreemption);

        }
    }
}