using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Application.Services.Email.Dto
{
    public class EmailDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string message { get; set; } 
        // Avoid adding types like Encoding here
    }
}
