using BharatMirror.Data;

namespace BharatMirror.Models
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        public SQLUserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Users Add(Users Users)
        {
            context.Users.Add(Users);
            context.SaveChanges();
            return Users;
        }

        public Users Delete(int Id)
        {
            Users user = context.Users.Find(Id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            return user;
        }

        public IEnumerable<Users> GetAllUserss()
        {
            return context.Users;
        }

        public Users GetUsers(int Id)
        {
            return context.Users.FirstOrDefault(m => m.Id == Id);
        }

        public Users GetUserByEmail(String email)
        {
            return context.Users.FirstOrDefault(m => m.Email == email);
        }

        public Users Update(Users UsersChanges)
        {
            var user = context.Users.Attach(UsersChanges);
            user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return UsersChanges;
        }
    }
}





