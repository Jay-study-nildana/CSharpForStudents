using System;
using System.ComponentModel.DataAnnotations;

namespace crudaspnetcore3point1mysql.Models
{
    // a simple watchlist tracker
    //contains one reference to the user's unique i.e. email address
    //contains a reference to the movie ID that is automatically generated
    //by the sql database.
    public class WatchList
    {
    
        public int Id { get; set; }
    
        public string UserEmailAddress {get;set;}

        public int movieID {get;set;}
    }
}