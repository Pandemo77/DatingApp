using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions;

public static class ClaimsPrincipleExtensions
{
  public static string GetUserName(this ClaimsPrincipal user)
  {
    var username = user.FindFirstValue(ClaimTypes.NameIdentifier) 
    ?? throw new Exception("Cannot get a username from token");
    
    return username;
  }
}
