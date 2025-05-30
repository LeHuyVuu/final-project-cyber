// Services/UserService.cs
using cybersoft_final_project.Entities;
using cybersoft_final_project.Infrastructure.UnitOfWork;
using cybersoft_final_project.Repositories;

namespace cybersoft_final_project.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unit;

        public UserService(UnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<(List<user>, int)> GetUsers(int page, int pageSize, string sortBy, string sortOrder)
        {
            var users = await _unit.UserRepository.GetUsers(page, pageSize, sortBy, sortOrder);
            var totalItems = await _unit.UserRepository.GetTotalUsers();
            return (users, totalItems);
        }

        public async Task<user?> GetById(int id)
        {
            return await _unit.UserRepository.GetById(id);
        }

        public async Task<bool> PhoneExists(string phone, int excludeId)
        {
            return await _unit.UserRepository.PhoneExists(phone, excludeId);
        }

        public async Task UpdateUser(user user, user updatedUser)
        {
            user.fullname = updatedUser.fullname;
            user.address = updatedUser.address;
            user.birthday = updatedUser.birthday;
            user.phone = updatedUser.phone;
            user.role = updatedUser.role;

            if (!string.IsNullOrEmpty(updatedUser.password))
            {
                user.password = updatedUser.password;
            }

            _unit.UserRepository.Update(user); // <<== Gọi hàm Update() để attach entity
            await _unit.SaveAsync();
        }

        
       
    }
}