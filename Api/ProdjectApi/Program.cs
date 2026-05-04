using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProdjectApi.Configuration;
using ProdjectApi.Data;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Services.Currencies;
using ProdjectApi.Service;
using ProdjectApi.Service.Contracts;
using ProdjectApi.Service.Rooms;
using System.Text.Json.Serialization;

namespace ProdjectApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddScoped<SmtpEmailService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddHostedService<VerificationCleanupService>();
            builder.Services.AddSingleton<RoomCodeGenerator>();
            builder.Services.AddSingleton<ICurrencyRateService, CbrCurrencyRateService>();
            builder.Services.AddDbContext<ProjectDbContext>(options => options.UseNpgsql(connectionString));
            builder.Services.AddHttpClient();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            await app.RunAsync();
        }
    }

}
