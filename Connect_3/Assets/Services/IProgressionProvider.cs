public interface IProgressionProvider:IProvider
{
    SaveGameJsonWrapper Load();
    void Save();
}
