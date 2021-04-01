using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MaquetaTienda.Models;
using Microsoft.AspNet.Identity;


namespace MaquetaTienda.Controllers
{
    public class ProductosController : Controller
    {
        private ModeloTiendaContainer db = new ModeloTiendaContainer();

        public ActionResult AddCart (int  id, CarritoCompra cc)
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("Login", "Account");
            }
            /// añadir el producto con ID = id al carrito
            /// que está en session
            Producto prod = db.Productos.Find(id);
            
            // Lo guardamos en el Modelo CarritoCompra
            cc.Add(prod);

            // Guardamos los Productos en los Pedidos, pero sin descontar los articulos
            String currentUser = User.Identity.GetUserId();
            // Buscamos por Cliente y por Id_Producto
            List<Pedido> pedidoList = db.Pedidos.AsEnumerable()
                .Where(pedidoAux => pedidoAux.Cliente.Equals(currentUser)
                                    && pedidoAux.Id_Producto == prod.Id).ToList();

            Pedido pedido = new Pedido();
            if (pedidoList.Count > 0)
            {
                pedido = pedidoList[0];
                pedido.Cantidad = pedido.Cantidad + 1;
            }
            else
            {
                pedido.Id_Producto = prod.Id;
                pedido.Cliente = User.Identity.GetUserId();
                pedido.Cantidad = 1;
                pedido.Fecha = DateTime.Now;
            }

            db.Pedidos.Add(pedido);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Productos
        public ActionResult Index()
        {
            return View(db.Productos.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {   
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Cantidad,Descripcion,Precio,Img")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Cantidad,Descripcion,Precio,Img")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }
        // Model Binders
        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
