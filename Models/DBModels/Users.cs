using System;
using System.Collections.Generic;

namespace WebApi.Models.DBModels
{
    public partial class Users
    {
        public Users()
        {
            Groupmembers = new HashSet<Groupmembers>();
            Usergroup = new HashSet<Usergroup>();
        }

        public int Id { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Groupmembers> Groupmembers { get; set; }
        public virtual ICollection<Usergroup> Usergroup { get; set; }
    }
}
