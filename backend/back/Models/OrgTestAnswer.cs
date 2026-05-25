using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mabyWorking.Models
{
    [Table("org_test_answers")]
    public class OrgTestAnswer
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("OrgTestQuestion")]
        [Column("question_id")]
        public long QuestionId { get; set; }
        public OrgTestQuestion Question { get; set; } = null!;

        [Required]
        [Column("text")]
        public string Text { get; set; } = string.Empty;

        [Column("is_correct")]
        public bool IsCorrect { get; set; } = false;
    }
}
