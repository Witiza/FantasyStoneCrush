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
        SaveGameJsonWrapper saveGameJsonWrapper = SaveSystem.LoadClass<SaveGameJsonWrapper>(SaveSystem.ProgressionPath);
        if(saveGameJsonWrapper == null)
        {
           saveGameJsonWrapper = new SaveGameJsonWrapper();
        }
        return saveGameJsonWrapper;
    }
    public void Save()
    {
        SaveSystem.SaveClass(new SaveGameJsonWrapper(playerProgressionService), SaveSystem.ProgressionPath);
    }
}