using System;

namespace miniapp.EntityFrameworkCore.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public AppUser CreatedBy { get; set; }
        public AppUser ModifiedBy { get; set; }
    }
}
