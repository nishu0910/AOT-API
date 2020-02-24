using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.DBModels;

namespace WebApi.Services
{
    public interface IUserService
    {
        Users Authenticate(string username, string password);
        //IEnumerable<Users> GetAll();
        //Users GetById(int id);

        List<Group> GetAllGroups();
        int JoinGroup(int groupId, int userId);
        string UpdateGroupName(int groupId, string newGroupName);
        Groupmembers RemoveUserFromGroup(int userId, int groupId);
        Usergroup CreateGroup(int userId, string groupName);
        IEnumerable<Users> GetAllGroupMembers(int userId);

        Users RegisterUser(string emailId, string username, string password);
        Users SocialLogin(User user);
    }

    public class UserService : IUserService
    {
        private assignmentContext _context;
        public UserService(assignmentContext context, IOptions<AppSettings> appSettings)
        {
            this._context = context;
            _appSettings = appSettings.Value;

        }

        private DbSet<Users> _users => _context.Users;

        private readonly AppSettings _appSettings;

        //Usergroup  Groupmembers
        public List<Group> GetAllGroups()
        {
            return _context.Usergroup.Select(x => new Group { GroupId = x.Id, GroupName = x.GroupName }).ToList();

        }

        public int JoinGroup(int groupId, int userId)
        {
            var user = _context.Users.FirstOrDefault(X => X.Id == userId);
            var group = _context.Usergroup.FirstOrDefault(x => x.Id == groupId);
            var groupMember = new Groupmembers()
            {
                GroupId = group.Id,
                UserId = user.Id

            };
            var dbEntry = _context.Groupmembers.Add(groupMember);
            _context.SaveChanges();

            return dbEntry.Entity.Id;
        }

        public string UpdateGroupName(int groupId, string newGroupName)
        {
            var group = _context.Usergroup.FirstOrDefault(x => x.Id == groupId);
            group.GroupName = newGroupName;
            _context.SaveChanges();

            return group.GroupName;
        }

        //need to change 
        public Groupmembers RemoveUserFromGroup(int userId, int groupAdminId)
        {

            var userGroup = _context.Usergroup.FirstOrDefault(a=>a.AdminId==groupAdminId);
            var groupMember = _context.Groupmembers.Include("User").Include("Group").FirstOrDefault(x => x.UserId == userId && x.GroupId==userGroup.Id);
            // var group = _context.Usergroup.FirstOrDefault(x => x.Id == groupId);
            var status = _context.Groupmembers.Remove(groupMember);
            _context.SaveChanges();

            return status.Entity;

        }

        public Usergroup CreateGroup(int userId, string groupName)
        {
            var userGroup = new Usergroup()
            {
                AdminId = userId,
                GroupName = groupName
            };

            var entry = _context.Usergroup.Add(userGroup);

            _context.SaveChanges();

            return entry.Entity;
        }

        public IEnumerable<Users> GetAllGroupMembers(int userId)
        {
            
            var group = _context.Usergroup.FirstOrDefault(grp => grp.AdminId == userId);
            if (group == null) return new List<Users>();

            var result = _context.Groupmembers.Where(x => x.GroupId == group.Id).Select(user => user.User);
            return result;
        }

        public Users RegisterUser(string emailId, string username, string password)
        {
            Users existingUser= Authenticate(username, password);
            if (existingUser != null)
            {
                return new Users { Id = -1};
            }
            string generatedToken = GenerateToken(emailId);
            var user = new Users
            {
                EmailId = emailId,
                Password = password,
                UserName = username,
                Token=generatedToken
            };

            var addedUser = _context.Users.Add(user);
            _context.SaveChanges();
            return addedUser.Entity;
        }


        public Users Authenticate(string username, string password)
        {

            var user = _users.SingleOrDefault(x => x.UserName == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            string generatedToken = GenerateToken(user.EmailId);

            user.Token = generatedToken;
            // remove password before returning
            user.Password = null;

            return user;
        }


        private string GenerateToken(string emailId)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, emailId)
                    //new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var generatedToken = tokenHandler.WriteToken(token);
            return generatedToken;
        }

        public Users SocialLogin(User user)
        {
            var matchedUser = _context.Users.FirstOrDefault(u => u.UserName == user.Username && u.EmailId == user.emailId);
            if (matchedUser != null)
            {
                return matchedUser;
            }

            var newUser = new Users
            {
                EmailId = user.emailId,
                UserName = user.Username,
                Token = user.Token,
                Password=String.Empty
            };
           var addedUser = _context.Users.Add(newUser);
            _context.SaveChanges();
            return addedUser.Entity;

        }


        //public IEnumerable<Users> GetAll()
        //{
        //    // return users without passwords

        //    return _users.Where(c => c.Password == null).ToList();
        //    //return _users.Select(x => {x.Password = null; return x; });
        //}

        //public Users GetById(int id)
        //{
        //    var user = _users.FirstOrDefault(x => x.Id == id);

        //    // return user without password
        //    if (user != null)
        //        user.Password = null;

        //    return user;
        //}
    }
}