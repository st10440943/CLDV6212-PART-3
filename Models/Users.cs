using Microsoft.AspNetCore.Identity;

namespace ABC_Retail.Models
{
    public class Users : IdentityUser
    {
       
        public string? Role { get; set; }  
    }
}

