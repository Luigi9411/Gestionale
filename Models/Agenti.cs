using System;
using System.Collections.Generic;

namespace Gestionale.Models;

public partial class Agenti
{
    public short Id { get; set; }

    public string Agente { get; set; } = null!;

    public string? Ui { get; set; }

    public string? Password { get; set; }

    public bool? Lock { get; set; }

    public virtual ICollection<Clienti> Clientis { get; set; } = new List<Clienti>();

    public virtual ICollection<Ordini> Ordinis { get; set; } = new List<Ordini>();
}
