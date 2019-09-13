namespace Scheduability
{
    public class Task
    {
        public char Id { get; set; }
        public int Period { get; set; }
        public int ResponseTime { get; set; }
        public int Priority { get; set; }
        public int ExecutionTime{ get; set; }
        public decimal Utilization { get; set; }

        public void CalcUtilization()
        {
            this.Utilization = decimal.Divide(ExecutionTime, Period);
        }
    }
}