using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowDoYouDoThis.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HowDoYouDoThisContext(
                serviceProvider.GetRequiredService<DbContextOptions<HowDoYouDoThisContext>>()))
            {
                // Look for any movies.
                if (context.QuestionItem.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.QuestionItem.AddRange(
                    new QuestionItem
                    {
                        title = "How do you do this question?",
                        description = "This is the description",
                        diagramURL = "https://i.kym-cdn.com/photos/images/original/001/371/723/be6.jpg",
                        tag = "Life",
                        authorID = 1
            }
                );

                context.SolutionItem.AddRange(
                    new SolutionItem
                    {
                        questionID = 1,
                        answer = "42",
                        description = "This is how you get the anwer: 1 + 41 = 42",
                        workingImage = "https://live.worldbank.org/sites/default/files/styles/focal_point_bio_detail/public/experts/billgates.jpg",
                        authorID = 1,
                        upvotes = 0
                    }
                );

                context.UserItem.AddRange(
                    new UserItem
                    {
                        firstName = "Xiaobin",
                        lastName = "Lin",
                        username = "lindabot",
                        password = "password",
                        dateCreated = "18/11/2018",
                        admin = true,
                        TagID = "TAG-ID"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}