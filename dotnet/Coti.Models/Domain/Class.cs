using System;
using System.Collections.Generic;
using System.Text;

namespace Coti.Models.Domain
{
    public class Class
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public int Location_Id { get; set; }

        public int CoverImage_Id { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int CreatedBy { get; set; }

        public int ClassCount { get; set; }

        public List<ClassList> ClassList { get; set; }
    }
}
