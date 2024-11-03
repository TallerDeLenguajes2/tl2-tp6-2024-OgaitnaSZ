using System.Collections.Generic;
using System.Linq;

namespace tl2_tp6_2024_OgaitnaSZ.Models;
public class Presupuesto{
    public int idPresupuesto{ get; set; }
    public string nombreDestinario{ get; set; }
    public DateTime fechaCreacion { get; set; }
    public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();

    public Presupuesto(int Id, string NombreDestinario, DateTime FechaCreacion){
        Id = idPresupuesto;
        NombreDestinario = nombreDestinario;
        FechaCreacion = fechaCreacion;
    }

        public int MontoPresupuesto()
        {
            return Detalle.Sum(d => d.producto.precio * d.cantidad);
        }

        public int MontoPresupuestoConIva()
        {
            const double iva = 0.21;
            return (int)(MontoPresupuesto() * (1 + iva));
        }

        public int CantidadProductos()
        {
            return Detalle.Sum(d => d.cantidad);
        }
}