using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_OgaitnaSZ.Models;
using Repositorios;

namespace tp_san.Controllers;

public class ClientesController : Controller{

    /* ----- Crear Cliente ----- */
    public IActionResult CrearCliente(){
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Cliente cliente){
        if (ModelState.IsValid){
            //ProductoRepository.CrearProducto(producto);
            //return RedirectToAction("ListarProductos");
        }
        return View("CrearCliente", cliente);
    }
}