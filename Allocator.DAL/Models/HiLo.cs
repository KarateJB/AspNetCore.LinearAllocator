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
        public string Key { get; set; }

        [Required]
        [Column(TypeName="bigint")]
        public Int64 NextHi { get; set; }

        [Required]
        [Column(TypeName="bigint")]
        public Int64 MaxValue { get; set; }
    }
}