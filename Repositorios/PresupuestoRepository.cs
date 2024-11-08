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
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                presupuesto.FechaCreacion = DateTime.Now;  //Fecha de creacion del presupuesto
                var consulta = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@Nombre, @Fecha)";
                connection.Open();
                var command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@Nombre", presupuesto.NombreDestinatario));
                command.Parameters.Add(new SqliteParameter("@Fecha", presupuesto.FechaCreacion));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine("Error al crear un presupuesto: " + ex);
        }
    }

    public Presupuesto ObtenerPresupuestoPorId(int id){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                string consulta = "SELECT * FROM Presupuestos WHERE idPresupuesto=@id;";
                SqliteCommand command = new SqliteCommand(consulta, connection);
                connection.Open();
                command.Parameters.AddWithValue("@id", id);

                using (SqliteDataReader reader = command.ExecuteReader()){
                    if (reader.Read()){
                        int idDB = Convert.ToInt32(reader["idPresupuesto"]);
                        string nombreDB = reader["NombreDestinatario"].ToString();

                        DateTime fecha;
                        DateTime.TryParse(reader["FechaCreacion"]?.ToString(), out fecha);

                        return new Presupuesto(idDB, nombreDB, fecha);
                    }
                }
            }
        }catch(Exception ex){
            Console.WriteLine("Error al encontrar producto: " + ex);
        }
        return null;
    }

    public void EliminarPresupuesto(int id){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                var consulta = "DELETE FROM Presupuestos WHERE idPresupuesto=@id ";
                connection.Open();
                var command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@id", id));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine("Error al eliminar presupuesto: " + ex);
        }
        EliminarProductosDePresupuesto(id);
    }

    public void EliminarProductosDePresupuesto(int id){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                var consulta = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto=@id ";
                connection.Open();
                var command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@id", id));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine("Error al eliminar presupuesto: " + ex);
        }
    }

    /* Obtener detalles del presupuesto */
    public List<PresupuestoDetalle> ObtenerDetalles(int idPresupuesto){
        List<PresupuestoDetalle> detalles = new();

        //Obtener producto y cantidad
        using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
            string consulta = "SELECT * FROM PresupuestosDetalle WHERE idPresupuesto=@id;";
            SqliteCommand command = new SqliteCommand(consulta, connection);
            connection.Open();
            command.Parameters.Add(new SqliteParameter("@id", idPresupuesto));
            using (SqliteDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    int idProductoDB = Convert.ToInt32(reader["idProducto"]);
                    int cantidadDB = Convert.ToInt32(reader["Cantidad"]);
                    // Buscar producto por id
                    PresupuestoDetalle detalle = new(obtenerProductoPorId(idProductoDB), cantidadDB);
                    detalles.Add(detalle);
                }
            }
            connection.Close();
        }
        return detalles;
    }

    //Obtener producto por ID
    public Producto obtenerProductoPorId(int id){
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

    public List<Producto> ObtenerProductos(){
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
}