using System;
using System.Collections.Generic;

namespace Gestionale.Models;

public partial class Ordini
{
    public short Id { get; set; }

    public DateTime DataOrdine { get; set; }

    public short? NumOrdine { get; set; }

    public short? IdAgente { get; set; }

    public short? IdCliente { get; set; }

    public decimal? TotaleOrdine { get; set; }

    public bool? Evaso { get; set; }

    public virtual Agenti? IdAgenteNavigation { get; set; }

    public virtual Clienti? IdClienteNavigation { get; set; }

    public virtual ICollection<Moviordini> Moviordinis { get; set; } = new List<Moviordini>();
}
