using MindMapManager.Core.Entities;


namespace MindMapManager.Core.RepositoryContracts
{
    public interface IRoadmapRepository
    {
        public IQueryable<Roadmap> Get();
        public Roadmap? GetByIdWithDetails(int id);
        public Roadmap? GetByIdWithLevelsAndProgress(int id);
        public IQueryable<Roadmap> Search(string? searchTirm);
        public Roadmap? FindByName(string roadmapName);
        public bool Existed(string roadmapName , int trackId , int? execludId);
        public void Add(Roadmap roadmap);
        public void Remove(Roadmap roadmap);
        public void Save();
        public void Update(Roadmap roadmap);
        public Roadmap? GetById(int id);
        public IQueryable<Roadmap> FilterByTrackId(int trackId);
    }
}
