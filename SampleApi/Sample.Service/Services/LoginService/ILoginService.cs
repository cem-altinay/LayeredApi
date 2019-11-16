using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Service
{
    public interface ILoginService
    {
        Models.UserToken Authenticate(string username, string password);
    }
}
