namespace LearningStarter.Entities
{
    public class PayRate
    {
        public int Id { get; set; }
        public int Rate { get; set; }
    }

    public class PayRateGetDto
    {
        public int Id { get; set; }
        public int Rate { get; set; }
    }

    public class PayRateCreateDto
    {
        public int Rate { get; set; }
    }
}
