using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowDoYouDoThis.Models
{
    public class SolutionItem
    {
        public int ID { get; set; }
        public int questionID { get; set; }
        public string answer { get; set; }
        public string description { get; set; }
        public string workingImage { get; set; }
        public string author { get; set; }
        public int authorID { get; set; }
        public int upvotes { get; set; }
    }
}
