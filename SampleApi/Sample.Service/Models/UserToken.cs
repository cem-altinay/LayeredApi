using Sample.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Service.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public Users User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
