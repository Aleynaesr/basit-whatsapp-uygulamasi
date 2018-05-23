using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgProgramlamaOdev2.Models.DbModels
{
    public class Group
    {
        [Key]
        public int id { get; set; }
        public string GroupName { get; set; }

        public virtual List<GroupMessage> GroupMessages { get; set; }



    }
}