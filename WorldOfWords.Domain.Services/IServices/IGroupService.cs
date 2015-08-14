using System.Collections.Generic;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.Domain.Services
{
    public interface IGroupService
    {
        List<Group> GetAll(int userId);
        bool CheckIfGroupNameExists(GroupModel groupModel);
        bool Add(GroupModel groupModel);
        Group GetById(int groupId, int userId);
        bool DeleteById(int groupId);
    }
}