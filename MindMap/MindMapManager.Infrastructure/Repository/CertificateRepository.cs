
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly AppDbContext _context;

        public CertificateRepository(AppDbContext context)
        {
            _context = context;
        }

        public Certificate? GetById(int id)
        {
            return _context.Certificates.FirstOrDefault(c => c.CertId == id);
        }

        public List<Certificate> GetByUserId(int userId)
        {
            return _context.Certificates
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public Certificate? GetByUserAndRoadmap(int userId, int roadmapId)
        {
            return _context.Certificates
                .FirstOrDefault(c => c.UserId == userId && c.Rid == roadmapId);
        }

        public void Add(Certificate certificate)
        {
            _context.Add(certificate);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}