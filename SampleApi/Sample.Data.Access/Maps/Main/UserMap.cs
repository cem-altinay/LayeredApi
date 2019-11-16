using Microsoft.EntityFrameworkCore;
using Sample.Data.Access.Maps.Common;
using Sample.Data.Models;


namespace Sample.Data.Access.Maps.Main
{
    public class UserMap : IMap
    {
        public void Visit(ModelBuilder builder) => builder.Entity<Users>()
                                                          .ToTable("Users")
                                                          .HasKey(x => x.Id);
    }
}
