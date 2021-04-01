using MaquetaTienda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MaquetaTienda.Controllers
{
    public class CarritoController : Controller
    {
        private ModeloTiendaContainer db = new ModeloTiendaContainer();

        // GET: Carrito
        public ActionResult Index(CarritoCompra cc)
        {
            List<PedidoDto> pedidos = new List<PedidoDto>();

            foreach (Producto prod in cc)
            {
                PedidoDto pedido = isProductoInPedidos(pedidos, prod);

                if (pedido == null)
                {
                    pedido = new PedidoDto();
                    pedido.Producto = prod;
                    pedido.Cantidad = 1;

                    pedidos.Add(pedido);
                } else
                {
                    pedido.Cantidad = pedido.Cantidad + 1;
                }
            }

            return View(pedidos);
        }

        private PedidoDto isProductoInPedidos(List<PedidoDto> pedidos, Producto prod)
        {
            PedidoDto pedidoFound = null;

            foreach(PedidoDto pedido in pedidos)
            {
                if (pedido.Producto.Id == prod.Id)
                {
                    pedidoFound = pedido;
                }
            }

            return pedidoFound;
        }

        public ActionResult SavePedido(CarritoCompra cc)
        {
            if (cc.Count > 0)
            {
                // Guardamos los Productos en los Pedidos, pero sin descontar los articulos
                String currentUser = User.Identity.GetUserId();
                // Buscamos por Cliente y por Id_Producto
                List<Pedido> pedidoList = db.Pedidos.AsEnumerable()
                    .Where(pedidoAux => pedidoAux.Cliente.Equals(currentUser)).ToList();

                // Hacer un mapeo entre PedidoDto y Pedido
                foreach (Pedido pedido in pedidoList)
                {
                    // Buscamos el producto y borramos su cantidad
                    Producto prod = db.Productos.Find(pedido.Id_Producto);
                    prod.Cantidad -= pedido.Cantidad;
                    db.SaveChanges();
                }

                cc.Clear();
            }
           
            // Persistir la lista de Pedidos en base de datos
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
           // Implementar borrado

            return View("Index");
        }
    }
}