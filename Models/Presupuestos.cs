using System.Collections.Generic;
using System.Linq;

namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Presupuesto{
    public int IdPresupuesto{ get; set; }
    public string NombreDestinatario{ get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();

    public Presupuesto(){}
    public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion){
        IdPresupuesto = idPresupuesto;
        NombreDestinatario = nombreDestinatario;
        FechaCreacion = fechaCreacion;
    }

        public int MontoPresupuesto()
        {
            return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        public int MontoPresupuestoConIva()
        {
            const double iva = 0.21;
            return (int)(MontoPresupuesto() * (1 + iva));
        }

        public int CantidadProductos()
        {
            return Detalle.Sum(d => d.Cantidad);
        }
}