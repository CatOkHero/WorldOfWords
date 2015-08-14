using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public interface IGroupForListingMapper
    {
        GroupForListingModel MapToGroupModel(Group group);
        List<GroupForListingModel> MapToGroupModelCollection(List<Group> groups);
    }
}
