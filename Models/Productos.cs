namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Producto{
    public int IdProducto{ get; set; }
    public string Descripcion{ get; set; }
    public int Precio{ get; set; }

    public Producto(int idProducto, string descripcion, int precio){
        IdProducto = idProducto;
        Descripcion = descripcion;
        Precio = precio;
    }
}