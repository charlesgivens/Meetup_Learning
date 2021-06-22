using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeetupAPI.Entities;
using AutoMapper;
using MeetupApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MeetupAPI.Identity;

namespace MeetupApi.Controllers
{
  [Route("api/account")]
  public class AccountController : ControllerBase
  {
    private readonly MeetupContext _meetupContext;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountController(MeetupContext meetupContext, IPasswordHasher<User> passwordHasher, IJwtProvider jwtProvider)
    {
      _meetupContext = meetupContext;
      _passwordHasher = passwordHasher;
      _jwtProvider = jwtProvider;
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] UserLoginDto userLoginDto)
    {
      var user = _meetupContext.Users
        .Include(user => user.Role)
        .FirstOrDefault(user => user.Email == userLoginDto.Email);

      if(user == null)
      {
        return BadRequest("Invalid username or password");
      }

      var passwordVerified  = _passwordHasher.VerifyHashedPassword(user, user.Password, userLoginDto.Password);
      if(passwordVerified == PasswordVerificationResult.Failed)
      {
        return BadRequest("Invalid username or password");
      }

      var token = _jwtProvider.GenerateJwtToken(user);

      return Ok(token);

    }

    [HttpPost("register")]
    public ActionResult Register([FromBody] RegisterUserDto registerUserDto)
    {
      if (!ModelState.IsValid)
      {
          return BadRequest(ModelState);
      }

      var newUser = new User()
      {
        FirstName = registerUserDto.FirstName,
        LastName = registerUserDto.LastName,
        Email = registerUserDto.Email,
        RoleId = registerUserDto.RoleId
      };

      var password = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
      newUser.Password = password;

      _meetupContext.Users.Add(newUser);
      _meetupContext.SaveChanges();

      return Ok();
    }


  }
}