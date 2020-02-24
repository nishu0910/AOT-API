using System;
using System.Collections.Generic;

namespace WebApi.Models.DBModels
{
    public partial class Groupmembers
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? GroupId { get; set; }

        public virtual Usergroup Group { get; set; }
        public virtual Users User { get; set; }
    }
}
