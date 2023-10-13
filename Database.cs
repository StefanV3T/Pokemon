using MySql.Data.MySqlClient;

public class Database
{
    public const string ConnectionString = "Server=localhost;Database=pokemon;User=bit_academy;Password=bit_academy;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }
}