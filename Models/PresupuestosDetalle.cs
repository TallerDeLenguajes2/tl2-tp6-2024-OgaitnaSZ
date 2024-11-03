namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class PresupuestoDetalle{
    public Producto producto{ get; set; }
    public int cantidad{ get; set; }

    public PresupuestoDetalle(Producto Producto, int Cantidad){
        Producto = producto;
        Cantidad = cantidad;
    }
}