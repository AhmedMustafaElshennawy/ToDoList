using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Infrastrucure.Authentication
{
    public class JwtSettings
    {
        public const string SectionName = "JWT";
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public string Key { get; init; } = null!;
        public double DurationInDays { get; set; }
    }
}
