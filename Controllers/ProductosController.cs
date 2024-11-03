using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_OgaitnaSZ.Models;
using Repositorios;

namespace tp_san.Controllers;

public class ProductosController : Controller{
    private readonly IProductoRepository ProductoRepository;

    public ProductosController(IProductoRepository productoRepository){
        ProductoRepository = productoRepository;
    }

    /* ----- Listar Productos ----- */
    public IActionResult ListarProductos(){
        List<Producto> productos = ProductoRepository.ListarProductos();
        return View(productos);
    }

    /* ----- Crear Producto ----- */
    public IActionResult CrearProducto(){
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Producto producto){
        if (ModelState.IsValid){
            ProductoRepository.CrearProducto(producto);
            return RedirectToAction("ListarProductos");
        }
        return View("CrearProducto", producto);
    }

    /* ----- Modificar Producto ----- */
    public IActionResult EditarProducto(int id){
        Producto productoAModificar = ProductoRepository.ObtenerProductoPorId(id);
        if(productoAModificar == null){
            return NotFound();
        }
        return View(productoAModificar);
    }

    [HttpPost]
    public IActionResult Editar(Producto producto){
        if (ModelState.IsValid){
            ProductoRepository.ModificarProducto(producto.IdProducto ,producto);
            return RedirectToAction("ListarProductos");
        }
        return View("EditarProducto", producto);
    }

    /* ----- Eliminar Producto ----- */
    public IActionResult EliminarProducto(int id){
        Producto productoAEliminar = ProductoRepository.ObtenerProductoPorId(id);
        if(productoAEliminar == null){
            return NotFound();
        }
        return View(productoAEliminar);
    }

    [HttpGet]
    public IActionResult Eliminar(int id){
        if (ModelState.IsValid){
            ProductoRepository.EliminarProducto(id);
            return RedirectToAction("ListarProductos");
        }
        return View("ListarProductos");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
