namespace Domain.Models
{
    public class Comment : EntityBase
    {
        public DateTime CreationTime { get; set; } = DateTime.UtcNow; //not null | has default
        public string Text { get; set; } = null!; //not null
        public string ChapterExternalId { get; set; } = null!; //not null
        public bool IsPined { get; set; } = false; //not null | has default = false


        //FK and References
        public long UserId { get; set; }  // FK User
        public User User { get; set; } = null!;

        //на который отвечают
        public long? RepliedCommentId { get; set; }  // FK Comment (DELETE CASCADE)
        public Comment? RepliedComment { get; set; }

        //которыми ответили на этот
        public ICollection<Comment> RepliesComments { get; set; } = new List<Comment>();


    }
}
