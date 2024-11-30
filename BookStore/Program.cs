
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string txt = "Bookstore Api";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(op =>
            {
                op.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date"
                });
                op.EnableAnnotations();
                op.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BookStore API - V1",
                    Version = "v1",
                    Description = "An API for managing books, authors, and customer orders in an online bookstore. This API allows users to browse available books, manage their orders, and view book details.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ahmed Salah Mohammed",
                        Email = "ahmedsalahmohammed98@gmail.com"
                    }
                });
            });
            builder.Services.AddDbContext<BookStoreContext>(op => op.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("BookConn")));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BookStoreContext>();
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
                op =>
                {
                    op.SaveToken = true;
                    string key = "Book store api secret key for token validation";
                    var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = secretKey,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(txt);


            app.MapControllers();

            app.Run();
        }
    }
}
