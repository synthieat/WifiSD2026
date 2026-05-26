using SD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Common.Services
{
    public  interface IUserService
    {
        Task<User> Authenticate(string username, string password, CancellationToken cancellationToken);
    }
}
