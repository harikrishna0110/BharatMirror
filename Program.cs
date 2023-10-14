using BharatMirror.Data;
using BharatMirror.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;



namespace BharatMirror
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            
            builder.Services.AddDbContext<AppDbContext>();  
            builder.Services.AddTransient<IUserRepository, SQLUserRepository>();
            builder.Services.AddTransient<IAdvertisementRepository, SQLAdvertisementRepository>();
            builder.Services.AddTransient<IMailService, MailService>();
            builder.Services.AddSession();
             //builder.Services.AddTransient<IAdvertisementRepository, SQLAdvertisementRepository>();
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
           

            try
            {
                using (var app = builder.Build())
                {

                    // Configure the HTTP request pipeline.
                    if (!app.Environment.IsDevelopment())
                    {
                        app.UseExceptionHandler("/Home/Error");
                        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                        app.UseHsts();
                    }

                    app.UseHttpsRedirection();
                    app.UseSession();
                    app.UseStaticFiles();

                    app.UseRouting();

                    app.UseAuthorization();

                    app.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=User}/{action=Index}/{id?}");

                    app.Run();
                }
            }
            catch (Exception ex)

            { 
                
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}