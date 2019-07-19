using System;

namespace miniapp.EntityFrameworkCore.Entities
{
    public class ToDo : BaseEntity
    {
        public bool Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
