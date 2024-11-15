using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace KoiPool_Project.Models

{
    public class AppUserModel : IdentityUser
    { 
        [Key]
        public string Occupation {  get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }

    }
}
