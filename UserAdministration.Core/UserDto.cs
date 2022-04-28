using Microsoft.AspNet.Identity;

namespace UserAdministration.Core
{
    public class UserDto: PasswordHasher
    {
        public void TryObject()
        {
            UserDto objecto = new UserDto();

            objecto.HashPassword("holaa");
        }
        
    }
}
