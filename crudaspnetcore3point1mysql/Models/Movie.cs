using System;
using System.ComponentModel.DataAnnotations;

namespace crudaspnetcore3point1mysql.Models
{
    public class Movie
    {
        //The Movie class contains an Id field, which is required by the database for the primary key.
        public int Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}