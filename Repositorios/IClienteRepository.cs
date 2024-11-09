using tl2_tp6_2024_OgaitnaSZ.Models;
using System.Collections.Generic;
namespace Repositorios;
public interface IClienteRepository{
    List<Cliente> ListarClientes();
    void CrearCliente(Cliente cliente);
    void ModificarCliente(int id, Cliente cliente);
    Cliente ObtenerClientePorId(int id);
    void EliminarCliente(int id);
    void EliminarPresupuestosDelCliente(int id);
}