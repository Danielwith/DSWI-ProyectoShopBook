using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class EmailSend
    {
        public string email { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public string html { get; set; }
    }
}