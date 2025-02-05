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
        List<Presupuesto> presupuestos = PresupuestoRepository.ListarPresupuestos();
        return View(presupuestos);
    }

    /* ----- Crear Presupuesto ----- */
    public IActionResult CrearPresupuesto(){
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Presupuesto presupuesto){
        if (ModelState.IsValid){
            int idPresupuesto = PresupuestoRepository.CrearPresupuesto(presupuesto);
            return RedirectToAction("AgregarProductosAPresupuesto", "Presupuestos", new { idPresupuesto = idPresupuesto });
        }
        return View("CrearPresupuesto", presupuesto);
    }

    /* ----- Eliminar Presupuesto ----- */
    public IActionResult EliminarPresupuesto(int id){
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
        Presupuesto presupuestoAMostrar = PresupuestoRepository.ObtenerPresupuestoPorId(id);
        if(presupuestoAMostrar == null){
            return NotFound();
        }
        List<PresupuestoDetalle> detalles = PresupuestoRepository.ObtenerDetalles(id);
        MiViewModel viewModel = new MiViewModel{detalles = detalles , idPresupuesto = id};
        return View(viewModel);
    }

    public IActionResult AgregarProductosAPresupuesto(int idPresupuesto){
        List<Producto> productos = PresupuestoRepository.ObtenerProductos();
        var MiViewModel2 = new MiViewModel2{productos = productos, idPresupuesto = idPresupuesto};
        return View("AgregarProductosAPresupuesto", MiViewModel2);
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