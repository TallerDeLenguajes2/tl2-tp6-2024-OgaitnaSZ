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
            PresupuestoRepository.CrearPresupuesto(presupuesto);
            return RedirectToAction("ListarPresupuestos");
        }
        return View("ListarPresupuestos", presupuesto);
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
        return View(detalles);
    }

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int idPresupuesto){
        return View("EditarProducto", presupuesto);
    }
}