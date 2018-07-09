using SV.Infrastructure;
using Dapper.Contrib.Extensions;

namespace SV.DataAccess.Models
{
    [Table("Test")]
    public class Test : BaseEntity
    {
        [Key]
        public int TestId { get; set; }
        public string TestName { get; set; }

    }
}
