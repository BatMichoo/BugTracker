using API.AutoMapper;
using Core.BugService;
using Core.DbService;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<TrackerDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("BugTracker"));
            });

            builder.Services.AddScoped<ITrackerDbService, TrackerDbService>();
            builder.Services.AddScoped<IBugService, BugService>();

            builder.Services.AddAutoMapper(opt =>
            {
                opt.AddProfile(typeof(BugProfile));
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}
