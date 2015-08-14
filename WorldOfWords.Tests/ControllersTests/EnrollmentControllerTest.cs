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
    public class EnrollmentControllerTest
    {
        private void GenerateData(string name, string[] roles)
        {
            GenericIdentity identity = new GenericIdentity(name);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, roles);
        }

        [Test]
        public void Get_GetEnrollmentsByGroupId_ReturnsEnrollments_Positive()
        {
            int groupId = 1;
            int userId = 1;
            int courseId = 1;

            var initialEnrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Name = "Roman"
                    },
                    GroupId = groupId,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Name = "Andriy"
                    },
                    GroupId = groupId,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 3,
                    User = new User
                    {
                        Id = 3,
                        Name = "Nazar"
                    },
                    GroupId = groupId,
                    Date = DateTime.Now
                }
            };

            var initialGroup = new Group
            {
                Id = groupId,
                OwnerId = userId,
                Name = "Basic English",
                CourseId = courseId
            };

            var userProgressMapping = new Dictionary<int, double>();
            userProgressMapping.Add(1, 68.53);
            userProgressMapping.Add(2, 76.56);
            userProgressMapping.Add(3, 28.43);

            var expected = new List<EnrollmentWithProgressModel>();

            foreach(var enrollment in initialEnrollments)
            {
                expected.Add(new EnrollmentWithProgressModel()
                    {
                        Enrollment = new EnrollmentModel
                        {
                            Id = enrollment.Id,
                            User = new UserForListingModel
                            {
                                Id = enrollment.User.Id,
                                Name = enrollment.User.Name
                            },
                            GroupId = enrollment.GroupId,
                            Date = string.Format("{0:dd.MM.yyyy}", enrollment.Date)
                        },
                        Progress = userProgressMapping[enrollment.User.Id]
                    });
            }
            initialEnrollments.Reverse();

            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            enrollmentService.Setup(x => x.GetByGroupId(groupId)).Returns(initialEnrollments);
            enrollmentMapper.Setup(x => x.MapToEnrollmentModelCollection(initialEnrollments)).Returns(
                initialEnrollments.Select(e => new EnrollmentModel
                {
                    Id = e.Id,
                    GroupId = e.GroupId,
                    User = new UserForListingModel { Id = e.User.Id, Name = e.User.Name },
                    Date = string.Format("{0:dd.MM.yyyy}", e.Date)
                }).ToList());
            groupService.Setup(x => x.GetById(groupId, userId)).Returns(initialGroup);
            courseService.Setup(x => x.GetProgress(courseId,
                It.Is<int>(id => initialEnrollments.FirstOrDefault(e => e.User.Id == id) != null)))
                .Returns<int, int>((cId, uId) => userProgressMapping[uId]);

            var actual = controller.GetByGroupId(groupId);

            Assert.That(actual, Is.EquivalentTo(expected).Using(new EnrollmentWithProgressModelComparer()));
        }
        [Test]
        public void Get_GetEnrollmentsByGroupId_ReturnsNull_Negative()
        {
            int groupId = 1;
            int userId = 1;
            int courseId = 1;

            var initialEnrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Id = 1,
                    User = new User
                    {
                        Id = 1,
                        Name = "Roman"
                    },
                    GroupId = groupId,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 2,
                    User = new User
                    {
                        Id = 2,
                        Name = "Andriy"
                    },
                    GroupId = groupId,
                    Date = DateTime.Now
                },
                new Enrollment
                {
                    Id = 3,
                    User = new User
                    {
                        Id = 3,
                        Name = "Nazar"
                    },
                    GroupId = groupId,
                    Date = DateTime.Now
                }
            };

            Group initialGroup = null;

            var userProgressMapping = new Dictionary<int, double>();
            userProgressMapping.Add(1, 68.53);
            userProgressMapping.Add(2, 76.56);
            userProgressMapping.Add(3, 28.43);

            var expected = new List<EnrollmentWithProgressModel>();

            foreach (var enrollment in initialEnrollments)
            {
                expected.Add(new EnrollmentWithProgressModel()
                {
                    Enrollment = new EnrollmentModel
                    {
                        Id = enrollment.Id,
                        User = new UserForListingModel
                        {
                            Id = enrollment.User.Id,
                            Name = enrollment.User.Name
                        },
                        GroupId = enrollment.GroupId,
                        Date = string.Format("{0:dd.MM.yyyy}", enrollment.Date)
                    },
                    Progress = userProgressMapping[enrollment.User.Id]
                });
            }
            initialEnrollments.Reverse();

            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            enrollmentService.Setup(x => x.GetByGroupId(groupId)).Returns(initialEnrollments);
            enrollmentMapper.Setup(x => x.MapToEnrollmentModelCollection(initialEnrollments)).Returns(
                initialEnrollments.Select(e => new EnrollmentModel
                {
                    Id = e.Id,
                    GroupId = e.GroupId,
                    User = new UserForListingModel { Id = e.User.Id, Name = e.User.Name },
                    Date = string.Format("{0:dd.MM.yyyy}", e.Date)
                }).ToList());
            groupService.Setup(x => x.GetById(groupId, userId)).Returns(initialGroup);
            courseService.Setup(x => x.GetProgress(courseId,
                It.Is<int>(id => initialEnrollments.FirstOrDefault(e => e.User.Id == id) != null)))
                .Returns<int, int>((cId, uId) => userProgressMapping[uId]);

            var actual = controller.GetByGroupId(groupId);

            Assert.That(actual, Is.Null);
        }
        [Test]
        public void Get_GetUsersNotBelongingToGroups_ReturnsUsers_Positive()
        {
            var initialUsers = new List<User>
            {
                new User
                {
                    Id = 4,
                    Name = "Sasha",
                    Email = "Sasha@example.com"                    
                },
                new User
                {
                    Id = 5,
                    Name = "Slava",
                    Email = "Slava@example.com"
                },
                new User
                {
                    Id = 6,
                    Name = "Yaryna",
                    Email = "Yaryna@example.com"
                },
                new User
                {
                    Id = 7,
                    Name = "Yura",
                    Email = "Yura@example.com"
                }
            };

            var expected = new List<UserForListingModel>
            {
                new UserForListingModel
                {
                    Id = 4,
                    Name = "Sasha"
                },
                new UserForListingModel
                {
                    Id = 5,
                    Name = "Slava"
                },
                new UserForListingModel
                {
                    Id = 6,
                    Name = "Yaryna"
                },
                new UserForListingModel
                {
                    Id = 7,
                    Name = "Yura"
                }
            };

            int groupId = 1;

            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            enrollmentService.Setup(x => x.GetUsersNotBelongingToGroup(groupId)).Returns(initialUsers);
            userMapper.Setup(x => x.MapToUserModelCollection(initialUsers)).Returns(expected);

            var actual = controller.GetUsersNotBelongingToGroup(groupId);

            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void Post_EnrollUsersToGroup_ReturnsOkResult_Positive()
        {
            var initialUsers = new List<UserForListingModel>
            {
                new UserForListingModel
                {
                    Id = 4,
                    Name = "Sasha"       
                },
                new UserForListingModel
                {
                    Id = 5,
                    Name = "Slava"
                },
                new UserForListingModel
                {
                    Id = 6,
                    Name = "Yaryna"
                },
                new UserForListingModel
                {
                    Id = 7,
                    Name = "Yura"
                }
            };

            var mappedUsers = new List<User>
            {
                new User
                {
                    Id = 4,
                    Name = "Sasha"       
                },
                new User
                {
                    Id = 5,
                    Name = "Slava"
                },
                new User
                {
                    Id = 6,
                    Name = "Yaryna"
                },
                new User
                {
                    Id = 7,
                    Name = "Yura"
                }
            };

            int groupId = 1;
            UsersForEnrollmentModel data = new UsersForEnrollmentModel
            {
                GroupId = groupId,
                UserModels = initialUsers
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            userMapper.Setup(x => x.MapCollection(initialUsers)).Returns(mappedUsers);
            enrollmentService.Setup(x => x.EnrollUsersToGroup(mappedUsers, groupId)).Returns(true);
            wordSuiteService.Setup(x => x.CopyWordsuitesForUsersByGroup(mappedUsers, groupId)).Returns(true);
            wordProgressService.Setup(x => x.CopyProgressesForUsersInGroup(mappedUsers, groupId)).Returns(true);

            var actual = controller.EnrollUsersToGroup(data);

            Assert.IsInstanceOf(typeof(OkResult), actual);
        }
        [Test]
        public void Post_EnrollUsersToGroup_ThrowsArgumentNullException_Negative()
        {
            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            Assert.Throws<ArgumentNullException>(() => controller.EnrollUsersToGroup(null));
        }
        [Test]
        public void Post_EnrollUsersToGroup_ReturnsBadRequestResult_Negative()
        {
            var initialUsers = new List<UserForListingModel>
            {
                new UserForListingModel
                {
                    Id = 4,
                    Name = "Sasha"       
                },
                new UserForListingModel
                {
                    Id = 5,
                    Name = "Slava"
                },
                new UserForListingModel
                {
                    Id = 6,
                    Name = "Yaryna"
                },
                new UserForListingModel
                {
                    Id = 7,
                    Name = "Yura"
                }
            };

            var mappedUsers = new List<User>
            {
                new User
                {
                    Id = 4,
                    Name = "Sasha"       
                },
                new User
                {
                    Id = 5,
                    Name = "Slava"
                },
                new User
                {
                    Id = 6,
                    Name = "Yaryna"
                },
                new User
                {
                    Id = 7,
                    Name = "Yura"
                }
            };

            int groupId = 1;
            UsersForEnrollmentModel data = new UsersForEnrollmentModel
            {
                GroupId = groupId,
                UserModels = initialUsers
            };

            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            userMapper.Setup(x => x.MapCollection(initialUsers)).Returns(mappedUsers);
            enrollmentService.Setup(x => x.EnrollUsersToGroup(mappedUsers, groupId)).Returns(true);
            wordSuiteService.Setup(x => x.CopyWordsuitesForUsersByGroup(mappedUsers, groupId)).Returns(true);
            wordProgressService.Setup(x => x.CopyProgressesForUsersInGroup(mappedUsers, groupId)).Returns(false);

            var actual = controller.EnrollUsersToGroup(data);

            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actual);
        }
        [Test]
        public void Delete_DeleteEntrollment_ReturnsOkResult_Positive()
        {
            int enrollmentId = 1;
            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            wordProgressService.Setup(x => x.RemoveProgressesForEnrollment(enrollmentId)).Returns(true);
            wordSuiteService.Setup(x => x.RemoveWordSuitesForEnrollment(enrollmentId)).Returns(true);
            enrollmentService.Setup(x => x.DeleteById(enrollmentId)).Returns(true);

            var actual = controller.Delete(enrollmentId);

            Assert.IsInstanceOf(typeof(OkResult), actual);
        }
        [Test]
        public void Delete_DeleteEntrollment_ReturnsBadRequestResult_Negative()
        {
            int enrollmentId = 1;
            GenerateData("1", new[] { "NoRoles" });
            Mock<IEnrollmentMapper> enrollmentMapper = new Mock<IEnrollmentMapper>();
            Mock<ICourseService> courseService = new Mock<ICourseService>();
            Mock<IEnrollmentService> enrollmentService = new Mock<IEnrollmentService>();
            Mock<IWordSuiteService> wordSuiteService = new Mock<IWordSuiteService>();
            Mock<IWordProgressService> wordProgressService = new Mock<IWordProgressService>();
            Mock<IGroupService> groupService = new Mock<IGroupService>();
            Mock<IUserForListingMapper> userMapper = new Mock<IUserForListingMapper>();

            EnrollmentController controller = new EnrollmentController(enrollmentService.Object, enrollmentMapper.Object,
                wordSuiteService.Object, wordProgressService.Object, userMapper.Object, courseService.Object, groupService.Object);

            wordProgressService.Setup(x => x.RemoveProgressesForEnrollment(enrollmentId)).Returns(true);
            wordSuiteService.Setup(x => x.RemoveWordSuitesForEnrollment(enrollmentId)).Returns(false);
            enrollmentService.Setup(x => x.DeleteById(enrollmentId)).Returns(true);

            var actual = controller.Delete(enrollmentId);

            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actual);
        }
    }

    public class EnrollmentWithProgressModelComparer: IEqualityComparer<EnrollmentWithProgressModel>
    {
        public bool Equals(EnrollmentWithProgressModel x, EnrollmentWithProgressModel y)
        {
            return x.Enrollment.Id == y.Enrollment.Id && x.Enrollment.GroupId == y.Enrollment.GroupId
                && x.Enrollment.Date == y.Enrollment.Date && x.Enrollment.User.Id == y.Enrollment.User.Id
                && x.Enrollment.User.Name == y.Enrollment.User.Name && x.Progress == y.Progress;
        }

        public int GetHashCode(EnrollmentWithProgressModel obj)
        {
            if(obj is EnrollmentWithProgressModel)
            {
                EnrollmentWithProgressModel enrollmentModel = (EnrollmentWithProgressModel)obj;
                return enrollmentModel.Enrollment.Id.GetHashCode();
            }
            throw new ArgumentException("Parameter is not of valid type", "obj");
        }
    }
}
