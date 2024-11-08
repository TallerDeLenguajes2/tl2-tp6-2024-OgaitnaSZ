namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Cliente{
    public int IdCliente{ get; set; }
    public string Nombre{ get; set; }
    public string Email{ get; set; }
    public string Telefono{ get; set; }

    public Cliente(){}
    public Cliente(int idCliente, string nombre, string email, string telefono){
        IdCliente = idCliente;
        Nombre = nombre;
        Email = email;
        Telefono = telefono;
    }
}