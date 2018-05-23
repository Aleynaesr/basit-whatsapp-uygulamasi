using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgProgramlamaOdev2.Models.DbModels
{
    public class User
    {
        [Key]
        public int id { get; set; }

        public string email { get; set; }

        public string password { get; set; }

    }
}