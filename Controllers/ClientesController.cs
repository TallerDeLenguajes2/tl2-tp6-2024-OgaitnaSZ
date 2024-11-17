using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_OgaitnaSZ.Models;
using Repositorios;

namespace tp_san.Controllers;

public class ClientesController : Controller{
    private readonly IClienteRepository ClienteRepository;
    public ClientesController(IClienteRepository clienteRepository){
        ClienteRepository = clienteRepository;
    }

    /* Listar Clientes */
    public IActionResult ListarClientes(){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        List<Cliente> clientes = ClienteRepository.ListarClientes();
        return View(clientes);
    }

    /* ----- Crear Cliente ----- */
    public IActionResult CrearCliente(){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Cliente cliente){
        if (ModelState.IsValid){
            ClienteRepository.CrearCliente(cliente);
            return RedirectToAction("ListarClientes");
        }
        return View("CrearCliente", cliente);
    }

    /* ----- Modificar Cliente ----- */
    public IActionResult ModificarCliente(int id){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        Cliente clienteAModificar = ClienteRepository.ObtenerClientePorId(id);
        if(clienteAModificar == null){
            return NotFound();
        }
        return View(clienteAModificar);
    }

    [HttpPost]
    public IActionResult Modificar(Cliente cliente){
        if (ModelState.IsValid){
            ClienteRepository.ModificarCliente(cliente.IdCliente ,cliente);
            return RedirectToAction("ListarClientes");
        }
        return View("ModificarCliente", cliente);
    }

    /* ----- Eliminar Cliente ----- */
    public IActionResult EliminarCliente(int id){
        var username = Request.Cookies["AuthCookie"];
        if (username == null){
            return RedirectToAction("Login", "Usuarios");
        }
        Cliente clienteAEliminar = ClienteRepository.ObtenerClientePorId(id);
        if(clienteAEliminar == null){
            return NotFound();
        }
        return View(clienteAEliminar);
    }

    [HttpGet]
    public IActionResult Eliminar(int id){
        if (ModelState.IsValid){
            ClienteRepository.EliminarCliente(id);
            return RedirectToAction("ListarClientes");
        }
        return View("ListarClientes");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}