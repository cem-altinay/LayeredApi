using Microsoft.AspNetCore.Http;
using Sample.Api.Models.User;
using Sample.Data.Access.Constants;
using Sample.Security;
using Sample.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Api.Security
{
    public class SecurityContext : ISecurityContext
    {


        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;
        private UserModel _user;

        public SecurityContext()
        {

        }

        public UserModel User
        {
            get
            {
                if (_user != null) return _user;

                if(!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new UnauthorizedAccessException();
                }

                var username = _contextAccessor.HttpContext.User.Identity.Name;
                var user = _userService.GetUser(username);
                  

                if (user == null)
                {
                    throw new UnauthorizedAccessException("User is not found");
                }
                _user = new UserModel()
                {
                    Id=user.Id,
                    FirstName=user.FirstName,
                    IsDeleted=user.IsDeleted,
                    LastName=user.LastName,
                    Password=user.Password,
                    Username=user.Username
                };

                return _user;

            }
        }

        public bool IsAdministrator => Roles.GetRoles().Any();
    }
}
