using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppRoles : IdentityRole<Guid>
    {
        public const string Admin = "Admin";

        public const string PreAdmin = "PreAdmin";

        public const string Teacher = "Teacher";

        public const string Student = "Student";

        public string? Description {  get; set; }
    }
}

