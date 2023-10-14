namespace BharatMirror.Models
{
    public interface IUserRepository
    {
        Users GetUsers(int Id);
        IEnumerable<Users> GetAllUserss();
        Users Add(Users Users);
        Users Update(Users UsersChanges);

        Users GetUserByEmail(String email);

        Users Delete(int Id);
    }

}
