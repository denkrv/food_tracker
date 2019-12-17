using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Food.Core.Data;
using Food.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ParserTool
{
    
    class CalorizatorProductPersister : IDownloadResultHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private HtmlDownloadParser _parser;

        public CalorizatorProductPersister(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _parser = new HtmlDownloadParser(ParseProducts);
        }


        private async Task ParseProducts(ICrawlingContext context, IDownloadResult result, IHtmlDocument doc, Func<Task> next)
        {
            var products = ParseProducts(doc).ToList();

            //no checking!

            using (var scope = _serviceProvider.CreateScope())
            using (var foodContext = scope.ServiceProvider.GetService<FoodContext>())
            {
                await foodContext.BeginTransactionAsync();
                await foodContext.AddRangeAsync(products);
                await foodContext.CommitTransactionAsync();
                Console.WriteLine($"created {products.Count} products");
            }

            await next.Invoke();
        }

        private IEnumerable<Product> ParseProducts(IHtmlDocument doc)
        {
            var rowsSelector = "#main-content > div > div.view-content > table.views-table > tbody > tr";
            var rows = doc.QuerySelectorAll(rowsSelector);
            foreach (var row in rows.OfType<IHtmlTableRowElement>())
            {
                if (row.Cells.Length == 6)
                {
                    var facts = new NutritionFacts();
                    var name = row.Cells[1].TextContent.Trim();

                    if (!string.IsNullOrEmpty(name))
                    {
                        if (decimal.TryParse(row.Cells[2].TextContent.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var prot))
                            facts.Protein = prot;

                        if (decimal.TryParse(row.Cells[3].TextContent.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var fat))
                            facts.Fat = fat;

                        if (decimal.TryParse(row.Cells[4].TextContent.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var carb))
                            facts.Carbohydrates = carb;

                        if (decimal.TryParse(row.Cells[5].TextContent.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var calores))
                            facts.Calories = calores;


                        yield return new Product()
                        {
                            Name = name,
                            Facts = facts
                        };
                    }
                }
            }
        }

        public bool Accept(IDownloadResult download) => _parser.Accept(download);

        public async Task Handle(ICrawlingContext context, IDownloadResult download)
        {
            await _parser.Handle(context, download);
        }
    }
}