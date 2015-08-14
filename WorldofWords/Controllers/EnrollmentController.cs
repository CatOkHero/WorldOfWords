using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Models;
using WorldOfWords.Domain.Services;

namespace WorldofWords.Controllers
{
    [WowAuthorization(Roles = "Teacher")]
    [RoutePrefix("api/Enrollment")]
    public class EnrollmentController : BaseController
    {
        private readonly IEnrollmentMapper _enrollmentMapper;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IWordSuiteService _wordSuiteService;
        private readonly IWordProgressService _wordProgressService;
        private readonly IGroupService _groupService;
        private readonly IUserForListingMapper _userMapper;

        public EnrollmentController(IEnrollmentService enrollmentService, IEnrollmentMapper enrollmentMapper,
            IWordSuiteService wordSuiteService, IWordProgressService wordProgressService, IUserForListingMapper userMapper,
            ICourseService courseService, IGroupService groupService)
        {
            _enrollmentService = enrollmentService;
            _enrollmentMapper = enrollmentMapper;
            _wordSuiteService = wordSuiteService;
            _wordProgressService = wordProgressService;
            _userMapper = userMapper;
            _courseService = courseService;
            _groupService = groupService;
        }

        [Route("getEnrollmentsByGroupId")]
        public List<EnrollmentWithProgressModel> GetByGroupId(int groupId)
        {
            List<EnrollmentModel> enrollments = _enrollmentMapper.MapToEnrollmentModelCollection(_enrollmentService.GetByGroupId(groupId));
            Group currGroup = _groupService.GetById(groupId, UserId);
            if(currGroup == null)
            {
                return null;
            }
            return enrollments.Select(e => new EnrollmentWithProgressModel
                {
                    Enrollment = e,
                    Progress = _courseService.GetProgress(currGroup.CourseId, e.User.Id)
                }).ToList();
        }

        [Route("getUsersNotBelongingToGroup")]
        public List<UserForListingModel> GetUsersNotBelongingToGroup(int groupId)
        {
            return _userMapper.MapToUserModelCollection(_enrollmentService.GetUsersNotBelongingToGroup(groupId));
        }

        [HttpPost]
        [Route("enrollUsersToGroup")]
        public IHttpActionResult EnrollUsersToGroup(UsersForEnrollmentModel data)
        {
            if(data == null)
            {
                throw new ArgumentNullException("Parameter could not be null", "data");
            }
            var users = _userMapper.MapCollection(data.UserModels);
            if (_enrollmentService.EnrollUsersToGroup(users, data.GroupId)
                && _wordSuiteService.CopyWordsuitesForUsersByGroup(users, data.GroupId)
                && _wordProgressService.CopyProgressesForUsersInGroup(users, data.GroupId))
            {
                return Ok();
            }
            return BadRequest("Some problem occurred!");
        }

        public IHttpActionResult Delete(int enrollmentId)
        {
            if (_wordProgressService.RemoveProgressesForEnrollment(enrollmentId)
                && _wordSuiteService.RemoveWordSuitesForEnrollment(enrollmentId)
                && _enrollmentService.DeleteById(enrollmentId))
            {
                return Ok();
            }
            return BadRequest("Some problem occurred!");
        }
    }
}