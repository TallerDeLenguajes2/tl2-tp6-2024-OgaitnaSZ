using tl2_tp6_2024_OgaitnaSZ.Models;
using Microsoft.Data.Sqlite;

namespace Repositorios;
public class ClienteRepository : IClienteRepository{
    string cadenaConexion = @"Data Source=Tienda.db;Cache=Shared";
    
    public List<Cliente> ListarClientes(){
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
    public void CrearCliente(Cliente cliente){
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
    public void ModificarCliente(int id, Cliente cliente){
        Console.WriteLine($"Modificando cliente con ID: {id}");
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                var query = "UPDATE Clientes SET Nombre=@Nombre, Email=@Email, Telefono=@Telefono WHERE idCliente=@id;";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@Nombre", cliente.Nombre));
                command.Parameters.Add(new SqliteParameter("@Email", cliente.Email));
                command.Parameters.Add(new SqliteParameter("@Telefono", cliente.Telefono));;
                command.Parameters.Add(new SqliteParameter("@id", id));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine($"Error al modificar cliente: {ex.Message}");
        }
    }
    public Cliente ObtenerClientePorId(int id){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Clientes WHERE idCliente=@id;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@id", id));
            using (SqliteDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    int idDB = Convert.ToInt32(reader["idCliente"]);
                    string nombreDB = reader["Nombre"].ToString();
                    string emailDB = reader["Email"].ToString();
                    string telefonoDB = reader["Telefono"].ToString();
                    return new Cliente(idDB, nombreDB, emailDB, telefonoDB);
                }
            }
            connection.Close();
        }
        return null;
    }
    public void EliminarCliente(int id){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                var consulta = "DELETE FROM Clientes WHERE idCliente=@id ";
                connection.Open();
                var command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@id", id));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine("Error al eliminar cliente: " + ex);
        }
        EliminarPresupuestosDelCliente(id);
    }
    public void EliminarPresupuestosDelCliente(int id){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                var consulta = "DELETE FROM Presupuestos WHERE idCliente=@id ";
                connection.Open();
                var command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@id", id));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine("Error al eliminar presupuestos del cliente: " + ex);
        }
    }

}