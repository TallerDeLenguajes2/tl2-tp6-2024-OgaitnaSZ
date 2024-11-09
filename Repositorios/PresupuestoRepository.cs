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
                    int idCliente = Convert.ToInt32(reader["idCliente"]);
                    string FechaCreacionDB = reader["FechaCreacion"].ToString();
                    DateTime fecha = DateTime.Parse(FechaCreacionDB);

                    Cliente cliente = ObtenerClientePorId(idCliente);
                    listaPresupuestos.Add(new Presupuesto(IdPresupuestoDB, cliente, fecha));
                }
            }
            connection.Close();
        }
        return listaPresupuestos;
    }

    public int CrearPresupuesto(Presupuesto presupuesto){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                connection.Open();
                presupuesto.FechaCreacion = DateTime.Now;  //Fecha de creacion del presupuesto
                var consulta = "INSERT INTO Presupuestos (idCliente, FechaCreacion) VALUES (@idCliente, @Fecha)";
                var command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idCliente", presupuesto.Cliente.IdCliente));
                command.Parameters.Add(new SqliteParameter("@Fecha", presupuesto.FechaCreacion));
                command.ExecuteNonQuery();

                //Obtener el último ID insertado
                var consultaId = "SELECT last_insert_rowid();";
                var commandId = new SqliteCommand(consultaId, connection);
                int idPresupuesto = Convert.ToInt32(commandId.ExecuteScalar());
                connection.Close();
                return idPresupuesto;
            }
        }catch(Exception ex){
            Console.WriteLine("Error al crear un presupuesto: " + ex);
            return 0;
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
                        int idCliente = Convert.ToInt32(reader["idCliente"]);

                        DateTime fecha;
                        DateTime.TryParse(reader["FechaCreacion"]?.ToString(), out fecha);

                        Cliente cliente = ObtenerClientePorId(idCliente);
                        return new Presupuesto(idDB, cliente, fecha);
                    }
                }
            }
        }catch(Exception ex){
            Console.WriteLine("Error al encontrar producto: " + ex);
        }
        return null;
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
    public List<Cliente> ObtenerClientes(){
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
    public void EliminarProductoDelPresupuesto(int idProducto, int idPresupuesto){
        try{
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion)){
                connection.Open();

                //Eliminar producto de los presupuestos
                var consulta = "DELETE FROM PresupuestosDetalle WHERE idProducto = @idProducto AND idPresupuesto = @idPresupuesto";
                var command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }catch(Exception ex){
            Console.WriteLine("Error al eliminar el producto del presupuesto: " + ex.Message);
        }
    }
}