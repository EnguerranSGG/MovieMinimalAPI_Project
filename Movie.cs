namespace MovieMinimalAPI;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseYear { get; set; }
    public int Duration { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime PutDate { get; set; }
}

public class Actor
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public DateTime Birthdate { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime PutDate { get; set; }
}


public class ActorPost
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public int Age { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime PutDate { get; set; }
}