using System.Collections.Generic;
using System.Linq;
using Schedule.Data;

namespace Schedule.Calculator
{
    public static class Calculator
    {
        public static List<long> FindFrameSize(List<Task> data, long hyperPeriod)
        {
            //Criteria 1:
            var maxExecutionTime = data.Max(o => o.ExecutionTime);
            var frameSize = new List<long>();
            foreach (var period in data.Where(o => o.Period > o.ExecutionTime).Select(o => o.Period).Distinct().ToList())
            {
                if(DivideEvenly(period, hyperPeriod))
                    frameSize.Add(period);
            }


            var result = multipleExecution(frameSize, data);
            return result;
        }

        private static List<long> multipleExecution(List<long> dividesHyperPeriod, List<Task> data)
        {
            var result = (from f in dividesHyperPeriod
                from d in data
                where 2 * f - MathUtils.GCD(d.Period, f) < 10000 * d.Deadline
                select f);
            return result.Distinct().ToList();
        }

        public static long FindHyperPeriod(List<Task> data)
        {
            var distinctPeriods = data.Select(o => (long) o.Period).Distinct().ToArray();
            var hyperPeriod = MathUtils.LCM(distinctPeriods);
            return hyperPeriod;
        }


        private static bool DivideEvenly(long period, long HyperPeriod)
        {
            return (HyperPeriod % period == 0);
        }
    }
}