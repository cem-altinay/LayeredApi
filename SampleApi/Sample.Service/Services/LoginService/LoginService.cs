using Sample.Data.Access.DAL;
using Sample.Data.Models;
using Sample.Security;
using Sample.Security.Auth;
using Sample.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Sample.Api.Common.Exceptions;
using Sample.Data.Access.Helpers;
using Sample.Data.Access.Constants;

namespace Sample.Service.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _db;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IUserService _userService;
        private readonly ISecurityContext _securityContext;
        private Random _random;

        public LoginService(IUnitOfWork db, ITokenBuilder tokenBuilder, IUserService userService, ISecurityContext securityContext)
        {
            this._db = db;
            this._tokenBuilder = tokenBuilder;
            this._userService = userService;
            this._securityContext = securityContext;
            this._random = new Random();
        }

        public UserToken Authenticate(string username, string password)
        {
            var user = _db.Query<Users>().FirstOrDefault(w => w.Username == username);

            if (user is null)
            {
                throw new BadRequestException("username/password aren't right");
            }

            if (string.IsNullOrWhiteSpace(password) || !user.Password.VerifyCrypt(password))
            {
                throw new BadRequestException("username/password aren't right");
            }
            
            var expiresIn = DateTime.Now + TokenAuthOption.ExpiresSpan;
            var token = _tokenBuilder.Build(user.Username, Roles.GetRoles().ToArray(), expiresIn);

            return new UserToken
            {
                ExpiresAt = expiresIn,
                Token = token,
                User = user
            };
        }
    }
}
