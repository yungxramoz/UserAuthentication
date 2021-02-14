using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuthentication.Data.Entities;
using UserAuthentication.Helpers;
using UserAuthentication.Models;
using UserAuthentication.Services;

namespace UserAuthentication.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Get all users</returns>
        /// <response code="201">Returns all the users</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Get()
        {
            var users = _userService.Get();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        /// <summary>
        /// Get a specific user
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The requested user</returns>
        /// <response code="201">Returns the specific user</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Get(int id)
        {
            var user = _userService.Get(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        /// <summary>
        /// Authenticate a user with the credentials
        /// </summary>
        /// <param name="model">The credentials of a user</param>
        /// <returns>User with token on successful authenticate</returns>
        /// <response code="201">Successfully authenticated and returns token</response>
        /// <response code="400">If credentials were not valid</response>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            User user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest("Username or password is incorrect");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                UserId = user.UserId,
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Token = tokenString
            });
        }

        /// <summary>
        /// Create a new account for a new user
        /// </summary>
        /// <param name="model">All required data for a new user</param>
        /// <returns>Status if user has been successfully created</returns>
        /// <response code="201">Successfully registered the new user</response>
        /// <response code="400">If registration info were not valid</response>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RegistrationModel model)
        {
            var user = _mapper.Map<User>(model);

            try
            {
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (InfoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a specific User
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="model">Updated data of the user</param>
        /// <returns>Status if user has been successfully updated</returns>
        /// <response code="201">Successfully updated the user</response>
        /// <response code="400">If user does not exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] UpdateModel model)
        {
            var user = _mapper.Map<User>(model);
            user.UserId = id;

            try
            {
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (InfoException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a sepcific user
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        /// <returns>Status if user has been successfully deleted</returns>
        /// <response code="201">Successfully deleted the user</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}
