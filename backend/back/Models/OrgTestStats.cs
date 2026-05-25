using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mabyWorking.Models
{
    [Table("org_test_stats")]
    public class OrgTestStats
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("Stats")]
        [Column("stats_id")]
        public long StatsId { get; set; }
        public Stats Stats { get; set; } = null!;

        [ForeignKey("OrgTest")]
        [Column("org_test_id")]
        public long OrgTestId { get; set; }
        public OrgTest OrgTest { get; set; } = null!;

        [Column("is_passed")]
        public bool IsPassed { get; set; } = false;
    }
}
