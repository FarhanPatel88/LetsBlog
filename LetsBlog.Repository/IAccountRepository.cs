
using LetsBlog.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;

namespace LetsBlog.Repository;
public interface IAccountRepository
{
    public Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken);  

    public Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedusername, CancellationToken cancellationToken);  
}
