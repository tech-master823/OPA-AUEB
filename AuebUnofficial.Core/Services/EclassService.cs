using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuebUnofficial.Core.Services
{
    public class EclassService : Interfaces.IEclassService
    {
        public async Task<string> LoginAsync(string uname, string password)
        {
            return await "https://eclass.aueb.gr/modules/mobile/mlogin.php"
                .PostUrlEncodedAsync(new { uname = uname, pass = password })
                .ReceiveString();
        }

        public async Task<string> LogoutAsync(string token)
        {
            return await "https://eclass.aueb.gr/modules/mobile/mlogin.php?logout"
                .PostUrlEncodedAsync(new { token = token })
                .ReceiveString();
        }
    }
}
