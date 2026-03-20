using System;

class Movie_With_Validation
{
    // Movie class with validation for release year and rating.
    public class Movie
    {
        public string Title { get; }
        public int ReleaseYear { get; }
        private double _rating;
        public double Rating
        {
            get => _rating;
            private set
            {
                if (value < 0.0 || value > 10.0) throw new ArgumentOutOfRangeException(nameof(Rating));
                _rating = value;
            }
        }

        public Movie(string title, int releaseYear, double rating)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            if (releaseYear < 1888 || releaseYear > DateTime.Now.Year + 1)
                throw new ArgumentOutOfRangeException(nameof(releaseYear), "Unrealistic release year.");
            ReleaseYear = releaseYear;
            Rating = rating;
        }

        public override string ToString() => $"{Title} ({ReleaseYear}) - Rating: {Rating:F1}";
    }

    static void Main()
    {
        var m = new Movie("Inception", 2010, 8.8);
        Console.WriteLine(m);
        try
        {
            var bad = new Movie("Future Flick", 3000, 5.0);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("Validation caught: " + ex.Message);
        }
    }
}