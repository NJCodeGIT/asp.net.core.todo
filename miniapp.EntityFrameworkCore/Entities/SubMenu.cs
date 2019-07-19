namespace miniapp.EntityFrameworkCore.Entities
{
    public class SubMenu: BaseEntity
    {
        public bool IsEnabled { get; set; }
        public Menu ParentMenu { get; set; }
    }
}