using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Domain.Attributes;

public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
{
    
    public CustomAuthorizeAttribute() : base()
    { }

    public CustomAuthorizeAttribute(Role role)
    {
        Roles = role.ToString();
    }
    public CustomAuthorizeAttribute(Role role, string? policy)
        :base(policy: policy ?? string.Empty)
    {
        Roles = role.ToString();
    }
}