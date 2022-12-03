using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class DtBoletaLibroDTO
    {
        public int? idLibro { get; set; }
        public string tituLibro { get; set; }
        public int? cantidad { get; set; }
        public decimal? precUni { get; set; }
        public decimal? importe { get; set; }
    }
}