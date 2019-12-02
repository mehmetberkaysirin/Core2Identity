using Core2Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2Identity.Infrastructure
{
    public class CustomUserValidator : IUserValidator<ApplicationUSer>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUSer> manager, ApplicationUSer user)
        {
            if (user.Email.ToLower().EndsWith("@gmail.com")||user.Email.ToLower().EndsWith("@hotmail.com"))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError()
                {
                    Code = "EmailDomainError",
                    Description="Sadece @gmail.com ve @hotmail.com'a izin veriliyor."
                })) ;
            }
        }
    }
}
