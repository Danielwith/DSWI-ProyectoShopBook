using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class ClientData
    {
        public string email { get; set; }
        public string name { get; set; }
        public string apellido { get; set; }
        public string tipDoc { get; set; }
        public string docNum { get; set; }
        public string fecNac { get; set; }
        public string phone { get; set; }
        public string direccion { get; set; }
    }
}