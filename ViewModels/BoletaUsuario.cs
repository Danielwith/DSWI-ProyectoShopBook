using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopBook.ViewModels
{
    public class BoletaUsuario
    {
        [Display(Name = "N° Boleta")]
        public string nroBoleta { get; set; }

        [Display(Name = "Fecha Generada")]
        [DataType(DataType.Date)]
        public DateTime? fechGene { get; set; }

        [Display(Name = "Nombre del Usuario")]
        public string nombre { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }
    }
}