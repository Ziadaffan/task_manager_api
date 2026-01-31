using Domaine.Classes;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagerContext _context;

        public UserRepository(TaskManagerContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
                throw new KeyNotFoundException("User not found.");

            return user;
        }

        public async Task<User> Add(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email || u.Username == user.Username);

            if (existingUser != null)
                throw new InvalidOperationException("A user with the same email or username already exists.");

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }


        public async Task<bool> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
                return false;

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<User> Update(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => (u.Email == user.Email || u.Username == user.Username) && u.Id != user.Id);

            if (existingUser != null)
                throw new InvalidOperationException("A user with the same email or username already exists.");

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return user;
        }


        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<List<User>> GetByOrganisationId(Guid organisationId)
        {
            return _context.Users
                .Where(u => u.Organisation != null && u.Organisation.Id == organisationId)
                .ToListAsync();
        }

        public Task<List<User>> GetAll()
        {
            return _context.Users.Where(u => u.UserRole != Domaine.Enum.Role.Admin).Include(u => u.Organisation).ToListAsync();
        }
    }
}
