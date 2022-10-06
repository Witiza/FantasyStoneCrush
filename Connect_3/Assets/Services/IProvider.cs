using System.Threading.Tasks;

public interface IProvider
{
    Task<bool> Initialize();
}
