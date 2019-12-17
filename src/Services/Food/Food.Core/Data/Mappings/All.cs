using System;
using Food.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Food.Core.Data.Mappings
{
    class FoodConfiguration : IEntityTypeConfiguration<Domain.Food>
    {
        public void Configure(EntityTypeBuilder<Domain.Food> builder)
        {
            builder.ToTable("Foods");
            builder.HasDiscriminator<int>("FoodType")
                .HasValue<Product>(1)
                .HasValue<Recipe>(2);

            
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Name);
            builder.Property(p => p.Description);
            /*TODO: stop using owned entities!
             * https://github.com/aspnet/EntityFrameworkCore/issues/18299
             */
            builder.OwnsOne(p => p.Facts).WithOwner();

            builder
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN");
        }
    }

    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
        }
    }


    class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasMany(p => p.Ingridients).WithOne(p => p.Recipe).HasForeignKey(p => p.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    class IngridientConfiguration : IEntityTypeConfiguration<Ingridient>
    {
        public void Configure(EntityTypeBuilder<Ingridient> builder)
        {
            builder.ToTable("Ingridients");
            builder.HasKey(p => new { p.ProductId, p.RecipeId});
            builder.Property(p => p.Amount);

        }
    }

    class MealConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.ToTable("Meals");
            
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .UseIdentityColumn();

            builder.Property(p => p.IntakeTime)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); ;

            builder.OwnsOne(p => p.Facts).WithOwner();

            builder.HasMany(p => p.Items).WithOne(p => p.Meal).HasForeignKey(p => p.MealId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    class MealItemConfiguration : IEntityTypeConfiguration<MealItem>
    {
        public void Configure(EntityTypeBuilder<MealItem> builder)
        {
            builder.ToTable("MealItems");

            builder.HasKey(p => new { p.MealId, p.FoodId});

            builder.Property(p => p.Amount);

            builder.OwnsOne(p => p.Facts).WithOwner();
        }
    }
}
