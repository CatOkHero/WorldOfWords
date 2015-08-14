using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public interface ICourseForGroupMapper
    {
        CourseForGroupModel MapToCourseModel(Course course);
        List<CourseForGroupModel> MapToCourseModelCollection(IEnumerable<Course> courses);
    }
}
