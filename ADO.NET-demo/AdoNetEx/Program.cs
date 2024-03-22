using System.Data.SqlClient;

namespace AdoNetEx
{
    internal class Program
    {
        const string connectionString = @"Server=DESKTOP-9FL9J1C;Database=MinionsDB;Integrated Security=True";
        const string getVillainsById = @"SELECT Name FROM Villains WHERE Id = @Id ";
        const string getMinionsOfVillains = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                                m.Name, 
                                                m.Age
                                        FROM MinionsVillains AS mv
                                            JOIN Minions As m ON mv.MinionId = m.Id
                                        WHERE mv.VillainId = @Id
                                        ORDER BY m.Name";

        static SqlConnection sqlConnection;
        static async Task Main(string[] args)
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                await GetOrderedMinionsByVillainId(1);
            }
            finally { sqlConnection.Close(); }
        }

        static async Task GetOrderedMinionsByVillainId(int id)
        {
            using SqlCommand command = new SqlCommand(getVillainsById, sqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            var result = await command.ExecuteScalarAsync();
            if (result is null)
            {
                await Console.Out.WriteLineAsync($"No villain with ID {id} exists in the database.");
            }
            else
            {
                await Console.Out.WriteLineAsync($"Villain: {result}");

                using SqlCommand commandGetMinions = new SqlCommand(getMinionsOfVillains, sqlConnection);
                commandGetMinions.Parameters.AddWithValue("@Id", id);
                
                var minionsReader=await commandGetMinions.ExecuteReaderAsync();

                while (await minionsReader.ReadAsync())
                {
                    await Console.Out.WriteLineAsync($"{minionsReader["RowNum"]} {minionsReader["Name"]} {minionsReader["Age"]}");
                }
            }
        }
    }

}