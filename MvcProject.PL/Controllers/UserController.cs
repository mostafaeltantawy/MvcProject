using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcProject.DAL.Models;
using MvcProject.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcProject.PL.Controllers
{
    [Authorize]

    public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager , IMapper mapper )
		{
			_userManager = userManager;
            _mapper = mapper;
        }
		public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var Users = await _userManager.Users.Select(U => new UserViewModel() 
				{ Id = U.Id,
					Fname = U.Fname,
					Lname = U.Lname,
					PhoneNumber = U.PhoneNumber,
					Email = U.Email, 
					Roles = _userManager.GetRolesAsync(U).Result }).ToListAsync();
				return View(Users);

			}
			else
			{
				var  User = await _userManager.FindByEmailAsync(SearchValue);

				var mappedUser =User is not null? new UserViewModel()
				{
					Id = User.Id,
					Fname = User.Fname,
					Lname = User.Lname,
					PhoneNumber = User.PhoneNumber,
					Email = User.Email ,
					Roles = _userManager.GetRolesAsync(User).Result
				}: null ;
				return View(new List<UserViewModel> { mappedUser });

			}
		}

		public async Task<IActionResult> Details(string Id , string ViewName = "Details") 
		{
			if (Id is null)
				return BadRequest();
			var user = await _userManager.FindByIdAsync(Id);
			if(user == null)
				return NotFound();
			var MappedUser = _mapper.Map<ApplicationUser  , UserViewModel>(user);
			return View(ViewName,MappedUser );
		}

		
		public async Task<IActionResult> Edit(string Id) 
		{
			return await Details(Id, "Edit"); 
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserViewModel model , [FromRoute]string Id)
		{
			if (Id != model.Id)
				return BadRequest();
			if (ModelState.IsValid) 
			{
				try
				{
					var User = await _userManager.FindByIdAsync(model.Id);
					User.PhoneNumber = model.PhoneNumber;
					User.Fname = model.Fname;
					model.Lname = model.Lname;

					await _userManager.UpdateAsync(User);
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(string.Empty , ex.Message);
				}

			}
			return View(model);

		}

		public async Task<IActionResult> Delete(string id) 
		{
			return await Details(id , "Delete");
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmDelete( string id )
		{
			try
			{
				var User = await _userManager.FindByIdAsync (id);
				await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));


            }
            catch (Exception ex)
			{

				ModelState.AddModelError (string.Empty , ex.Message);
				return RedirectToAction("Error");
			}

		}
	}
}
