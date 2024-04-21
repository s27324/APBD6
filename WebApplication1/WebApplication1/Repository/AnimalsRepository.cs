using Microsoft.Data.SqlClient;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Repository;

public class AnimalsRepository : IAnimalsRepository
{
    private IConfiguration _configuration;

    public AnimalsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Animal> GetAnimals(string column = "Name")
    {
        if (column.Contains(","))
        {
            column = column.Split(",")[0];
        }
        
        using var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        sqlConnection.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = sqlConnection;
        cmd.CommandText = "SELECT IdAnimal, Name, Description, Category, Area FROM ANIMAL ORDER BY " + column;

        var dr = cmd.ExecuteReader();
        var animals = new List<Animal>();
        while (dr.Read())
        {
            var grade = new Animal
            {
                IdAnimal = (int)dr["IdAnimal"],
                Name = dr["Name"].ToString(),
                Description = dr["Description"].ToString(),
                Category = dr["Category"].ToString(),
                Area = dr["Area"].ToString()
            };
            animals.Add(grade);
        }

        return animals;
    }

    public int CreateAnimal(Animal animal)
    {
        using var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        sqlConnection.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = sqlConnection;
        cmd.CommandText = "INSERT INTO Animal(Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);

        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }

    public int UpdateAnimal(Animal animal)
    {
        using var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        sqlConnection.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = sqlConnection;
        cmd.CommandText = "UPDATE Animal SET Name=@Name, Description=@Description, Category=@Category, Area=@Area WHERE IdAnimal=@IdAnimal";
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);
        cmd.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }

    public int DeleteAnimal(int idAnimal)
    {
        using var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        sqlConnection.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = sqlConnection;
        cmd.CommandText = "DELETE FROM Animal WHERE IdAnimal=@IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", idAnimal);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
}