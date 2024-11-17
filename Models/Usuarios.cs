using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Usuario{
    public int IdUsuario{ get; set; }
    public string Nombre{ get; set; }
    public string User{ get; set; }
    public string Password{ get; set; }
    public string Rol{ get; set; }

    public Usuario(){}
    public Usuario(int idUsuario, string nombre, string user, string password, string rol){
        IdUsuario = idUsuario;
        Nombre = nombre;
        User = user;
        Password = password;
        Rol = rol;
    }
}