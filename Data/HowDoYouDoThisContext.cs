using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HowDoYouDoThis.Models;

namespace HowDoYouDoThis.Models
{
    public class HowDoYouDoThisContext : DbContext
    {
        public HowDoYouDoThisContext (DbContextOptions<HowDoYouDoThisContext> options)
            : base(options)
        {
        }

        public DbSet<HowDoYouDoThis.Models.QuestionItem> QuestionItem { get; set; }

        public DbSet<HowDoYouDoThis.Models.SolutionItem> SolutionItem { get; set; }

        public DbSet<HowDoYouDoThis.Models.UserItem> UserItem { get; set; }
    }
}
