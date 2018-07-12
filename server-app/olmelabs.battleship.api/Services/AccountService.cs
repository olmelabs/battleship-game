using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Repositories;

namespace olmelabs.battleship.api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IStorage _storage;
        public AccountService(IStorage storage)
        {
            _storage = storage;
        }
        public Task<User> FindUserAsync(string email)
        {
            return _storage.FindUserAsync(email);
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            //TODO: Add email confirmation flow
            user.IsEmailConfirmed = true;

            PasswordHasher<User> pw = new PasswordHasher<User>();
            user.PasswordHash = pw.HashPassword(user, user.Password);

            var newUser = await _storage.RegisterUserAsync(user);
            return newUser;
        }
    }
}
