using Microsoft.AspNetCore.Identity;

namespace KoiPool_Project.Models

{
    public class AppUserModel : IdentityUser
    {
        public string Occupation {  get; set; }


    }
}
