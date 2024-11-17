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
            Usuario usuario = UsuarioRepository.AutenticarUsuario(User, Password);
            if(usuario != null){
                // Crear variables de sesion
                HttpContext.Session.SetString("User", usuario.User);
                HttpContext.Session.SetString("Rol", usuario.Rol);

                // Crear Cookie
                Response.Cookies.Append("AuthCookie", usuario.Nombre, new CookieOptions{
                    HttpOnly = true,
                    Expires = DateTimeOffset.Now.AddHours(1), 
                });

                return RedirectToAction("listarPresupuestos", "Presupuestos");
            }
        }
        return View("Login", User);
    }

    /* ---- Cerrar Sesion ---- */
    public IActionResult Logout(){
        Response.Cookies.Delete("AuthCookie");
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}