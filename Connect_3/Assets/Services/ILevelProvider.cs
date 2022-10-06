using System.Collections.Generic;

public interface ILevelProvider:IProvider
{
    bool loaded { get;}
    List<BoardConfig> GetLevels();
    void SetLevels();
    void SaveLevels();
}
