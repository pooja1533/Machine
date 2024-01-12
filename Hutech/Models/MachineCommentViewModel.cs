namespace Hutech.Models
{
    public class MachineCommentViewModel
    {
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public long MachineId { get; set; }   
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
