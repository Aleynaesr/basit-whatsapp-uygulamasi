using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgProgramlamaOdev2.Models.DbModels
{
    public class Online
    {
        [Key]
        public int id { get; set; }
        public string ConenctionId { get; set; }

        public virtual User User {get;set;}
    }
}