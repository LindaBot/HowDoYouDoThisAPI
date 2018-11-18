using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowDoYouDoThis.Models
{
    public class QuestionItem
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string tag { get; set; }
        public string diagramURL { get; set; }
        public int authorID { get; set; }
    }
}
