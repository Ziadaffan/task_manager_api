using Application.Interfaces;
using Domaine.Classes;
using Infrastructure.Interface;

namespace Application.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IOrganisationRepository _organisationRepository;

        public OrganisationService(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<List<Organisation>> GetAll()
        {
            return await _organisationRepository.GetAll();
        }

        public async Task<Organisation> GetById(Guid id)
        {
            return await _organisationRepository.GetById(id);
        }

        public async Task<Organisation> Add(Organisation organisation)
        {
            return await _organisationRepository.Add(organisation);
        }

        public async Task<Organisation> Update(Organisation organisation)
        {
            return await _organisationRepository.Update(organisation);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _organisationRepository.Delete(id);
        }
    }
}
