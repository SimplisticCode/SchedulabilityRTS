namespace Schedule.Data
{
    public class Task
    {
        public string Id { get; set; }
        public int Period { get; set; }
        public int ResponseTime { get; set; }
        public int StaticPriority { get; set; }
        public int DynamicPriority { get; set; }
        public int ExecutionTime{ get; set; }
        public decimal Utilization { get; set; }
        public int Offset { get; set; }
        public int Deadline { get; set; }
        public int BlockingTime { get; set; }
        public int StartTimeWorstCase { get; set; }
        public int FinishTimeWorstCase { get; set; }
        public int WorstCaseRunTime { get; set; }

        public void CalcUtilization()
        {
            this.Utilization = decimal.Divide(ExecutionTime, Period);
        }

        public override string ToString()
        {
            return $"Task {Id} has a worst case response time of {WorstCaseRunTime}. It has a deadline of {Deadline}. Static priority {StaticPriority}. Dynamic Priority {DynamicPriority}";
            ;
        }
    }
}