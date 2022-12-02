using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBook.Class_DTO
{
    public class CategoriaLibroDTO
    {
        public int idcategoria { get; set; }
        public string categoria { get; set; }
        public IEnumerable<LibroDTO> libros { get; set; }
    }
}