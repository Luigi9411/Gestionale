using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Gestionale.Models;

public partial class AgentiContext : DbContext
{
    public AgentiContext()
    {
    }

    public AgentiContext(DbContextOptions<AgentiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agenti> Agentis { get; set; }

    public virtual DbSet<Articoli> Articolis { get; set; }

    public virtual DbSet<Clienti> Clientis { get; set; }

    public virtual DbSet<Moviordini> Moviordinis { get; set; }

    public virtual DbSet<Ordini> Ordinis { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agenti>(entity =>
        {
            entity.ToTable("agenti");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Agente)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("agente");
            entity.Property(e => e.Lock).HasColumnName("lock");
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .HasColumnName("password");
            entity.Property(e => e.Ui)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("UI");
        });

        modelBuilder.Entity<Articoli>(entity =>
        {
            entity.ToTable("articoli");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codice)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("codice");
            entity.Property(e => e.Descrizione)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descrizione");
            entity.Property(e => e.Prezzo)
                .HasColumnType("smallmoney")
                .HasColumnName("prezzo");
            entity.Property(e => e.Unitamisura)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Clienti>(entity =>
        {
            entity.ToTable("clienti");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cap)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("cap");
            entity.Property(e => e.Citta)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("citta");
            entity.Property(e => e.IdAgente).HasColumnName("id_agente");
            entity.Property(e => e.Indirizzo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("indirizzo");
            entity.Property(e => e.Prov)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("prov");
            entity.Property(e => e.Ragionesociale)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ragionesociale");

            entity.HasOne(d => d.IdAgenteNavigation).WithMany(p => p.Clientis)
                .HasForeignKey(d => d.IdAgente)
                .HasConstraintName("FK_clienti_agenti");
        });

        modelBuilder.Entity<Moviordini>(entity =>
        {
            entity.ToTable("moviordini");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdArticolo).HasColumnName("id_articolo");
            entity.Property(e => e.IdOrdine).HasColumnName("id_ordine");
            entity.Property(e => e.Prezzo)
                .HasColumnType("smallmoney")
                .HasColumnName("prezzo");
            entity.Property(e => e.Quantita).HasColumnName("quantita");

            entity.HasOne(d => d.IdArticoloNavigation).WithMany(p => p.Moviordinis)
                .HasForeignKey(d => d.IdArticolo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_moviordini_articoli");

            entity.HasOne(d => d.IdOrdineNavigation).WithMany(p => p.Moviordinis)
                .HasForeignKey(d => d.IdOrdine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_moviordini_Ordini");
        });

        modelBuilder.Entity<Ordini>(entity =>
        {
            entity.ToTable("Ordini");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataOrdine)
                .HasColumnType("date")
                .HasColumnName("data_ordine");
            entity.Property(e => e.Evaso).HasColumnName("evaso");
            entity.Property(e => e.IdAgente).HasColumnName("id_agente");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.NumOrdine).HasColumnName("num_ordine");
            entity.Property(e => e.TotaleOrdine)
                .HasColumnType("smallmoney")
                .HasColumnName("totale_ordine");

            entity.HasOne(d => d.IdAgenteNavigation).WithMany(p => p.Ordinis)
                .HasForeignKey(d => d.IdAgente)
                .HasConstraintName("FK_Ordini_agenti");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Ordinis)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_Ordini_clienti");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
