﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WorldCities.Domain.DTOs.Identity;
using WorldCities.Persistence;
using WorldCities.Persistence.Handlers;
using WorldCities.Persistence.Results;

namespace WorldCities.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtHandler _jwtHandler;

    public AccountController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        JwtHandler jwtHandler)
    {
        _context = context;
        _userManager = userManager;
        _jwtHandler = jwtHandler;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Email);

        if (user == null || !await _userManager
            .CheckPasswordAsync(user, loginRequest.Password))
        {
            return Unauthorized(new LoginResult
            {
                Succes = false,
                Message = "Invalid Email or Password."
            });
        }

        var secToken = await _jwtHandler.GetTokenAsync(user);
        var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);

        return Ok(new LoginResult()
        {
            Succes = true,
            Message = "Login successful",
            Token = jwt
        });
    }
}