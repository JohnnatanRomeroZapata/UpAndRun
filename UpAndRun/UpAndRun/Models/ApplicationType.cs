using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UpAndRun.Models
{
    public class ApplicationType
    {
        public int Id { get; set; }

        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }
    }
}
