using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfWords.API.Models.Models
{
    public class ForgotPasswordUserModel
    {
        public string Password { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }
    }
}
