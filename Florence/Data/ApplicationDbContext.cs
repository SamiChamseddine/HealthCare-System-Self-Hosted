using Microsoft.EntityFrameworkCore;
using Florence.Models;

namespace Florence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNumber)
                .IsUnique();

            modelBuilder.Entity<ExpenseReport>()
                .HasOne(er => er.Patient)
                .WithMany(p => p.ExpenseReports)
                .HasForeignKey(er => er.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseItem>()
                .HasOne(ei => ei.Report)
                .WithMany(er => er.Items)
                .HasForeignKey(ei => ei.ExpenseReportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseItem>()
                .HasOne(ei => ei.Nurse)
                .WithMany(n => n.ExpenseItems)
                .HasForeignKey(ei => ei.NurseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}