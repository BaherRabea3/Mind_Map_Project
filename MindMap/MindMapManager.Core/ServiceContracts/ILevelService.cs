using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface ILevelService
    {
        public LevelResoponse GetWithDetails(int id);
        public int AddLevel(LevelRequest topicRequest);
        public void UpdateLevel(int id, LevelRequest levelRequest);
        public void DeleteLevel(int id);
    }
}
