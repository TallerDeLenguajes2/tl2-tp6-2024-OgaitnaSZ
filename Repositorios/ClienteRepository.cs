using tl2_tp6_2024_OgaitnaSZ.Models;
using Microsoft.Data.Sqlite;

namespace Repositorios;
public class ClienteRepository : IClienteRepository{
    string cadenaConexion = @"Data Source=Tienda.db;Cache=Shared";
    
    List<Cliente> ObtenerClientes(){
        List<Cliente> listadoClientes = new();
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Clientes;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            try{
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader()){
                    while (reader.Read()){
                        int idDB = Convert.ToInt32(reader["idCliente"]);
                        string nombreDB = reader["Nombre"].ToString();
                        string emailDB = reader["Email"].ToString();
                        string telDB = reader["Telefono"].ToString();
                        listadoClientes.Add(new Cliente(idDB, nombreDB, emailDB, telDB));
                    }
                }
                connection.Close();
            }catch(Exception ex){
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            }
        }
        return listadoClientes;
    }
    void CrearCliente(Cliente cliente){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                var query = "INSERT INTO Clientes (Nombre, Email, Telefono) VALUES (@Nombre, @Email, @Telefono);";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine($"Error al insertar cliente: {ex.Message}");
        }
    }
}