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
            PasswordHasher<User> pw = new PasswordHasher<User>();
            user.PasswordHash = pw.HashPassword(user, user.Password);

            var newUser = await _storage.RegisterUserAsync(user);
            return newUser;
        }

        public async Task<string> RegisterResetPasswordCodeAsync(User user)
        {
            string code = Guid.NewGuid().ToString();
            await _storage.AddResetPasswordCodeAsync(code, user.Email);

            return code;
        }

        public async Task<User> ResetPasswordAsync(User user)
        {
            PasswordHasher<User> pw = new PasswordHasher<User>();
            user.PasswordHash = pw.HashPassword(user, user.Password);

            return await _storage.UpdateUserAsync(user);
        }

        public async Task<User> GetUserByResetPasswordTokenAsync(string code)
        {
            string email = await _storage.GetEmailByResetPasswordCodeAsync(code);

            if (string.IsNullOrWhiteSpace(email))
                return null;

            await _storage.DeleteResetPasswordCodeAsync(code);

            return await _storage.FindUserAsync(email);
        }

        public async Task<string> RegisterEmailConfirmationCodeAsync(User user)
        {
            string code = Guid.NewGuid().ToString();
            await _storage.AddEmailConfirmationCodeCodeAsync(code, user.Email);

            return code;
        }

        public async Task<User> GetUserByEmailConfirmationCodeAsync(string code)
        {
            string email = await _storage.GetEmailByConfirmationCodeAsync(code);

            if (string.IsNullOrWhiteSpace(email))
                return null;

            await _storage.DeleteEmailConfirmationCodeAsync(code);

            return await _storage.FindUserAsync(email);
        }

        public async Task<User> ConfirmEmailAsync(User user)
        {
            user.IsEmailConfirmed = true;

            return await _storage.UpdateUserAsync(user);
        }
    }
}
