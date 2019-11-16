using Sample.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service
{
    public interface IUserService
    {
        Users GetUser(string userName);
        Task<Users> GetUserAsync(string userName);

        void CreateTest();
    }
}
