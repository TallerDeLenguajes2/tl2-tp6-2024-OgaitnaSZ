using tl2_tp6_2024_OgaitnaSZ.Models;
using System.Collections.Generic;
namespace Repositorios;
public interface IUsuarioRepository{
    bool AutenticarUsuario(string user, string pass);
    Usuario ObtenerUsuario(string user, string pass);
}