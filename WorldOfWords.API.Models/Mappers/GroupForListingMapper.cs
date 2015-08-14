using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public class GroupForListingMapper: IGroupForListingMapper
    {
        public GroupForListingModel MapToGroupModel(Group group)
        {
            return new GroupForListingModel()
            {
                Id = group.Id,
                Name = group.Name,
                CourseId = group.Course.Id,
                CourseName = group.Course.Name
            };
        }

        public List<GroupForListingModel> MapToGroupModelCollection(List<Group> groups)
        {
            List<GroupForListingModel> models = new List<GroupForListingModel>();
            foreach(var group in groups)
            {
                models.Add(MapToGroupModel(group));
            }
            return models;
        }
    }
}
