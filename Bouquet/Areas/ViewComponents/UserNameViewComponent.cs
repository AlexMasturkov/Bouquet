﻿using Bouquet.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bouquet.Areas.ViewComponents
{
    public class UserNameViewComponent:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserNameViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claims.Value);
                return await Task.Run(() => View(userFromDb));
            }
            catch
            {
              return await Task.Run(() => View("None Exist"));
            }

        }
    }
}
