using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowDoYouDoThis.Models
{
    public class QuestionItem
    {
        public int ID { get; set; }
        public string questionTitle { get; set; }
        public string questionDescription { get; set; }
        public string questionDiagramURL { get; set; }
        public string solutionIDs { get; set; }
    }
}
