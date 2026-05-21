using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Identity.Dtos
{
    public sealed record AppUserDto(string UserId, string Email, IList<string> Roles, IList<Claim> Claims);    
    
}
