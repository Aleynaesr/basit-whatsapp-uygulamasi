using System;
using System.ComponentModel.DataAnnotations;

namespace AgProgramlamaOdev2.Models.DbModels
{
    public class GroupMessage
    {
        [Key]
        public int id { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }


        public virtual User SendUser { get; set; }
        public virtual Group Group { get; set; }

    }
}