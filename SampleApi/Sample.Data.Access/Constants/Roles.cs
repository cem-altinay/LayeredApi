using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Data.Access.Constants
{
    public class Roles
    {
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string AdministratorOrManager = "Administrator,Manager";

        public static List<string> GetRoles()
        {
            return new List<string>
            {
                Administrator,
                Manager,
                AdministratorOrManager
            };
        }
    }
}
