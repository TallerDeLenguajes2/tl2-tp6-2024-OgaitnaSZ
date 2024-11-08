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
        List<Cliente> clientes = ClienteRepository.ListarClientes();
        return View(clientes);
    }

    /* ----- Crear Cliente ----- */
    public IActionResult CrearCliente(){
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Cliente cliente){
        if (ModelState.IsValid){
            ClienteRepository.CrearCliente(cliente);
            return RedirectToAction("ListarProductos");
        }
        return View("CrearCliente", cliente);
    }
}