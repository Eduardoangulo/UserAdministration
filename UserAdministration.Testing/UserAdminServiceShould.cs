using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using UserAdministration.Application;
using UserAdministration.Application.DTO;
using Xunit;

namespace UserAdministration.Testing
{
    public class UserAdminServiceShould
    {
        private UserUseCase _unitUnderTesting = null;
        public UserAdminServiceShould()
        {
            if (_unitUnderTesting == null)
            {
                _unitUnderTesting = new UserUseCase();
            }
        }

        [Fact]
        public void CreateUser()
        {
            var userRequest = new UserRequest { 
                
            };

            var result = _unitUnderTesting.Add(userRequest);

            Assert.IsTrue(result.Id > 0, "User created");
        }
    }
}
