namespace Hutech.Models
{
    public class UserMenuPermissionViewModel
    {
      
        public string RoleId { get; set; }
        public string? MenuIds { get; set; }
        public long? Id { get; set; }
        public long? MenuId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
    }
}
