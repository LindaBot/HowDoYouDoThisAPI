using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowDoYouDoThis.Models
{
    public class UserItem
    {
        public int ID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string dateCreated { get; set; }
        public bool admin { get; set; }
        public string TagID { get; set; }
    }
}
