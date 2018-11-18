using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowDoYouDoThis.Models
{
    public class newQuestion
    {
        public string title { get; set; }
        public string description { get; set; }
        public string tag { get; set; }
        public IFormFile image { get; set; }
        public int authorID { get; set; }
    }
}
