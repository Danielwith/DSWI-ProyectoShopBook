using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBook.Entity
{
    public class ShoppingCart
    {
        public tb_libros Libros { get; set; }
        public int Cantidad { get; set; }
        public tb_usuario Usuario { get; set; }
        public ShoppingCart()
        {

        }

        public ShoppingCart(tb_libros libros,int cant, tb_usuario usuario)
        {
            this.Libros = libros;
            this.Cantidad = cant;
            this.Usuario = usuario;
        }
    }
}