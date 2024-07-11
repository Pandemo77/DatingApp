﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController(DataContext context) : BaseAPIController
{
  [Authorize]
  [HttpGet("auth")]
  public ActionResult<string> GetAuth()
  {
    return "secrets here";
  }

  [HttpGet("not-found")]

  public ActionResult<AppUser> GetNotFound()
  {
    var thing = context.Users.Find(-1);

    if (thing == null) return NotFound();

    return thing;
  }

  [HttpGet("server-error")]
  public ActionResult<AppUser> GetServerError()
  {
    var thing = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened!");

    return thing;
  }

  [HttpGet("bad-request")]
  public ActionResult<AppUser> GetBadRequest()
  {
    return BadRequest("This was nog a good request");
  }

}