using Azure.Core;
using FinalHackBank.CORE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace FinalHackBank.EF
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Bid> Bids { get; set; }

        public virtual DbSet<Call> Calls { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Decision> Decisions { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Documentt> Documentts { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Requestt> Requestts { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }

        public virtual DbSet<StatusDemand> StatusDemands { get; set; }

        public virtual DbSet<StatusUser> StatusUsers { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=DESKTOP-0188551\\SQLEXPRESS;Database=gestionachats;User Id=sa;Password=HackBank;TrustServerCertificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(e => e.BidId).HasName("PK__Bids__4A733D920495A34B");

                entity.Property(e => e.AmountTtc).HasColumnName("AmountTTC");
                entity.Property(e => e.Winner).HasColumnName("winner");

                entity.HasOne(d => d.Company).WithMany(p => p.Bids)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyId");

                entity.HasOne(d => d.Document).WithMany(p => p.Bids)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentId");

                entity.HasOne(d => d.IdcallNavigation).WithMany(p => p.Bids)
                    .HasForeignKey(d => d.Idcall)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Idcallb");
            });

            modelBuilder.Entity<Call>(entity =>
            {
                entity.HasKey(e => e.CallId).HasName("PK__Calls__5180CFAAD9D4ED45");

                entity.Property(e => e.Descriptions)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Remark)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Status).WithMany(p => p.Calls)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StatusIdCall");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Company__1788CC4CA37F0857");

                entity.ToTable("Company");

                entity.Property(e => e.UserId).ValueGeneratedNever();
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.User).WithOne(p => p.Company)
                    .HasForeignKey<Company>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserIdc");
            });

            modelBuilder.Entity<Decision>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Decision__3214EC07B509F1E2");

                entity.ToTable("Decision");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07D4468F3A");

                entity.ToTable("Department");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Documentt>(entity =>
            {
                entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF0FE5C79C75");

                entity.ToTable("Documentt");

                entity.Property(e => e.Content).HasColumnType("text");
                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Employee__1788CC4CC2C9C9CB");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DepartmentId");

                entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleIde");

                entity.HasOne(d => d.User).WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserIde");
            });

            modelBuilder.Entity<Requestt>(entity =>
            {
                entity.HasKey(e => e.RequestId).HasName("PK__Requestt__33A8517A261D9DD3");

                entity.ToTable("Requestt");

                entity.Property(e => e.Daterequest)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Description).HasColumnType("text");

                entity.HasOne(d => d.Decision).WithMany(p => p.Requestts)
                    .HasForeignKey(d => d.DecisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DecisionId");

                entity.HasOne(d => d.Employee).WithMany(p => p.Requestts)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_EmployeeId");

                entity.HasOne(d => d.Status).WithMany(p => p.Requestts)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StatusId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07078F5DCD");

                entity.ToTable("Role");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07DBF242EA");

                entity.ToTable("Status");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusDemand>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__StatusDe__3214EC07C59F2A50");

                entity.ToTable("StatusDemand");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusUser>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__StatusUs__3214EC07F4F237DA");

                entity.ToTable("StatusUser");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C1AB39A04");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.StatusUser).WithMany(p => p.Users)
                    .HasForeignKey(d => d.StatusUserId)
                    .HasConstraintName("FK_StatusUserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
