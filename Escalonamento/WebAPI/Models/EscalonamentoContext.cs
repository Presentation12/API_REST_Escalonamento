using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Escalonamento.Models
{
    public partial class EscalonamentoContext : DbContext
    {
        public EscalonamentoContext()
        {
        }

        public EscalonamentoContext(DbContextOptions<EscalonamentoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Conexao> Conexao { get; set; }
        public virtual DbSet<Maquina> Maquina { get; set; }
        public virtual DbSet<Utilizador> Utilizador { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:escalonamento.database.windows.net,1433;Initial Catalog=Escalonamento;Persist Security Info=False;User ID=escalonamento;Password=Quimgordo69;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Conexao>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdSim, e.IdJob, e.IdOp })
                    .HasName("pk_conexao");

                entity.ToTable("conexao");

                entity.Property(e => e.Estado).HasColumnName("estado");
            });

            modelBuilder.Entity<Maquina>(entity =>
            {
                entity.HasKey(e => e.IdMaq)
                    .HasName("pk_id_maq");

                entity.ToTable("maquina");

                entity.Property(e => e.IdMaq).HasColumnName("id_maq");

                entity.Property(e => e.Estado)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("estado");
            });

            modelBuilder.Entity<Utilizador>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("pk_id_user");

                entity.ToTable("utilizador");

                entity.HasIndex(e => e.Mail, "uk_mail")
                    .IsUnique();

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.Aut).HasColumnName("aut");

                entity.Property(e => e.Estado)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.PassHash)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("pass_hash");

                entity.Property(e => e.PassSalt)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("pass_salt");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
