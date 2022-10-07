using System.Collections.Generic;

public interface ILevelProvider:IProvider
{
    bool loaded { get;}
    int priority { get; set; }
    List<BoardConfig> GetLevels();
    void SaveLevels(List<BoardConfig> levels);
}
