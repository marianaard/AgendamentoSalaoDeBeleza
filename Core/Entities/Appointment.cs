namespace Core.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Service { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsCancelled { get; set; }
    }
}
