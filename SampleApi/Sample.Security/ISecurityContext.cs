using Sample.Api.Models.User;
using Sample.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Security
{
    public interface ISecurityContext
    {
        UserModel User { get; }

        bool IsAdministrator { get; }
    }
}
