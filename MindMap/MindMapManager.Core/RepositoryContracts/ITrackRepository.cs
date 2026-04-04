using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ITrackRepository
    {
        public IQueryable<Track> Get();
        public Track? GetById(int id);
        public void Update(Track track);
        public void Add(Track track);
        public void Remove(Track track);
        public void Save();
        public IQueryable<Track> GetIncludeUsersAndRoadMaps();
        public int Count();
        public Track? GetByIdIncludeUsersAndRoadMaps(int id);
        public IQueryable<Track> Search(string? SearchTirm);
        public Track? FindByName(string name);

    }
}
