namespace TodoApi.Models
{
    public class TodoItem
    {
        //The Id property functions as the unique key in a relational database.
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
