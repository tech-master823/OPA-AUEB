using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuebUnofficial.Core.Interfaces
{
    public interface IEclassService
    {
        Task<string> LoginAsync(string uname, string password);
        Task<string> LogoutAsync(string token);
    }
}
