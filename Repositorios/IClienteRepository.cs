using tl2_tp6_2024_OgaitnaSZ.Models;
using System.Collections.Generic;
namespace Repositorios;
public interface IClienteRepository{
    List<Cliente> ObtenerClientes();
    void CrearCliente(Cliente cliente);
}