using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mabyWorking.Models
{
    [Table("org_test_questions")]
    public class OrgTestQuestion
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("OrgTest")]
        [Column("org_test_id")]
        public long OrgTestId { get; set; }
        public OrgTest OrgTest { get; set; } = null!;
        
        [Column("explanation")]
        public string? Explanation { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("reward_rings")]
        public int RewardRings { get; set; } = 0;
        [Column("reward_xp")]
        public int RewardXp { get; set; } = 0;
        public ICollection<OrgTestAnswer> Answers { get; set; } = new List<OrgTestAnswer>();
    }
}