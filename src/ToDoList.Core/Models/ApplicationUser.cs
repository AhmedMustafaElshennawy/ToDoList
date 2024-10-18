using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ToDolist ToDolist { get; set; }
    }
}
