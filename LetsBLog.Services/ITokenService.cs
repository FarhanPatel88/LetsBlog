using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsBlog.Models.Account;

namespace LetsBLog.Services
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUserIdentity user);
    }
}
