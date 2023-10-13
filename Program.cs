using System.Text.Json;
using MySql.Data.MySqlClient;

using (var connection = Database.GetConnection())
{
    connection.Open();

    CreatePokemonTable(connection);

    var client = new HttpClient();
    client.BaseAddress = new Uri($"https://pokeapi.co/api/v2/pokemon/");

    for (int i = 1; i <= 151; i++)
    {
        var response = client.GetAsync($"{i}").Result;
        string json = response.Content.ReadAsStringAsync().Result;
        var pokemon = JsonSerializer.Deserialize<Pokemon>(json);
        AddPokemon(connection, pokemon);
    }

    int pokemonCount = GetPokemonCount(connection);
    Console.WriteLine($"Geslaagd! {pokemonCount} gebruikers toegevoegd.");
}

static void CreatePokemonTable(MySqlConnection connection)
{
    using (var command = new MySqlCommand(
        "CREATE TABLE IF NOT EXISTS Pokemons (Id INT, Name VARCHAR(255), Weight INT, Height INT)",
        connection))
    {
        command.ExecuteNonQuery();
    }
}

static void AddPokemon(MySqlConnection connection, Pokemon user)
{
    using (var command = new MySqlCommand(
        "INSERT INTO Pokemons (Id, Name, Weight, Height) VALUES (@Id, @Name, @Weight, @Height)",
        connection))
    {
        command.Parameters.AddWithValue("@Id", user.id);
        command.Parameters.AddWithValue("@name", user.name);
        command.Parameters.AddWithValue("@Weight", user.weight);
        command.Parameters.AddWithValue("@Height", user.height);
        command.ExecuteNonQuery();
    }
}

static int GetPokemonCount(MySqlConnection connection)
{
    using (var command = new MySqlCommand("SELECT COUNT(*) FROM Pokemons", connection))
    {
        return Convert.ToInt32(command.ExecuteScalar());
    }
}