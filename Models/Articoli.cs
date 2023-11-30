using System;
using System.Collections.Generic;

namespace Gestionale.Models;

public partial class Articoli
{
    public short Id { get; set; }

    public string Codice { get; set; } = null!;

    public string Descrizione { get; set; } = null!;

    public string? Unitamisura { get; set; }

    public decimal? Prezzo { get; set; }

    public virtual ICollection<Moviordini> Moviordinis { get; set; } = new List<Moviordini>();
}
