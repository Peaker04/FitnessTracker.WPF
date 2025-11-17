using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.Models
{
    [Table("ProgressRecords")]
    public class ProgressRecord
    {
        [Key]
        public int ProgressRecordId { get; set; }

        public int UserId { get; set; }

        public DateTime RecordDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? BodyFatPercentage { get; set; }

        // Circumferences (cm)
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Chest { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Waist { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Hips { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? LeftArm { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? RightArm { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? LeftThigh { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? RightThigh { get; set; }

        // Photos
        [MaxLength(500)]
        public string FrontPhotoUrl { get; set; }

        [MaxLength(500)]
        public string SidePhotoUrl { get; set; }

        [MaxLength(500)]
        public string BackPhotoUrl { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}