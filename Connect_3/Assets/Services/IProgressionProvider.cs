using System.Threading.Tasks;

public interface IProgressionProvider
{
    Task<bool> Initialize();
    SaveGameJsonWrapper Load();
    void Save();
}
