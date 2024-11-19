namespace KoiPool_Project.Models
{
    public class UserOrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string FullName { get; set; }

        public DateTime CreatedTime { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double GardenArea { get; set; }
        public string ServiceType { get; set; }
        public string RequestContent { get; set; }
        
        public int Status { get; set; }
    }
}
