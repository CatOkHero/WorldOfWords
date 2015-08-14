using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public interface IGroupMapper
    {
        Group MapToGroup(GroupModel groupMode);
        GroupModel MapToGroupModel(Group group);
        List<GroupModel> MapToGroupModelCollection(List<Group> groups);
    }
}
