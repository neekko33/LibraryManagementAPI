
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LibraryManagementAPI
{
    public class Program
    {
        public static IConfiguration Configuration;
        public static void Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<libraryContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("LibraryManagementDatabase"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"))
            );

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                    {
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C07F25AF-4EAF-649E-E086-AB21292E7095")),
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromMilliseconds(60)
                        };
                    });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
