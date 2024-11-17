using tl2_tp6_2024_OgaitnaSZ.Models;
using System.Collections.Generic;
namespace Repositorios;
public interface IUsuarioRepository{
    Usuario AutenticarUsuario(string user, string pass);
}