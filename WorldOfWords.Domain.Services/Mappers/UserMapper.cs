using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.Domain.Services
{
    public class UserMapper: IUserMapper
    {
        public User Map(NewUser source)
        {
            User user = new User();
            user.Name = source.Login;
            user.Password = source.Password;
            user.EMail = source.Email;

            return user;
        }
    }
}
