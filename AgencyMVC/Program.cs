using AgencyMVC.DAL;
using AgencyMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgencyMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
           builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt =>

            opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))

            );
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.MaxFailedAccessAttempts =3;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            
            var app = builder.Build();

                       
            app.MapControllerRoute(
                "admin",
                "{area:exists}/{controller=home}/{action=index}/{Id?}");

            app.MapControllerRoute(
                "default",
                "{controller=home}/{action=index}/{Id?}");

            app.UseStaticFiles();
            app.Run();
        }
    }
}
