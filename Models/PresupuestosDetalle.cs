namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class PresupuestoDetalle{
    public Producto Producto{ get; set; }
    public int Cantidad{ get; set; }

    public PresupuestoDetalle(Producto producto, int cantidad){
        Producto = producto;
        Cantidad = cantidad;
    }
}
public class MiViewModel{
    public List<PresupuestoDetalle> detalles{get;set;}
    public int idPresupuesto {get;set;}
}

public class MiViewModel2{
    public List<Producto> productos{get;set;}
    public int idPresupuesto {get;set;}
}
