﻿using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService,
  IMapper mapper) : BaseAPIController
{
  [HttpPost("register")] // account/register

  public async Task<ActionResult<UserDTO>> Register(RegisterDTOs registerDTOs)
  {
    if (await UserExits(registerDTOs.Username)) return BadRequest("Username is already taken.");
    using var hmac = new HMACSHA512();

    var user = mapper.Map<AppUser>(registerDTOs);
    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTOs.Password));
    user.PasswordSalt = hmac.Key;

    context.Users.Add(user);
    await context.SaveChangesAsync();

    return new UserDTO
    {
      Username = user.UserName,
      Token = tokenService.CreateToken(user),
      KnownAs = user.KnownAs
    };
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDTO>> Login(LoginDto loginDto)
  {
    var user = await context.Users
      .Include(p => p.Photos)
        .FirstOrDefaultAsync(x =>
          x.UserName == loginDto.Username.ToLower());

    if (user == null) return Unauthorized("Invalid username ");

    using var hmac = new HMACSHA512(user.PasswordSalt);

    var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

    for (int i = 0; i < ComputeHash.Length; i++)
    {
      if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
    }
    return new UserDTO
    {
      Username = user.UserName,
      KnownAs = user.KnownAs,
      Token = tokenService.CreateToken(user),
      PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
    };
  }

  private async Task<bool> UserExits(string username)
  {
    return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
  }
}
