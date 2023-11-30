using System;
using System.Collections.Generic;

namespace Gestionale.Models;

public partial class Clienti
{
    public short Id { get; set; }

    public string Ragionesociale { get; set; } = null!;

    public string? Indirizzo { get; set; }

    public string? Cap { get; set; }

    public string? Citta { get; set; }

    public string? Prov { get; set; }

    public short? IdAgente { get; set; }

    public virtual Agenti? IdAgenteNavigation { get; set; }

    public virtual ICollection<Ordini> Ordinis { get; set; } = new List<Ordini>();
}
