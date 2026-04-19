using MindMapManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ITopicRepository
    {
        public void Add(Topic topic);
        public void Update(Topic topic);
        public Topic? GetByIdWithResources(int id);
        public Topic? GetById(int id);
        public Topic? GetWithLevel(int id);

        public void Delete(Topic topic);
        public void Save();
        public bool IsExist(Topic topic);
        public int CountPerLevel(int levelId);
    }
}
