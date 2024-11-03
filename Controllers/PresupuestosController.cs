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
        presupuesto.FechaCreacion = DateTime.Now;  //Fecha de creacion del presupuesto
        if (ModelState.IsValid){
            PresupuestoRepository.CrearPresupuesto(presupuesto);
            return RedirectToAction("ListarPresupuestos");
        }
        return View("ListarPresupuestos", presupuesto);
    }

    /* ----- Eliminar Producto ----- */
    public IActionResult EliminarPresupuesto(int id){
        Presupuesto presupuestoAModificar = PresupuestoRepository.ObtenerPresupuestoPorId(id);
        if(presupuestoAModificar == null){
            return NotFound();
        }
        return View(presupuestoAModificar);
    }

    [HttpGet]
    public IActionResult Eliminar(int id){
        if (ModelState.IsValid){
            PresupuestoRepository.EliminarPresupuesto(id);
            return RedirectToAction("ListarPresupuestos");
        }
        return View("ListarPresupuestos");
    }

    /* ----- Modificar Presupuesto ----- */
    public IActionResult EditarPresupuesto(int id){
        Presupuesto presupuestoAModificar = PresupuestoRepository.ObtenerPresupuestoPorId(id);
        if(presupuestoAModificar == null){
            return NotFound();
        }
        return View(presupuestoAModificar);
    }

    [HttpPost]
    public IActionResult Editar(Presupuesto presupuesto){
        if (ModelState.IsValid){
            
            return RedirectToAction("ListarProductos");
        }
        return View("EditarProducto", presupuesto);
    }
}