using System.Threading.Tasks;

public class FileProgressionProvider:IProgressionProvider
{
    PlayerProgressionService playerProgressionService;
    public async Task<bool> Initialize()
    {
        playerProgressionService = ServiceLocator.GetService<PlayerProgressionService>();
        await Task.Yield();
        return true;
    }
    public SaveGameJsonWrapper Load()
    {
        return SaveSystem.LoadGame();
    }
    public void Save()
    {
        SaveSystem.SaveGame(new SaveGameJsonWrapper(playerProgressionService));
    }
}