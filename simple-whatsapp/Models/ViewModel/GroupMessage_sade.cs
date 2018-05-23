using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgProgramlamaOdev2.Models.ViewModel
{
    public class GroupMessage_sade
    {
        public int id { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public int UserId { get; set; }
        public string UserMail { get; set; }

    }
}