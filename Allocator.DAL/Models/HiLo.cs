using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allocator.DAL.Models
{
    [Table("HiLos")]
    public class HiLo : UowEntity
    {
        [Key]
        [StringLength(200)]
        public required string Key { get; set; }

        [Required]
        [Column(TypeName="bigint")]
        public long NextHi { get; set; }

        [Required]
        [Column(TypeName="bigint")]
        public long MaxValue { get; set; }
    }
}