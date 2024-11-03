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

    public IActionResult ListarProductos()
    {
        List<Producto> productos = ProductoRepository.ListarProductos();
        return View(productos);
    }

    public IActionResult CrearProducto()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
