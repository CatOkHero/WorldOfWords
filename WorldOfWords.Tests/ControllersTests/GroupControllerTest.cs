using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Principal;
using NUnit.Framework;
using Moq;
using WorldofWords.Controllers;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Models;
using WorldOfWords.Domain.Services;
using System.Web.Http.Results;

namespace WorldOfWords.Tests.ControllersTests
{
    [TestFixture]
    public class GroupControllerTest
    {
        private void GenerateData(string name, string[] roles)
        {
            GenericIdentity identity = new GenericIdentity(name);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, roles);
        }

        [Test]
        public void Get_Groups_ReturnsGroupsList_Positive()
        {
            var initial = new List<Group>
            {
                new Group
                {
                    Id=1,
                    CourseId=1,
                    OwnerId=1,
                    Name="Basic English",
                    Course = new Course
                    {
                        Name = "English.A1"
                    }
                },
                new Group
                {
                    Id=2,
                    CourseId=2,
                    OwnerId=1,
                    Name="Basic German",
                    Course = new Course
                    {
                        Name = "German.A1"
                    }
                },
                new Group
                {
                    Id=3,
                    CourseId=3,
                    OwnerId=1,
                    Name="Basic French",
                    Course = new Course
                    {
                        Name = "French.A1"
                    }
                }
            };
            var expected = new List<GroupForListingModel>
            {
                new GroupForListingModel
                {
                    Id=1,
                    CourseId=1,
                    Name="Basic English",
                    CourseName = "English.A1"
                },
                new GroupForListingModel
                {
                    Id=2,
                    CourseId=2,
                    Name="Basic German",
                    CourseName="German.A1"
                },
                new GroupForListingModel
                {
                    Id=3,
                    CourseId=3,
                    Name="Basic French",
                    CourseName="French.A1"
                }
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();
            int userId = 1;
            
            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);
            
            groupService.Setup(x => x.GetAll(userId)).Returns(initial);
            groupForListingMapper.Setup(x => x.MapToGroupModelCollection(initial)).Returns(expected);

            var actual = groupController.Get();

            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void Get_GetGroupById_ReturnsNull_Negative()
        {
            Group expected = null;
            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            int userId = 1;
            int groupId = 111111;

            groupService.Setup(x => x.GetById(groupId, userId)).Returns(expected);

            var actual = groupController.Get(groupId);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Get_GetGroupById_ReturnsGroupModel_Positive()
        {
            var initial = new Group
            {
                Id = 1,
                OwnerId = 1,
                CourseId = 1,
                Name = "Simple English",
                Course = new Course
                {
                    Name = "English.A1"
                }
            };

            var expected = new GroupForListingModel
            {
                Id = 1,
                CourseId = 1,
                Name = "Simple English",
                CourseName = "English.A1"
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            int groupId = 1;
            int userId = 1;

            groupService.Setup(x => x.GetById(groupId, userId)).Returns(initial);
            groupForListingMapper.Setup(x => x.MapToGroupModel(initial)).Returns(expected);

            var actual = groupController.Get(groupId);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Get_GetCourses_ReturnsCourses_Positive()
        {
            var initial = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "English.A1",
                    LanguageId = 1,
                    OwnerId = 1,
                    IsPrivate = false
                },
                new Course
                {
                    Id = 2,
                    Name = "French.A1",
                    LanguageId = 2,
                    OwnerId = 1,
                    IsPrivate = false
                }
            };

            var expected = new List<CourseForGroupModel>
            {
                new CourseForGroupModel
                {
                    Id = 1,
                    Name = "English.A1"
                },
                new CourseForGroupModel
                {
                    Id = 2,
                    Name = "French.A1"
                }
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            int userId = 1;

            courseService.Setup(x => x.GetAllCourses(userId)).Returns(initial);
            courseMapper.Setup(x => x.MapToCourseModelCollection(initial)).Returns(expected);

            var actual = groupController.GetCourses();
            
            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void Post_NewGroup_ReturnsOkResult_Positive()
        {
            var initial = new GroupModel
            {
                Name = "Some Group Name",
                OwnerId = 1,
                CourseId = 1
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            groupService.Setup(x => x.CheckIfGroupNameExists(initial)).Returns(false);
            groupService.Setup(x => x.Add(initial)).Returns(true);

            var actual = groupController.Post(initial);

            Assert.IsInstanceOf(typeof(OkResult), actual);
        }
        [Test]
        public void Post_NewGroup_ThrownException_Negative()
        {
            GroupModel initial = null;

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            Assert.Throws<ArgumentNullException>(() => groupController.Post(initial));
        }
        [Test]
        public void Post_NewGroup_ReturnsBadRequestResult_Negative()
        {
            var initial = new GroupModel
            {
                Name = "Some Group Name",
                OwnerId = 1,
                CourseId = 1
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            groupService.Setup(x => x.CheckIfGroupNameExists(initial)).Returns(true);
            groupService.Setup(x => x.Add(initial)).Returns(true);

            var actual = groupController.Post(initial);

            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actual);
        }
        [Test]
        public void Delete_Group_ReturnsOkResult_Positive()
        {
            int groupId = 1;
            List<Enrollment> initialEnrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Id = 1,
                    GroupId = groupId,
                    UserId = 1,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 2,
                    GroupId = groupId,
                    UserId = 2,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 3,
                    GroupId = groupId,
                    UserId = 3,
                    Date = DateTime.Now
                }
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            enrollmentService.Setup(x => x.GetByGroupId(groupId)).Returns(initialEnrollments);
            wordProgressService.Setup(x => x.RemoveProgressesForEnrollment(
                It.Is<int>(id => initialEnrollments.FirstOrDefault(e => e.Id == id) != null))).Returns(true);
            wordsuiteService.Setup(x => x.RemoveWordSuitesForEnrollment(
                It.Is<int>(id => initialEnrollments.FirstOrDefault(e => e.Id == id) != null))).Returns(true);
            groupService.Setup(x => x.DeleteById(groupId)).Returns(true);

            var actual = groupController.Delete(groupId);

            Assert.IsInstanceOf(typeof(OkResult), actual);
        }
        [Test]
        public void Delete_Group_ReturnsBadRequestResult_Negative()
        {
            int groupId = 1;
            List<Enrollment> initialEnrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Id = 1,
                    GroupId = groupId,
                    UserId = 1,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 2,
                    GroupId = groupId,
                    UserId = 2,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 3,
                    GroupId = groupId,
                    UserId = 3,
                    Date = DateTime.Now
                }
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IGroupForListingMapper> groupForListingMapper = new Mock<IGroupForListingMapper>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IWordSuiteService> wordsuiteService = new Mock<IWordSuiteService>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<ICourseForGroupMapper> courseMapper = new Mock<ICourseForGroupMapper>();
            Mock<IGroupMapper> groupMapper = new Mock<IGroupMapper>();

            GroupController groupController = new GroupController(groupService.Object, groupForListingMapper.Object,
                enrollmentService.Object, wordProgressService.Object, wordsuiteService.Object, courseService.Object,
                courseMapper.Object, groupMapper.Object);

            enrollmentService.Setup(x => x.GetByGroupId(groupId)).Returns(initialEnrollments);
            wordProgressService.Setup(x => x.RemoveProgressesForEnrollment(
                It.Is<int>(id => initialEnrollments.FirstOrDefault(e => e.Id == id) != null))).Returns(true);
            wordsuiteService.Setup(x => x.RemoveWordSuitesForEnrollment(
                It.Is<int>(id => initialEnrollments.FirstOrDefault(e => e.Id == id) != null))).Returns(true);
            groupService.Setup(x => x.DeleteById(groupId)).Returns(false);

            var actual = groupController.Delete(groupId);

            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actual);
        }
    }
}
