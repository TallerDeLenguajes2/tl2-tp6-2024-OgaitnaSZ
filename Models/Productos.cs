using System.ComponentModel.DataAnnotations;
namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Producto{
    [Required] [StringLength(100)]
    public int IdProducto{ get; set; }
    [Required] [StringLength(250)] 
    public string Descripcion{ get; set; }
    [Range(0, 999.99)] 
    public decimal Precio{ get; set; }

    public Producto(){}
    public Producto(int idProducto, string descripcion, int precio){
        IdProducto = idProducto;
        Descripcion = descripcion;
        Precio = precio;
    }
}