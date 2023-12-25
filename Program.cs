using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace MovieMinimalAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => { builder.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod(); });
            });

            // Register database connection
            builder.Services.AddSingleton<MySqlConnection>(_ => new MySqlConnection("Host=localhost;Port=3307;Username=root;Password=;Database=streaming;Pooling=true;"));

            var app = builder.Build();

            // Configure middleware
            app.UseCors();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/movies/{id}", (int id, MySqlConnection connection) => GetMovieById(id, connection));
            app.MapGet("/movies", GetAllMovie);
            app.MapGet("/movies_desc", GetAllMovieDesc);

            app.MapPost("/movies", (Movie movie, MySqlConnection connection) => AddMovie(movie, connection));
            app.MapPost("/actors", (ActorPost actor, MySqlConnection connection) => AddActor(actor, connection));

            app.MapPut("/movies/{id}", (int id, Movie movie, MySqlConnection connection) => UpdateMovie(id, movie, connection));

            app.MapDelete("/actors/{id}", (int id, MySqlConnection connection) => DeleteActor(id, connection));

            await app.RunAsync();
        }

        private static async Task<Movie[]> GetAllMovie()
        {
            List<Movie> movies = new();

            await using var connection = new MySqlConnection("Host=localhost;Port=3307;Username=root;Password=;Database=streaming;Pooling=true;");
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM Movie;", connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Movie movieToAdd = new()
                {
                    Id = reader.GetInt32("Id_movie"),
                    Title = reader.GetString("title"),
                    Duration = reader.GetInt32("duration"),
                    ReleaseYear = reader.GetDateTime("release_year"),
                    CreateDate = reader.GetDateTime("creation_date_movie"),
                    PutDate = reader.GetDateTime("modification_date_movie"),
                };
                movies.Add(movieToAdd);
            }

            return movies.ToArray();
        }

        private static async Task<Movie[]> GetAllMovieDesc()
        {
            List<Movie> movies = new();

            await using var connection = new MySqlConnection("Host=localhost;Port=3307;Username=root;Password=;Database=streaming;Pooling=true;Allow User Variables=true;");
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT title, release_year FROM Movie ORDER BY release_year DESC;", connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Movie movieToAdd = new()
                {
                    Title = reader.GetString("title"),
                    ReleaseYear = reader.GetDateTime("release_year"),
                };
                movies.Add(movieToAdd);
            }

            return movies.ToArray();
        }

        private static async Task<Movie> GetMovieById(int movieId, MySqlConnection connection)
        {
            Movie movie = null;

            await using (connection)
            {
                await connection.OpenAsync();

                string query = "SELECT title, release_year, Id_movie FROM Movie WHERE Id_movie = @movieId ;";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@movieId", movieId);

                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    movie = new Movie
                    {
                        Id = movieId,
                        Title = reader.GetString("title"),
                        ReleaseYear = reader.GetDateTime("release_year"),
                    };
                }
            }

            return movie;
        }

        private static async Task AddMovie([FromBody] Movie movie, MySqlConnection connection)
        {
            await using (connection)
            {
                await connection.OpenAsync();

                string query = "INSERT INTO Movie (title, release_year, duration, creation_date_movie, modification_date_movie) VALUES (@title, @releaseYear, @duration, @createDate, @putDate);";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@title", movie.Title);
                command.Parameters.AddWithValue("@releaseYear", movie.ReleaseYear);
                command.Parameters.AddWithValue("@duration", movie.Duration);
                command.Parameters.AddWithValue("@createDate", DateTime.Now);
                command.Parameters.AddWithValue("@putDate", DateTime.Now);

                await command.ExecuteNonQueryAsync();
            }
        }

        private static async Task AddActor([FromBody] ActorPost actor, MySqlConnection connection)
        {
            await using (connection)
            {
                await connection.OpenAsync();

                DateTime Birthdate = DateTime.Now.AddYears(-actor.Age);

                string query = "INSERT INTO Actor (firstname_actor, lastname_actor, birthdate_actor, creation_date_actor, modification_date_actor) VALUES (@Firstname, @Lastname, @Birthdate, @CreateDate, @PutDate);";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Firstname", actor.Firstname);
                command.Parameters.AddWithValue("@Lastname", actor.Lastname);
                command.Parameters.AddWithValue("@Birthdate", Birthdate);
                command.Parameters.AddWithValue("@CreateDate", DateTime.Now); 
                command.Parameters.AddWithValue("@PutDate", DateTime.Now);    

                await command.ExecuteNonQueryAsync();
            }
        }

        private static async Task UpdateMovie(int Id_movie, Movie movie, MySqlConnection connection)
        {
            await connection.OpenAsync();

            try
            {
                string query = "UPDATE Movie SET title = @title, release_year = @release_year, duration = @duration, modification_date_movie = @PutDate WHERE Id_movie = @Id_movie;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id_movie", Id_movie);
                command.Parameters.AddWithValue("@title", movie.Title);
                command.Parameters.AddWithValue("@release_year", movie.ReleaseYear);
                command.Parameters.AddWithValue("@duration", movie.Duration);
                command.Parameters.AddWithValue("@PutDate", DateTime.Now);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                connection.Close();
            }
        }

        private static async Task DeleteActor(int Id_actor, MySqlConnection connection)
        {
            await connection.OpenAsync();

            try
            {
                string query = "DELETE FROM Actor WHERE Id_actor = @Id_actor;";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id_actor", Id_actor);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}


