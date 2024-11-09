using System.Collections.Generic;
using System.Linq;

namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Presupuesto{
    public int IdPresupuesto{ get; set; }
    public Cliente Cliente{ get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();

    public Presupuesto(){}
    public Presupuesto(int idPresupuesto, Cliente cliente, DateTime fechaCreacion){
        IdPresupuesto = idPresupuesto;
        Cliente = cliente;
        FechaCreacion = fechaCreacion;
    }

        public decimal MontoPresupuesto(){
            return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        public decimal MontoPresupuestoConIva(){
            const decimal iva = (decimal)0.21;
            return (MontoPresupuesto() * (1 + iva));
        }

        public int CantidadProductos(){
            return Detalle.Sum(d => d.Cantidad);
        }
}