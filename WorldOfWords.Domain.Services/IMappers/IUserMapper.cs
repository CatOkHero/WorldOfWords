using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Models;

namespace WorldOfWords.Domain.Services
{
    public interface IUserMapper
    {
        User Map(NewUser source);
    }
}
