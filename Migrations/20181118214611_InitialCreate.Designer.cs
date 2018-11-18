﻿// <auto-generated />
using HowDoYouDoThis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HowDoYouDoThis.Migrations
{
    [DbContext(typeof(HowDoYouDoThisContext))]
    [Migration("20181118214611_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("HowDoYouDoThis.Models.QuestionItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("authorID");

                    b.Property<string>("description");

                    b.Property<string>("diagramURL");

                    b.Property<string>("tag");

                    b.Property<string>("title");

                    b.HasKey("ID");

                    b.ToTable("QuestionItem");
                });

            modelBuilder.Entity("HowDoYouDoThis.Models.SolutionItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("answer");

                    b.Property<int>("authorID");

                    b.Property<string>("description");

                    b.Property<int>("questionID");

                    b.Property<int>("upvotes");

                    b.Property<string>("workingImage");

                    b.HasKey("ID");

                    b.ToTable("SolutionItem");
                });

            modelBuilder.Entity("HowDoYouDoThis.Models.UserItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TagID");

                    b.Property<bool>("admin");

                    b.Property<string>("dateCreated");

                    b.Property<string>("firstName");

                    b.Property<string>("lastName");

                    b.Property<string>("password");

                    b.Property<string>("username");

                    b.HasKey("ID");

                    b.ToTable("UserItem");
                });
#pragma warning restore 612, 618
        }
    }
}
