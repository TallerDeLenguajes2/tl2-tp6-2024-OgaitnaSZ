namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Producto{
    public int idProducto{ get; set; }
    public string descripcion{ get; set; }
    public int precio{ get; set; }

    public Producto(int Id, string Descripcion, int Precio){
        Id = idProducto;
        Descripcion = descripcion;
        Precio = precio;
    }
}