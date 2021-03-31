using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaquetaTienda.Models
{
    using System;
    using System.Collections.Generic;

    public class PedidoDto
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public string Cliente { get; set; }
    }
}