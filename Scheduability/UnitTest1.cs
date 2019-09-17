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
            
            ResponseTimeAnalysis.blockingTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '2').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '3').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '4').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '5').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '6').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '7').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '8').BlockingTime, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '9').BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').StartTimeWorstCase, 0);
            Assert.Equal(taskSet.Single(o => o.Id == '2').StartTimeWorstCase, 5);
            Assert.Equal(taskSet.Single(o => o.Id == '3').StartTimeWorstCase, 10);
            Assert.Equal(taskSet.Single(o => o.Id == '4').StartTimeWorstCase, 17);
            Assert.Equal(taskSet.Single(o => o.Id == '5').StartTimeWorstCase, 24);
            Assert.Equal(taskSet.Single(o => o.Id == '6').StartTimeWorstCase, 34);
            Assert.Equal(taskSet.Single(o => o.Id == '7').StartTimeWorstCase, 42);
            Assert.Equal(taskSet.Single(o => o.Id == '8').StartTimeWorstCase, 59);
            Assert.Equal(taskSet.Single(o => o.Id == '9').StartTimeWorstCase, 74);
            
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
            
            ResponseTimeAnalysis.blockingTime(taskSet);
            
            Assert.Equal(taskSet.Single(o => o.Id == '1').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '2').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '3').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '4').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '5').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '6').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '7').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '8').BlockingTime, 15);
            Assert.Equal(taskSet.Single(o => o.Id == '9').BlockingTime, 0);
            
            ResponseTimeAnalysis.WorstCaseStartTime(taskSet);
            
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
            
        } 
    }
}