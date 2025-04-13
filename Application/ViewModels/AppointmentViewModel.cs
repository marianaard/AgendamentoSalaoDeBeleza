namespace Application.ViewModels
{
    public class AppointmentViewModel
    {
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
