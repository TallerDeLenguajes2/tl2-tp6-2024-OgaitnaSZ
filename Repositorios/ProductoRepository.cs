using tl2_tp6_2024_OgaitnaSZ.Models;
using Microsoft.Data.Sqlite;

namespace Repositorios;
public class ProductoRepository : IProductoRepository{
    string cadenaConexion = @"Data Source=Tienda.db;Cache=Shared";

    public void CrearProducto(Producto producto){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";
            connection.Open();

            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ModificarProducto(int id, Producto nuevoProducto){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            var query = "UPDATE Productos SET Descripcion= @Descripcion , Precio= @Precio WHERE idProducto=@id ";
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@Descripcion", nuevoProducto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", nuevoProducto.Precio));
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Producto> ListarProductos(){
        List<Producto> listadoProductos = new();
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Productos;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            try{
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader()){
                    while (reader.Read()){
                        int idDB = Convert.ToInt32(reader["idProducto"]);
                        string descripcionDB = reader["Descripcion"].ToString();
                        int precioDB = Convert.ToInt32(reader["Precio"]);
                        listadoProductos.Add(new Producto(idDB, descripcionDB, precioDB));
                    }
                }
                connection.Close();
            }catch(Exception ex){
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            }
        }
        return listadoProductos;
    }

    public Producto ObtenerProductoPorId(int id){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Productos WHERE idProducto=@id;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@id", id));
            using (SqliteDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    int idDB = Convert.ToInt32(reader["idProducto"]);
                    string descripcionDB = reader["Descripcion"].ToString();
                    int precioDB = Convert.ToInt32(reader["Precio"]);
                    return new Producto(idDB, descripcionDB, precioDB);
                }
            }
            connection.Close();
        }
        return null;
    }

    public void EliminarProducto(int id){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            var consulta = "DELETE FROM Productos WHERE idProducto=@id ";
            connection.Open();
            var command = new SqliteCommand(consulta, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}