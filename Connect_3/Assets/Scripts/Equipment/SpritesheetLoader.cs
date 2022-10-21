using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

[System.Serializable]
public class SpritesheetLoader
{
    bool IsLoaded = false;
    public AssetReferenceT<SpriteAtlas> iconSheet;
    public Sprite defaultSprite;
    private AsyncOperationHandle<SpriteAtlas> _spriteOperation;
    

    public void Load()
    {
        _spriteOperation = iconSheet.LoadAssetAsync();
        _spriteOperation.Completed += LoadingCompleted;
    }

    private void LoadingCompleted(AsyncOperationHandle<SpriteAtlas> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                IsLoaded = true;
                break;
            case AsyncOperationStatus.Failed:
                Debug.LogError("Sprite load failed.");
                break;
        }
    }

    public Sprite getSprite(string id)
    {
        if (IsLoaded)
        {
            return _spriteOperation.Result.GetSprite(id);
        }
        else
        {
            return defaultSprite;
        }
    }
    public void Unload()
    {
        if (_spriteOperation.IsValid())
        {
            Addressables.Release(_spriteOperation);
            Debug.Log("Successfully released sprite load operation.");
        }
    }
}
