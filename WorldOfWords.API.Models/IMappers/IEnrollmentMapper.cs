using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public interface IEnrollmentMapper
    {
        EnrollmentModel MapToEnrollmentModel(Enrollment enrollment);
        List<EnrollmentModel> MapToEnrollmentModelCollection(List<Enrollment> enrollments);
    }
}
