using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowDoYouDoThis.Models
{
    public class NewSolution
    {
        public int questionID { get; set; }
        public string answer { get; set; }
        public string description { get; set; }
        public IFormFile workingImage { get; set; }
        public int authorID { get; set; }
    }
}
