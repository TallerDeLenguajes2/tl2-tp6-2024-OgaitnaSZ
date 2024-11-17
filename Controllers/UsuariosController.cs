using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_OgaitnaSZ.Models;
using Repositorios;

namespace tp_san.Controllers;

public class Usuarios : Controller{
    private readonly IUsuarioRepository UsuarioRepository;
    public Usuarios(IUsuarioRepository usuarioRepository){
        UsuarioRepository = usuarioRepository;
    }

    /* ----- Login ----- */
    public IActionResult Login(){
        return View();
    }

    /* ---- Validar ---- */

    [HttpPost]
    public IActionResult Validar(string User, string Password){
        if (ModelState.IsValid){
            if(UsuarioRepository.AutenticarUsuario(User, Password)){
                return RedirectToAction("listarPresupuestos", "Presupuestos");
            }
        }
        return View("Login", User);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}