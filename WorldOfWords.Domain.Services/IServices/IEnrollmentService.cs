using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.Domain.Services
{
    public interface IEnrollmentService
    {
        List<Enrollment> GetByGroupId(int groupId);
        List<User> GetUsersNotBelongingToGroup(int groupId);
        bool EnrollUsersToGroup(List<User> users, int groupId);
        bool DeleteById(int enrollmentId);
    }
}