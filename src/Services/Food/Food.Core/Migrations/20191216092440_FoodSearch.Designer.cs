﻿// <auto-generated />
using System;
using Food.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

namespace Food.Core.Migrations
{
    [DbContext(typeof(FoodContext))]
    [Migration("20191216092440_FoodSearch")]
    partial class FoodSearch
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0-preview3.19554.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Food.Core.Domain.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("FoodType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .HasColumnType("tsvector");

                    b.HasKey("Id");

                    b.HasIndex("SearchVector")
                        .HasAnnotation("Npgsql:IndexMethod", "GIN");

                    b.ToTable("Foods");

                    b.HasDiscriminator<int>("FoodType");
                });

            modelBuilder.Entity("Food.Core.Domain.Ingridient", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("RecipeId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.HasKey("ProductId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingridients");
                });

            modelBuilder.Entity("Food.Core.Domain.Meal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("IntakeTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("Food.Core.Domain.MealItem", b =>
                {
                    b.Property<int>("MealId")
                        .HasColumnType("integer");

                    b.Property<int>("FoodId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.HasKey("MealId", "FoodId");

                    b.HasIndex("FoodId");

                    b.ToTable("MealItems");
                });

            modelBuilder.Entity("Food.Core.Domain.Product", b =>
                {
                    b.HasBaseType("Food.Core.Domain.Food");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Food.Core.Domain.Recipe", b =>
                {
                    b.HasBaseType("Food.Core.Domain.Food");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Food.Core.Domain.Food", b =>
                {
                    b.OwnsOne("Food.Core.Domain.NutritionFacts", "Facts", b1 =>
                        {
                            b1.Property<int>("FoodId")
                                .HasColumnType("integer");

                            b1.Property<decimal>("Calories")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Carbohydrates")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Fat")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Protein")
                                .HasColumnType("numeric");

                            b1.HasKey("FoodId");

                            b1.ToTable("Foods");

                            b1.WithOwner()
                                .HasForeignKey("FoodId");
                        });
                });

            modelBuilder.Entity("Food.Core.Domain.Ingridient", b =>
                {
                    b.HasOne("Food.Core.Domain.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Food.Core.Domain.Recipe", "Recipe")
                        .WithMany("Ingridients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Food.Core.Domain.Meal", b =>
                {
                    b.OwnsOne("Food.Core.Domain.NutritionFacts", "Facts", b1 =>
                        {
                            b1.Property<int>("MealId")
                                .HasColumnType("integer");

                            b1.Property<decimal>("Calories")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Carbohydrates")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Fat")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Protein")
                                .HasColumnType("numeric");

                            b1.HasKey("MealId");

                            b1.ToTable("Meals");

                            b1.WithOwner()
                                .HasForeignKey("MealId");
                        });
                });

            modelBuilder.Entity("Food.Core.Domain.MealItem", b =>
                {
                    b.HasOne("Food.Core.Domain.Food", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Food.Core.Domain.Meal", "Meal")
                        .WithMany("Items")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Food.Core.Domain.NutritionFacts", "Facts", b1 =>
                        {
                            b1.Property<int>("MealItemMealId")
                                .HasColumnType("integer");

                            b1.Property<int>("MealItemFoodId")
                                .HasColumnType("integer");

                            b1.Property<decimal>("Calories")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Carbohydrates")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Fat")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Protein")
                                .HasColumnType("numeric");

                            b1.HasKey("MealItemMealId", "MealItemFoodId");

                            b1.ToTable("MealItems");

                            b1.WithOwner()
                                .HasForeignKey("MealItemMealId", "MealItemFoodId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
