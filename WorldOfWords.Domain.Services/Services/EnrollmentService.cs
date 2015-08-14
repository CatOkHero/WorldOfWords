using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;

namespace WorldOfWords.Domain.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        public List<Enrollment> GetByGroupId(int groupId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context.Enrollments.Where(e => e.GroupId == groupId).Include(e => e.User).OrderBy(e => e.User.Name).ToList();
            }
        }

        public List<User> GetUsersNotBelongingToGroup(int groupId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var group = context.Groups.FirstOrDefault(g => g.Id == groupId);
                if(group == null)
                {
                    return null;
                }
                var course = group.Course;
                var usersCurrentlyInGroup = context.Enrollments.Where(e => e.GroupId == groupId).Select(e => e.User);
                //User to add to this group must:
                return context.Users.Where(u =>
                    //have student role
                    u.Roles.Select(r => r.Name).Contains("Student")
                    //not be in this group already
                    && !usersCurrentlyInGroup.Any(u2 => u2.Id == u.Id)
                    //not be subscribed on course, that this group is assigned to, already
                    && u.Enrollments.Select(e => e.Group.Course).FirstOrDefault(c => c.Id == course.Id) == null
                    //and you cannot subscribe yourself to your group
                    && u.Id != group.OwnerId).ToList();
            }
        }

        public bool EnrollUsersToGroup(List<User> users, int groupId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var enrollmentsToAdd = users.Select(user => new Enrollment
                {
                    GroupId = groupId,
                    UserId = user.Id,
                    Date = DateTime.Now
                }).ToList();
                context.Enrollments.AddRange(enrollmentsToAdd);
                context.SaveChanges();
                return true;
            }
        }

        public bool DeleteById(int enrollmentId)
        {
            try
            {
                using (var context = new WorldOfWordsDatabaseContext())
                {
                    context.Enrollments.Remove(context.Enrollments.FirstOrDefault(e => e.Id == enrollmentId));
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}