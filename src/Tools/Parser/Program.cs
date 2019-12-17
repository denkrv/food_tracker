using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AngleSharp;
using Dasync.Collections;
using Food.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ParserTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true);
            if (!string.IsNullOrEmpty(envName))
            {
                builder = builder.AddJsonFile($"appsettings.{envName}.json", true, true);
            }

            IConfigurationRoot configuration = builder
                .AddEnvironmentVariables()
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<FoodContext>(opt =>
                    opt.UseNpgsql(configuration.GetConnectionString("FoodDb"))
                )
                .BuildServiceProvider();
            

            IDownloader[] downloaders = new IDownloader[] { new HttpDownloader() };

            var persister = new CalorizatorProductPersister(serviceProvider);

            var worker = new Crawler( downloaders, new IDownloadResultHandler[]{persister});

            await worker.Run(new Uri("http://www.calorizator.ru/product/all"));
        }
    }








}
