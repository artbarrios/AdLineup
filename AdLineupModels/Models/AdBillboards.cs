using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdLineup.Models
{
    public class AdBillboards
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Billboard")]
        public int BillboardId { get; set; }
        [ForeignKey("BillboardId")]
        [JsonIgnore]
        public virtual Billboard Billboard { get; set; }

        [Required]
        [Display(Name = "Ad")]
        public int AdId { get; set; }
        [ForeignKey("AdId")]
        [JsonIgnore]
        public virtual Ad Ad { get; set; }

    } // public class AdBillboards
}
