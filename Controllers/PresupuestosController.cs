using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_OgaitnaSZ.Models;
using Repositorios;

namespace tp_san.Controllers;

public class PresupuestosController : Controller{
    private readonly IPresupuestoRepository PresupuestoRepository;

    public PresupuestosController(IPresupuestoRepository presupuestoRepository){
        PresupuestoRepository = presupuestoRepository;
    }

    /* ----- Listar Presupuestos ----- */
    public IActionResult ListarPresupuestos(){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        List<Presupuesto> presupuestos = PresupuestoRepository.ListarPresupuestos();
        var datosPresupuestos = new ViweModelPresupuestos{Presupuestos = presupuestos, Rol = HttpContext.Session.GetString("Rol")};
        return View(datosPresupuestos);
    }

    /* ----- Crear Presupuesto ----- */
    public IActionResult CrearPresupuesto(){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        List<Cliente> clientes = PresupuestoRepository.ObtenerClientes();
        return View(clientes);
    }

    [HttpPost]
    public IActionResult Crear(int IdCliente){
        Presupuesto presupuesto = new Presupuesto();
        if (ModelState.IsValid){
            Cliente cliente = PresupuestoRepository.ObtenerClientePorId(IdCliente);
            presupuesto.Cliente = cliente;
            int idPresupuesto = PresupuestoRepository.CrearPresupuesto(presupuesto);
            return RedirectToAction("AgregarProductosAPresupuesto", "Presupuestos", new { idPresupuesto = idPresupuesto });
        }
        return View("CrearPresupuesto", presupuesto);
    }

    /* ----- Eliminar Presupuesto ----- */
    public IActionResult EliminarPresupuesto(int id){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        Presupuesto presupuestoAEliminar = PresupuestoRepository.ObtenerPresupuestoPorId(id);
        if(presupuestoAEliminar == null){
            return NotFound();
        }
        return View(presupuestoAEliminar);
    }
    [HttpGet]
    public IActionResult Eliminar(int id){
        if (ModelState.IsValid){
            PresupuestoRepository.EliminarPresupuesto(id);
            return RedirectToAction("ListarPresupuestos");
        }
        return View("ListarPresupuestos");
    }

    /* ----- Presupuesto Detalle ----- */
    public IActionResult PresupuestoDetalle(int id){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        Presupuesto presupuestoAMostrar = PresupuestoRepository.ObtenerPresupuestoPorId(id);
        if(presupuestoAMostrar == null){
            return NotFound();
        }
        List<PresupuestoDetalle> detalles = PresupuestoRepository.ObtenerDetalles(id);
        ViweModelDetallesPresupuesto viewModel = new ViweModelDetallesPresupuesto{Detalles = detalles , IdPresupuesto = id, Rol = HttpContext.Session.GetString("Rol")};
        return View(viewModel);
    }

    public IActionResult AgregarProductosAPresupuesto(int idPresupuesto){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        List<Producto> productos = PresupuestoRepository.ObtenerProductos();
        ViewModelNuevoPresupuesto datosForm = new ViewModelNuevoPresupuesto{productos = productos ,idPresupuesto = idPresupuesto};
        return View("AgregarProductosAPresupuesto", datosForm);
    }

    [HttpPost]
    public IActionResult Agregar(List<int> idProductos, List<int> cantidades, int idPresupuesto){
        if (idProductos != null && cantidades != null && idProductos.Count == cantidades.Count){
            for (int i = 0; i < idProductos.Count; i++){
                int idProducto = idProductos[i];
                int cantidad = cantidades[i];
                Producto producto = PresupuestoRepository.obtenerProductoPorId(idProducto);
                PresupuestoRepository.AgregarProductoAPresupuesto(idPresupuesto, producto, cantidad);
            }
        }
        return RedirectToAction("PresupuestoDetalle", new { id = idPresupuesto });
    }

    [HttpGet]
    public IActionResult EliminarProductoDelPresupuesto(int idProducto, int idPresupuesto){
        PresupuestoRepository.EliminarProductoDelPresupuesto(idProducto, idPresupuesto);
        return RedirectToAction("PresupuestoDetalle", new { id = idPresupuesto });
    }
}