namespace SupplyManagements.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Logo { get; set; }
        public DateTime  RegistrationDate { get; set; } = DateTime.Now;
        public StatusSelection Status { get; set; } = StatusSelection.Pending;
        
        
    }
    public enum StatusSelection
    {
        Pending,Approved,Rejected
    }
}
