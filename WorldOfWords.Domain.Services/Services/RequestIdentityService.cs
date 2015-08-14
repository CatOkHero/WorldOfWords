using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WorldOfWords.Domain.Services.IServices;
using WorldOfWords.Infrastructure.Data.EF;

namespace WorldOfWords.Domain.Services.Services
{
    public class RequestIdentityService: IRequestIdentityService
    {
        public bool CheckIdentity(string hashFromRequest, string hashedToken, string[] roles, out string id)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var users = context.Users;
                foreach (var user in users)
                {
                    if (CheckHash(hashFromRequest, hashedToken, user.Id.ToString(),user.HashedToken))                  
                    {
                        id = user.Id.ToString();
                        return true;
                    }
                }
                id = null;
                return false;
            }
        }

        public bool CheckHash(string hashFromRequest, string hashedToken, string id, string userHashedToken)
        {
            string hashFromDb = Sha256Hash(id);
            return (hashFromDb == hashFromRequest) && (hashedToken == userHashedToken);
        }

        private String Sha256Hash(String value)
        {
            using (SHA256 hash = SHA256.Create())
            {
                return String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}
