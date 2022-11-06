using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class LibroDTO
    {
        public string idLibro { get; set; }
        public string titulo { get; set; }
        public string autor { get; set; }
        public string sinopsis { get; set; }
        public string precio { get; set; }
        public string editorial { get; set; }
        public string fecha { get; set; }
        public string img { get; set; }
        public int idSubCategoria { get; set; }
        public int idCategoria { get; set; }
    }
}