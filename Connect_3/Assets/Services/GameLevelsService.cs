using System.Threading.Tasks;
using System.Collections.Generic;

//This service is currently useless, as I am using adressables for the level list, and they are automatically updated. I have decided to leave it, since I think its more scalable than adressables, and if I had found a way
//to change the RemoteConfig from script, I could have done a level editor tool etc.
public class GameLevelsService: IService
{
    public bool Initialized { get { return levels != null && levels.Count > 0; } }
    public List<BoardConfig> levels;
    List<ILevelProvider> levelProviders = new List<ILevelProvider>();

    public async Task Initialize()
    {
        DefaultLevelProvider defaultProvider = new DefaultLevelProvider();
        defaultProvider.priority = 0;
        await defaultProvider.Initialize();
        levelProviders.Add(defaultProvider);
        FileLevelProvider fileLevelProvider = new FileLevelProvider();
        fileLevelProvider.priority = 1;
        await fileLevelProvider.Initialize();
        levelProviders.Add(fileLevelProvider);
        RemoteLevelProvider remoteLevelProvider = new RemoteLevelProvider();
        remoteLevelProvider.priority = 2;
        await remoteLevelProvider.Initialize();
        levelProviders.Add(remoteLevelProvider);
        selectLevels();
        await Task.Yield();
    }

    void selectLevels()
    {
        levelProviders.Sort((x, y) => y.priority.CompareTo(x.priority));
        for(int i = 0;i<levelProviders.Count;i++)
        { 
            levels = levelProviders[i].GetLevels();
            if(levels!=null&&levels.Count>0)
            {
                break;
            }
        }
    }

    public void SaveLevels()
    {
        foreach(ILevelProvider levelProvider in levelProviders)
        {
            levelProvider.SaveLevels(levels);
        }
    }
    public void Clear()
    {

    }
} 
