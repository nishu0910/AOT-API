using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;

using System.Collections.Generic;
using System.Linq;
using WebApi.Models.DBModels;
using System;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{
    //  [Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        //private assignmentContext _context;

        //public UsersController(assignmentContext context)
        //{
        //    this._context = context;
        //}

        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        
        [HttpGet("getAllGroups")]
        public IActionResult GetAllGroups()
        {
            List<Group> groups = _userService.GetAllGroups();
            return Ok(groups);
        }


        
        [HttpPost("joinGroup")]
        public IActionResult JoinGroup(int groupId, int userId)
        {
            Console.WriteLine("test");
            int joinedMemberId = _userService.JoinGroup(groupId, userId);
            return Ok(joinedMemberId);
        }

       
        [HttpPut("updateGroupName")]
        public IActionResult UpdateGroupName(int groupId, string newGroupName)
        {
            string groupName = _userService.UpdateGroupName(groupId, newGroupName);
            return Ok(groupName);
        }

        [EnableCors("CustomPolicyCORS")]
        [HttpDelete("removeUserFromGroup")]
        public IActionResult RemoveUserFromGroup(int userId, int groupId)
        {
            var deletedGroupMember = _userService.RemoveUserFromGroup(userId, groupId);
            return Ok(deletedGroupMember);
        }
        [HttpPost("createGroup")]
        public IActionResult CreateGroup(int userId, string groupName)
        {
            Usergroup usergroup = _userService.CreateGroup(userId, groupName);
            return Ok(usergroup);
        }

        [EnableCors("CustomPolicyCORS")]
        [HttpGet("getAllGroupMembers")]
        public IActionResult GetAllGroupMembers(int userId)
        {
            IEnumerable<Users> usergroup = _userService.GetAllGroupMembers(userId);
            return Ok(usergroup);
        }

        [EnableCors("CustomPolicyCORS")]
        [HttpPost("socialLogin")]
        public IActionResult SocialLogin([FromBody]User user)
        {
            var userNew = _userService.SocialLogin(user);
            
            return Ok(userNew);
        }


        //[AllowAnonymous]
        [HttpPost("registerUser")]
        public IActionResult RegisterUser(string emailId, string username, string password)
        {
            var user = _userService.RegisterUser(emailId, username, password);
            if(user.Id == -1)
            {
                return BadRequest("User Already exist!!!");
            }
            return Ok(user);
        }

        //[AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        
    }
}
