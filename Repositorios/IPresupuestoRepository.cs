using tl2_tp6_2024_OgaitnaSZ.Models;
using System.Collections.Generic;
namespace Repositorios;
public interface IPresupuestoRepository{
    List<Presupuesto> ListarPresupuestos();
    int CrearPresupuesto(Presupuesto presupuesto);
    Presupuesto ObtenerPresupuestoPorId(int id);
    void EliminarPresupuesto(int id);
    void EliminarProductosDePresupuesto(int id);
    public List<PresupuestoDetalle> ObtenerDetalles(int idPresupuesto);
    public Producto obtenerProductoPorId(int id);
    void AgregarProductoAPresupuesto(int idPresupuesto, Producto producto, int cantidad);
    public List<Producto> ObtenerProductos();
    void EliminarProductoDelPresupuesto(int idProducto, int idPresupuesto);
}