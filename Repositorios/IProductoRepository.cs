using tl2_tp6_2024_OgaitnaSZ.Models;

namespace Repositorios;
public interface IProductoRepository{
    void CrearProducto(Producto producto);
    void ModificarProducto(int id, Producto producto);
    List<Producto> ListarProductos();
    Producto ObtenerProductoPorId(int id);
    void EliminarProducto(int id);
}