using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Cliente{
    public int IdCliente{ get; set; }
    [Required]
    public string Nombre{ get; set; }
    [EmailAddress]
    public string Email{ get; set; }
    [Phone]
    public string Telefono{ get; set; }

    public Cliente(){}
    public Cliente(int idCliente, string nombre, string email, string telefono){
        IdCliente = idCliente;
        Nombre = nombre;
        Email = email;
        Telefono = telefono;
    }
}