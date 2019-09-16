using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
 
    }
}