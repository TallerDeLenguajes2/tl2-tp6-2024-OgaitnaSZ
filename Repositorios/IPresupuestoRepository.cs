using tl2_tp6_2024_OgaitnaSZ.Models;
using System.Collections.Generic;
namespace Repositorios;
public interface IPresupuestoRepository{
    void CrearPresupuesto(Presupuesto presupuesto);
    List<Presupuesto> ListarPresupuestos();
    Presupuesto ObtenerPresupuestoPorId(int id);
    void AgregarProductoAPresupuesto(int idPresupuesto, Producto producto, int cantidad);
    void EliminarPresupuesto(int id);
    void EliminarProductosDePresupuesto(int id);
    public List<PresupuestoDetalle> ObtenerDetalles(int idPresupuesto);
    public Producto obtenerProductoPorId(int id);
}