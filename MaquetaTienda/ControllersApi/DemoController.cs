using MaquetaTienda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MaquetaTienda.ControllersApi
{
    public class DemoController : ApiController
    {
        private ModeloTiendaContainer db = new ModeloTiendaContainer();

        [HttpGet]
        public string Get()
        {
            return "holass";
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Producto> productoList = db.Productos.ToList();
            return Ok(productoList); ;
        }

    }
}
