using UserAdministration.Application.DTO;

namespace UserAdministration.Application.Interfaces
{
    public interface IUserUseCase
    {
        UserResponse Add(UserRequest user);
        UserResponse Update(UserUpdateRequest user);
        UserResponse Delete(UserUpdateRequest user);
    }
}
