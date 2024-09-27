using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MvcProject.BLL.Interfaces;
using MvcProject.BLL.Repositories;
using MvcProject.DAL.Data;
using MvcProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MvcProject.PL.MappingProfiles;

namespace MvcProject.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

			#region Allow DI


			Builder.Services.AddControllersWithViews(); // Register built in Builder.Builder.Services required by mvc


			//Builder.Services.AddScoped<ApplicationDbContext>();
			//Builder.Services.AddScoped<DbContextOptions<ApplicationDbContext>>();
			Builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection")));

			Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

			Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

			Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>() { new EmployeeProfile(), new UserProfile(), new RoleProfile() }));

			Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
			}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


			Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
			{
				options.LoginPath = "Account/Login";
				options.AccessDeniedPath = "Home/Error";
			});




			#endregion

			var app = Builder.Build();
			#region Configure MIDDLewares
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Account}/{action=login}/{id?}");
			});
			#endregion

			app.Run(); 
		}


	}
}
