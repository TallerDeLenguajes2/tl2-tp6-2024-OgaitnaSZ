namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class PresupuestoDetalle{
    public Producto Producto{ get; set; }
    public int Cantidad{ get; set; }

    public PresupuestoDetalle(Producto producto, int cantidad){
        Producto = producto;
        Cantidad = cantidad;
    }
}
public class ViweModelDetallesPresupuesto{
    public List<PresupuestoDetalle> Detalles{get;set;}
    public int IdPresupuesto {get;set;}
    public string Rol {get;set;}
}

public class ViewModelNuevoPresupuesto{
    public List<Producto> productos{get;set;}
    public int idPresupuesto {get;set;}
}
