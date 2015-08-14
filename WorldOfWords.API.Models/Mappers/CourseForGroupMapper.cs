using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public class CourseForGroupMapper : ICourseForGroupMapper
    {
        public CourseForGroupModel MapToCourseModel(Course course)
        {
            return new CourseForGroupModel() { Id = course.Id, Name = course.Name };
        }
        public List<CourseForGroupModel> MapToCourseModelCollection(IEnumerable<Course> courses)
        {
            List<CourseForGroupModel> courseModels = new List<CourseForGroupModel>();
            foreach(var course in courses)
            {
                courseModels.Add(MapToCourseModel(course));
            }
            return courseModels;
        }
    }
}
