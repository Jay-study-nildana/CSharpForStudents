namespace LoggingWithEFCoreSQLite.DB
{
    public class LogItem
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
