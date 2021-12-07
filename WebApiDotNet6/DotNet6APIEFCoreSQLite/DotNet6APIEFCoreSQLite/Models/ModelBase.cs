using System.ComponentModel.DataAnnotations;

namespace DotNet6APIEFCoreSQLite.Models
{
    public abstract class ModelBase
    {
        [Key]
        public int Id { get; set; }
    }
}
