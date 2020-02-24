using System;
using System.Collections.Generic;

namespace WebApi.Models.DBModels
{
    public partial class Usergroup
    {
        public Usergroup()
        {
            Groupmembers = new HashSet<Groupmembers>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public int AdminId { get; set; }

        public virtual Users Admin { get; set; }
        public virtual ICollection<Groupmembers> Groupmembers { get; set; }
    }
}
