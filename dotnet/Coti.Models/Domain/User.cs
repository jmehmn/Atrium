using System;
using System.Collections.Generic;
using System.Text;

namespace Coti.Models.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int UserStatus_Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int CreatedBy { get; set; }


    }
}
