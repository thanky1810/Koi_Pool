﻿    using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
namespace KoiPool_Project.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string OrderCode {  get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double GardenArea { get; set; }
        public string ServiceType { get; set; }
        public string RequestContent { get; set; }
    }
}
