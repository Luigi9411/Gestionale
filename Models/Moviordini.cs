using System;
using System.Collections.Generic;

namespace Gestionale.Models;

public partial class Moviordini
{
    public short Id { get; set; }

    public short IdOrdine { get; set; }

    public short IdArticolo { get; set; }

    public short Quantita { get; set; }

    public decimal Prezzo { get; set; }

    public virtual Articoli IdArticoloNavigation { get; set; } = null!;

    public virtual Ordini IdOrdineNavigation { get; set; } = null!;
}
