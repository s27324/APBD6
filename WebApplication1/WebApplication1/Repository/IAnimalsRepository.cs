using WebApplication1.Model;

namespace WebApplication1.Repository;

public interface IAnimalsRepository
{
    IEnumerable<Animal> GetAnimals(string column = "Name");
    int CreateAnimal(Animal animal);
    int UpdateAnimal(Animal animal);
    int DeleteAnimal(int idAnimal);
}