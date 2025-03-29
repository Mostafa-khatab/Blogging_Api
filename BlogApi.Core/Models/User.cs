using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Core.Models
{
    public class User : IdentityUser
    {
        public ICollection<Blog> Blogs { get; set; }
        public string? Bio {  get; set; }
    }
}
