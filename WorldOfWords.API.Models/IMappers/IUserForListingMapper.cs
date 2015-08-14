using System.Collections.Generic;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.API.Models
{
    public interface IUserForListingMapper
    {
        User Map(UserForListingModel userModel);
        List<User> MapCollection(List<UserForListingModel> userModels);
        UserForListingModel MapToUserModel(User user);
        List<UserForListingModel> MapToUserModelCollection(List<User> users);
    }
}
