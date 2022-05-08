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

        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobOp> JobOp { get; set; }
        public virtual DbSet<Maquina> Maquina { get; set; }
        public virtual DbSet<Operacao> Operacao { get; set; }
        public virtual DbSet<SimJob> SimJob { get; set; }
        public virtual DbSet<Simulacao> Simulacao { get; set; }
        public virtual DbSet<Utilizador> Utilizador { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //"Esconder" OnConfiguring
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:escalonamento.database.windows.net,1433;Initial Catalog=Escalonamento;Persist Security Info=False;User ID=escalonamento;Password=Quimgordo69;Encrypt=True;TrustServerCertificate=False;Connection Timeout=15;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.IdJob)
                    .HasName("pk_id_job");

                entity.ToTable("job");

                entity.Property(e => e.IdJob).HasColumnName("id_job");
            });

            modelBuilder.Entity<JobOp>(entity =>
            {
                entity.HasKey(e => new { e.IdJob, e.IdOp })
                    .HasName("prop_job_op");

                entity.ToTable("job_op");

                entity.Property(e => e.IdJob).HasColumnName("id_job");

                entity.Property(e => e.IdOp).HasColumnName("id_op");

                entity.Property(e => e.Duracao).HasColumnName("duracao");

                entity.HasOne(d => d.IdJobNavigation)
                    .WithMany(p => p.JobOps)
                    .HasForeignKey(d => d.IdJob)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_id_job");

                entity.HasOne(d => d.IdOpNavigation)
                    .WithMany(p => p.JobOps)
                    .HasForeignKey(d => d.IdOp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_id_op");
            });

            modelBuilder.Entity<Maquina>(entity =>
            {
                entity.HasKey(e => e.IdMaq)
                    .HasName("pk_id_maq");

                entity.ToTable("maquina");

                entity.Property(e => e.IdMaq).HasColumnName("id_maq");

                entity.Property(e => e.Estado).HasColumnName("estado");
            });

            modelBuilder.Entity<Operacao>(entity =>
            {
                entity.HasKey(e => e.IdOp)
                    .HasName("pk_id_op");

                entity.ToTable("operacao");

                entity.Property(e => e.IdOp).HasColumnName("id_op");

                entity.Property(e => e.IdMaq).HasColumnName("id_maq");

                entity.HasOne(d => d.IdMaqNavigation)
                    .WithMany(p => p.Operacaos)
                    .HasForeignKey(d => d.IdMaq)
                    .HasConstraintName("fk_id_maq");
            });

            modelBuilder.Entity<SimJob>(entity =>
            {
                entity.HasKey(e => new { e.IdJob, e.IdSim })
                    .HasName("pk_job_sim");

                entity.ToTable("sim_job");

                entity.Property(e => e.IdJob).HasColumnName("id_job");

                entity.Property(e => e.IdSim).HasColumnName("id_sim");

                entity.HasOne(d => d.IdJobNavigation)
                    .WithMany(p => p.SimJobs)
                    .HasForeignKey(d => d.IdJob)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_id_job2");

                entity.HasOne(d => d.IdSimNavigation)
                    .WithMany(p => p.SimJobs)
                    .HasForeignKey(d => d.IdSim)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_id_sim");
            });

            modelBuilder.Entity<Simulacao>(entity =>
            {
                entity.HasKey(e => e.IdSim)
                    .HasName("pk_id_sim");

                entity.ToTable("simulacao");

                entity.Property(e => e.IdSim).HasColumnName("id_sim");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Simulacaos)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("fk_id_user");
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
