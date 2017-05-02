using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdLineup.Models
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Index")]
        public int Index { get; set; }

        [Display(Name = "Flight Start")]
        [DataType(DataType.DateTime)]
        public DateTime FlightStart { get; set; }

        [Display(Name = "Flight End")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string FlightEnd { get; set; }

        [Display(Name = "Image Filename")]
        [DataType(DataType.Text)]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string ImageFilename { get; set; }

        [JsonIgnore]
        public virtual List<Billboard> Billboards { get; set; }

    } // public class Ad
}
