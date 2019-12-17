using System.Collections.Generic;
using NpgsqlTypes;

namespace Food.Core.Domain
{
    public interface IEnitity
    {
        int Id { get; }
            
    }

    public abstract class Food : IEnitity
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// normalized per 100g
        /// </summary>
        public NutritionFacts Facts { get; set; }

        public NpgsqlTsVector SearchVector { get; set; }
    }

    public class Product : Food
    {

    }

    public class Ingridient
    {
        public int RecipeId { get; set; }

        public int ProductId { get; set; }
        
        public Recipe Recipe { get; set; }

        public Product Product { get; set; }

        /// <summary>
        /// in gramms
        /// </summary>
        public decimal Amount { get; set; }
    }


    public class Recipe : Food
    {
        public string Content { get; set; }

        public List<Ingridient> Ingridients { get; set; }
    }

}