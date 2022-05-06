namespace CarPool.App.Settings
{
    internal class DALSettings
    {
        public string? ConnectionString { get; set; }
        public bool SkipMigrationAndSeedDemoData { get; set; }
    }
}
