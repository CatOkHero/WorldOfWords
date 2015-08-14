using System;
using System.Collections.Generic;
using System.Web.Http;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Models;
using WorldOfWords.Domain.Services;

namespace WorldofWords.Controllers
{
    [WowAuthorization(Roles = "Teacher")]
    public class GroupController : BaseController
    {
        private readonly IGroupForListingMapper _groupForListingMapper;
        private readonly IGroupService _groupService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IWordProgressService _wordProgressService;
        private readonly IWordSuiteService _wordsuiteService;
        private readonly ICourseService _courseService;
        private readonly ICourseForGroupMapper _courseMapper;
        private readonly IGroupMapper _groupMapper;

        public GroupController(IGroupService groupService, IGroupForListingMapper groupForListingMapper, IEnrollmentService enrollmentService, 
            IWordProgressService wordProgressService, IWordSuiteService wordsuiteService, ICourseService courseService,
            ICourseForGroupMapper courseMapper, IGroupMapper groupMapper)
        {
            _groupService = groupService;
            _groupForListingMapper = groupForListingMapper;
            _enrollmentService = enrollmentService;
            _wordProgressService = wordProgressService;
            _wordsuiteService = wordsuiteService;
            _courseService = courseService;
            _courseMapper = courseMapper;
            _groupMapper = groupMapper;
        }

        public List<GroupForListingModel> Get()
        {
            return _groupForListingMapper.MapToGroupModelCollection(_groupService.GetAll(UserId));
        }

        public GroupForListingModel Get(int groupId)
        {
            var group = _groupService.GetById(groupId, UserId);
            if (group == null)
            {
                return null;
            }

            return _groupForListingMapper.MapToGroupModel(group);
        }

        [Route("getCourses")]
        public List<CourseForGroupModel> GetCourses()
        {
            return _courseMapper.MapToCourseModelCollection(_courseService.GetAllCourses(UserId));
        }

        public IHttpActionResult Post(GroupModel newGroup)
        {
            if (newGroup == null)
            {
                throw new ArgumentNullException("Parameter could not be null", "newGroup");
            }
            if (!_groupService.CheckIfGroupNameExists(newGroup))
            {
                if (_groupService.Add(newGroup))
                {
                    return Ok();
                }
            }
            return BadRequest(string.Format("Group {0} already exist!", newGroup.Name));
        }

        public IHttpActionResult Delete(int groupId)
        {
                List<Enrollment> enrollments = _enrollmentService.GetByGroupId(groupId);
                foreach (var enrollment in enrollments)
                {
                if (!(_wordProgressService.RemoveProgressesForEnrollment(enrollment.Id)
                    && _wordsuiteService.RemoveWordSuitesForEnrollment(enrollment.Id)))
                {
                    return BadRequest("Some problem occurred!");
                    }
                }
                if (_groupService.DeleteById(groupId))
                {
                    return Ok();
                }
            return BadRequest("Some problem occurred!");
        }
    }
}