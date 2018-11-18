using Microsoft.EntityFrameworkCore.Migrations;

namespace HowDoYouDoThis.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    tag = table.Column<string>(nullable: true),
                    diagramURL = table.Column<string>(nullable: true),
                    authorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SolutionItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    questionID = table.Column<int>(nullable: false),
                    answer = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    workingImage = table.Column<string>(nullable: true),
                    upvotes = table.Column<int>(nullable: false),
                    authorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionItem", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    dateCreated = table.Column<string>(nullable: true),
                    admin = table.Column<bool>(nullable: false),
                    TagID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItem", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionItem");

            migrationBuilder.DropTable(
                name: "SolutionItem");

            migrationBuilder.DropTable(
                name: "UserItem");
        }
    }
}
