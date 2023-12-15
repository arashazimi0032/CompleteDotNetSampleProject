using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Domain.Attributes;

public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
{
    
    public CustomAuthorizeAttribute() : base()
    { }

    public CustomAuthorizeAttribute(Role role) : base()
    {
        Roles = role.ToString();
    }

    public CustomAuthorizeAttribute(string policy) : base(policy)
    {
    }
    public CustomAuthorizeAttribute(Role role, string policy)
        :base(policy)
    {
        Roles = role.ToString();
    }
}