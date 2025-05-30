using Microsoft.EntityFrameworkCore;

namespace zad.Data
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) : base(options) {}

        public DbSet<MySchemaRecord> MySchema { get; set; } = null!;
    }

    public class MySchemaRecord
    {
        public int Id { get; set; }
        public string? apparatusId { get; set; }
        public string? apparatusVersion { get; set; }
        public string? apparatusSensorType { get; set; }
        public string? apparatusTubeType { get; set; }
        public string? temperature { get; set; }
        public string? value { get; set; }
        public string? hitsNumber { get; set; }
        public string? calibrationFunction { get; set; }
        public string? startTime { get; set; }
        public string? endTime { get; set; }
    }
}
