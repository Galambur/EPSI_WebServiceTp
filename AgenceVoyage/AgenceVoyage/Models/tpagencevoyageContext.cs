using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AgenceVoyage.Models
{
    public partial class tpagencevoyageContext : DbContext
    {
        public tpagencevoyageContext()
        {
        }

        public tpagencevoyageContext(DbContextOptions<tpagencevoyageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Gare> Gare { get; set; }
        public virtual DbSet<Garedessertville> Garedessertville { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<Train> Train { get; set; }
        public virtual DbSet<Trainarret> Trainarret { get; set; }
        public virtual DbSet<Trainreservation> Trainreservation { get; set; }
        public virtual DbSet<Ville> Ville { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;database=tpagencevoyage;uid=root", x => x.ServerVersion("5.7.26-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("PRIMARY");

                entity.ToTable("client");

                entity.Property(e => e.IdClient).HasColumnType("int(11)");

                entity.Property(e => e.Adresse)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nom)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Prenom)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Telephone)
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Gare>(entity =>
            {
                entity.HasKey(e => e.IdGare)
                    .HasName("PRIMARY");

                entity.ToTable("gare");

                entity.Property(e => e.IdGare).HasColumnType("int(11)");

                entity.Property(e => e.NomGare)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Garedessertville>(entity =>
            {
                entity.HasKey(e => new { e.IdVille, e.IdGare })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("garedessertville");

                entity.HasIndex(e => e.IdGare)
                    .HasName("IdGare");

                entity.Property(e => e.IdVille).HasColumnType("int(11)");

                entity.Property(e => e.IdGare).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.IdReservation)
                    .HasName("PRIMARY");

                entity.ToTable("reservation");

                entity.HasIndex(e => e.IdClient)
                    .HasName("IdClient");

                entity.Property(e => e.IdReservation)
                    .HasColumnName("idReservation")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdClient).HasColumnType("int(11)");

                entity.Property(e => e.NbrPassager).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Train>(entity =>
            {
                entity.HasKey(e => e.IdTrain)
                    .HasName("PRIMARY");

                entity.ToTable("train");

                entity.Property(e => e.IdTrain).HasColumnType("int(11)");

                entity.Property(e => e.DateArrive).HasColumnType("datetime");

                entity.Property(e => e.DateDepart).HasColumnType("datetime");
            });

            modelBuilder.Entity<Trainarret>(entity =>
            {
                entity.HasKey(e => new { e.IdTrain, e.IdGare })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("trainarret");

                entity.HasIndex(e => e.IdGare)
                    .HasName("IdGare");

                entity.Property(e => e.IdTrain).HasColumnType("int(11)");

                entity.Property(e => e.IdGare).HasColumnType("int(11)");

                entity.Property(e => e.DateArret).HasColumnType("datetime");

                entity.Property(e => e.DateDepart).HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Trainreservation>(entity =>
            {
                entity.HasKey(e => new { e.IdReservation, e.IdTrain })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("trainreservation");

                entity.HasIndex(e => e.IdTrain)
                    .HasName("IdTrain");

                entity.Property(e => e.IdReservation)
                    .HasColumnName("idReservation")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdTrain).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Ville>(entity =>
            {
                entity.HasKey(e => e.IdVille)
                    .HasName("PRIMARY");

                entity.ToTable("ville");

                entity.Property(e => e.IdVille).HasColumnType("int(11)");

                entity.Property(e => e.CodePostal)
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NomVille)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
