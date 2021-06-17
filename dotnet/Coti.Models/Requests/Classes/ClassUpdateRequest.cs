using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Coti.Models.Requests.Classes
{
    public class ClassUpdateRequest : IModelIdentifier
    {
     
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int Location_Id { get; set; }

        [Required]
        public int CoverImage_Id { get; set; }

        [Required]
        public float IsActive { get; set; }

        public DateTime DateModified { get; set; }

        public int CreatedBy { get; set; }
    }
}
