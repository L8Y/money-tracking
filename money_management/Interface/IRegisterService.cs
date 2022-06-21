using money_management.Models;

namespace money_management.Interface
{
    public interface IRegisterService
    {
        public IEnumerable<Register> RegisteredUser();
        public int createUser(string Name, string EmailId, string Password);
        public IEnumerable<Register> isValidUser(string email, string password);
        public IEnumerable<Register> GetUserId(string email);
    }
}
