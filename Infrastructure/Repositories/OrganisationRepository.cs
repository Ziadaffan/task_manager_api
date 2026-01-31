using Domaine.Classes;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly TaskManagerContext _context;

        public OrganisationRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<List<Organisation>> GetAll()
        {
            return await _context.Organisations.ToListAsync();
        }

        public async Task<Organisation> GetById(Guid id)
        {
            var organisation = await _context.Organisations.FindAsync(id);

            if (organisation == null)
                throw new KeyNotFoundException("Organisation not found.");

            return organisation;
        }

        public async Task<Organisation> Add(Organisation organisation)
        {
            if (organisation is null)
                throw new ArgumentNullException(nameof(organisation));

            var existingOrganisation = await _context.Organisations.FirstOrDefaultAsync(u => u.Name == organisation.Name);

            if (existingOrganisation != null)
                throw new InvalidOperationException("An organisation with the same name already exists.");

            _context.Organisations.Add(organisation);

            await _context.SaveChangesAsync();

            return organisation;
        }

        public async Task<Organisation> Update(Organisation organisation)
        {
            if (organisation is null)
                throw new ArgumentNullException(nameof(organisation));

            var existingOrganisation = await _context.Organisations
                .FirstOrDefaultAsync(o => (o.Name == organisation.Name) && o.Id != organisation.Id);

            if (existingOrganisation != null)
                throw new InvalidOperationException("An organisation with the same name already exists.");

            _context.Organisations.Update(organisation);

            await _context.SaveChangesAsync();

            return organisation;
        }

        public async Task<bool> Delete(Guid id)
        {
            var organisation = await _context.Organisations.FindAsync(id);

            if (organisation is null)
                return false;

            _context.Organisations.Remove(organisation);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
