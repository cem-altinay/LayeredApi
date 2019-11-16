using Microsoft.EntityFrameworkCore;
using Sample.Api.Common.Exceptions;
using Sample.Data.Access.DAL;
using Sample.Data.Models;
using Sample.Security;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        private readonly ISecurityContext _securityContext;
 

        public UserService(IUnitOfWork db,ISecurityContext securityContext)
        {
            this._db = db;
            this._securityContext = securityContext;
      
        }


        public IQueryable<Users> Get()
        {
            var query = GetQuery();

            return query;
        }
        private IQueryable<Users> GetQuery() => _db.Query<Users>()
                                                  .Where(x => !x.IsDeleted);

        public Users GetUser(string userName) => _db.Query<Users>()
                                               .FirstOrDefault(x => x.Username == userName);

        public async Task<Users> GetUserAsync(string userName) => await _db.Query<Users>()
                                                          .FirstOrDefaultAsync(x => x.Username == userName);


        public void CreateTest()
        {
            Users user = new Users()
            {
                FirstName = "Cem Tranaction2",
                LastName = "Altınay DB First",
                IsDeleted = false,
                Password = "123456",
                Username = "cemaltinay"
            };
            _db.Add<Users>(user);        
            _db.SaveChanges();
        }
        public void SecurityContextTest() 
        {
            if(_securityContext.IsAdministrator)
            {
                _ = _securityContext.User; 
            }
        }


    }
}
