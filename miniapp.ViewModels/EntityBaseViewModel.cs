using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace miniapp.ViewModels
{
    public class EntityBaseViewModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public AppUserViewModel CreatedBy { get; set; }
        public AppUserViewModel ModifiedBy { get; set; }
    }
}
