using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mabyWorking.Models
{
    [Table("org_tests")]
    public class OrgTest
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required, StringLength(256)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<OrgTestQuestion> Questions { get; set; } = new List<OrgTestQuestion>();
    }
}
