using System;
using System.Collections.Generic;
using System.Text;

namespace Coti.Models.Domain
{
    public class UserProfile
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
