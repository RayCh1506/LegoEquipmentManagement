using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace EquipmentManagementPlatform.Repository.Context
{
    public class EquipmentContext : DbContext
    {
        public EquipmentContext(DbContextOptions<EquipmentContext> options) : base(options) { }

        public DbSet<EquipmentEntity> Equipment { get; set; }
        public DbSet<EquipmentHistoricStateEntity> EquipmentHistoricState { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EquipmentEntity>()
                .HasMany(e => e.HistoricStates)
                .WithMany();

            modelBuilder.Entity<EquipmentEntity>()
            .HasData(
                new EquipmentEntity(1, "EQ758490", "A1.01", EquipmentState.GREEN, true, string.Empty, new List<EquipmentHistoricState>(), 5443, new List<int> { 5443 }, 98.2, "EMP123 - FirstName1 LastName1"),
                new EquipmentEntity(2, "EQ125896", "A2.11", EquipmentState.RED, false, "Machine is blocked", new List<EquipmentHistoricState>(), new List<int>(), 12, "EMP666 - FirstName2 LastName2 "),
                new EquipmentEntity(3, "EQ147852", "B3.11", EquipmentState.YELLOW, true, string.Empty, new List<EquipmentHistoricState>(), new List<int>(), 76.5, "EMP006 - FirstName3 LastName3 "),
                new EquipmentEntity(4, "EQ125987", "B3.07", EquipmentState.GREEN, true, string.Empty, new List<EquipmentHistoricState>(), 1111, new List<int> { 1111 }, 96.7, "EMP223 - FirstName4 LastName4 "),
                new EquipmentEntity(5, "EQ136574", "B3.01", EquipmentState.RED, true, string.Empty, new List<EquipmentHistoricState>(), new List<int>(), 77.9, "EMP874 - FirstName5 LastName5 "),
                new EquipmentEntity(6, "EQ174444", "C2.02", EquipmentState.YELLOW, true, string.Empty, new List<EquipmentHistoricState>(), new List<int>(), 84.4, string.Empty),
                new EquipmentEntity(7, "EQ195874", "C3.03", EquipmentState.GREEN, true, string.Empty, new List<EquipmentHistoricState>(), 2222, new List<int> { 2222 }, 99.5, "EMP999 - FirstName6 LastName6 "),
                new EquipmentEntity(8, "EQ132132", "C1.05", EquipmentState.RED, false, "No power", new List<EquipmentHistoricState>(), new List<int>(), 41.2, string.Empty),
                new EquipmentEntity(9, "EQ456484", "D1.09", EquipmentState.YELLOW, true, string.Empty, new List<EquipmentHistoricState>(), new List<int>(), 86.9, string.Empty),
                new EquipmentEntity(10, "EQ741874", "D1.05", EquipmentState.GREEN, true, string.Empty, new List<EquipmentHistoricState>(), 3333, new List<int> { 3333 }, 81.4, "EMP365 - FirstName7 LastName7 "),
                new EquipmentEntity(11, "EQ896214", "D1.03", EquipmentState.RED, true, string.Empty, new List<EquipmentHistoricState>(), new List<int>(), 86.2, "EMP547 - FirstName8 LastName8 "),
                new EquipmentEntity(12, "EQ745635", "E2.01", EquipmentState.YELLOW, true, string.Empty, new List<EquipmentHistoricState>(), new List<int>(), 77, "EMP874 - - FirstName9 LastName9 "),
                new EquipmentEntity(13, "EQ555555", "E2.02", EquipmentState.GREEN, true, string.Empty, new List<EquipmentHistoricState>(), 4444, new List<int> { 4444 }, 79.4, "EMP852 - FirstName10 LastName10 "),
                new EquipmentEntity(14, "EQ785412", "E2.03", EquipmentState.RED, false, "A very long error message about the equipment fault", new List<EquipmentHistoricState>(), new List<int>(), 6.4, "EMP854 - FirstName11 LastName11  "),
                new EquipmentEntity(15, "EQ951375", "F6.11", EquipmentState.YELLOW, true, string.Empty, new List<EquipmentHistoricState>(), new List<int>(), 66.7, "EMP965 - FirstName12 LastName12 "),
                new EquipmentEntity(16, "EQ749625", "F7.17", EquipmentState.GREEN, true, string.Empty, new List<EquipmentHistoricState>(), 5555, new List<int> { 5555 }, 83.6, "EMP148 - FirstName13 LastName13 ")
            );
        }
    }
}
