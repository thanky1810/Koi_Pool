using System.ComponentModel.DataAnnotations;
namespace KoiPool_Project.Models
{
    public class RequestModel
    {
        [Key] 
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public double GardenArea { get; set; }
        public string ServiceType { get; set; }
        public string RequestContent { get; set; }
        public string Rating { get; set; }
    }
}
