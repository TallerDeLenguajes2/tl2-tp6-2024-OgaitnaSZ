using tl2_tp6_2024_OgaitnaSZ.Models;
using Microsoft.Data.Sqlite;

namespace Repositorios;
public class UsuarioRepository : IUsuarioRepository{
    string cadenaConexion = @"Data Source=Tienda.db;Cache=Shared";
    public bool AutenticarUsuario(string user, string pass){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Usuarios WHERE User=@user AND Password=@pass;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@user", user));
            command.Parameters.Add(new SqliteParameter("@pass", pass));
            using (SqliteDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    string userDB = reader["User"].ToString();
                    string passDB = reader["Password"].ToString();

                    if(userDB == user && passDB == pass){
                        return true;
                    }
                }
            }
            connection.Close();
        }
        return false;
    }
    public Usuario ObtenerUsuario(string user, string pass){
        Usuario usuario = new Usuario();
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Usuarios WHERE User=@user AND Password=@pass;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@user", user));
            command.Parameters.Add(new SqliteParameter("@pass", pass));
            using (SqliteDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    int idDB = Convert.ToInt32(reader["idUsuario"]);
                    string nombreDB = reader["Nombre"].ToString();
                    string userDB = reader["User"].ToString();
                    string passDB = reader["Password"].ToString();
                    string rolDB = reader["Rol"].ToString();
                    return new Usuario(idDB, nombreDB, userDB, passDB, rolDB);
                }
            }
            connection.Close();
        }
        return usuario;
    }

}