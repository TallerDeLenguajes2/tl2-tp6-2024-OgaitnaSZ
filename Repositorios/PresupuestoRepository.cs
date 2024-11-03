using tl2_tp6_2024_OgaitnaSZ.Models;
using Microsoft.Data.Sqlite;

namespace Repositorios;
public class PresupuestoRepository : IPresupuestoRepository{
    string cadenaConexion = @"Data Source=Tienda.db;Cache=Shared";

    public List<Presupuesto> ListarPresupuestos(){
        List<Presupuesto> listaPresupuestos = new();
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Presupuestos;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    int IdPresupuestoDB = Convert.ToInt32(reader["idPresupuesto"]);
                    string NombreDestinatarioDB = reader["NombreDestinatario"].ToString();
                    string FechaCreacionDB = reader["FechaCreacion"].ToString();
                    DateTime fecha = DateTime.Parse(FechaCreacionDB);
                    listaPresupuestos.Add(new Presupuesto(IdPresupuestoDB, NombreDestinatarioDB, fecha));
                }
            }
            connection.Close();
        }
        return listaPresupuestos;
    }
    
    public void CrearPresupuesto(Presupuesto presupuesto){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            var consulta = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@Nombre, @Fecha)";
            connection.Open();
            var command = new SqliteCommand(consulta, connection);
            command.Parameters.Add(new SqliteParameter("@Nombre", presupuesto.NombreDestinario));
            command.Parameters.Add(new SqliteParameter("@Fecha", presupuesto.FechaCreacion));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Presupuesto ObtenerPresupuestoPorId(int id){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM Presupuestos WHERE idPresupuesto=@id;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@id", id));
            using (SqliteDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    int idDB = Convert.ToInt32(reader["idPresupuesto"]);
                    string nombreDB = reader["NombreDestinario"].ToString();
                    string FechaCreacionDB = reader["FechaCreacion"].ToString();
                    DateTime fecha = DateTime.Parse(FechaCreacionDB);
                    return new Presupuesto(idDB, nombreDB, fecha);
                }
            }
            connection.Close();
        }
        return null;
    }

    public void AgregarProductoAPresupuesto(int idPresupuesto, Producto producto, int cantidad){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)) {
            connection.Open();

            // Verificar si el detalle ya existe
            var consulta = "SELECT Cantidad FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";
            using (var command = new SqliteCommand(consulta, connection)) {
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                command.Parameters.Add(new SqliteParameter("@idProducto", producto.IdProducto));
                var reader = command.ExecuteReader();
                
                if (reader.Read()) {
                    // Si ya existe, actualiza la cantidad
                    int nuevaCantidad = reader.GetInt32(0) + cantidad;
                    var updateQuery = "UPDATE PresupuestosDetalle SET Cantidad = @nuevaCantidad WHERE idPresupuesto = @idPresupuesto AND idProducto = @idProducto";
                    
                    using (var updateCommand = new SqliteCommand(updateQuery, connection)) {
                        updateCommand.Parameters.Add(new SqliteParameter("@nuevaCantidad", nuevaCantidad));
                        updateCommand.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                        updateCommand.Parameters.Add(new SqliteParameter("@idProducto", producto.IdProducto));
                        updateCommand.ExecuteNonQuery();
                    }
                } else {
                    // Sino, agrega un nuevo detalle
                    var insertQuery = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";
                    using (var insertCommand = new SqliteCommand(insertQuery, connection)) {
                        insertCommand.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                        insertCommand.Parameters.Add(new SqliteParameter("@idProducto", producto.IdProducto));
                        insertCommand.Parameters.Add(new SqliteParameter("@cantidad", cantidad));
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
            connection.Close();
        }
    }

    public void EliminarPresupuesto(int id){
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            var consulta = "DELETE FROM Presupuestos WHERE idPresupuesto=@id ";
            connection.Open();
            var command = new SqliteCommand(consulta, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}