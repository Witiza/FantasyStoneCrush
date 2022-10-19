using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;

public class GameIAPService : IStoreListener, IService
{
    public bool Initialized { get { return _initializationStatus == TaskStatus.RanToCompletion; } }
    private IStoreController _unityStoreController = null;
    TaskStatus _initializationStatus = TaskStatus.Created;
    TaskStatus _purchaseStatus = TaskStatus.Created;
    public bool IsReady => _initializationStatus == TaskStatus.RanToCompletion;
    public async Task Initialize(Dictionary<string,string> products)
    {
        _initializationStatus = TaskStatus.Running;
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach(KeyValuePair<string,string> product in products)
        {
            IDs ids = new IDs();
            ids.Add(product.Value, new[] {GooglePlay.Name});
            builder.AddProduct(product.Key, ProductType.Consumable, ids);
        }

        UnityPurchasing.Initialize(this, builder);
        while (_initializationStatus == TaskStatus.Running)
        {
            await Task.Delay(100);
        }
    }

    public async Task<bool> StartPurchase(string product)
    {
        if (!IsReady)
            return false;

        _purchaseStatus = TaskStatus.Running;
        _unityStoreController.InitiatePurchase(product);

        while (_purchaseStatus == TaskStatus.Running)
        {
            await Task.Delay(100);
        }

        return _purchaseStatus == TaskStatus.RanToCompletion;
    }

    public string GetLocalizedPrice(string product)
    {
        if (!IsReady)
            return string.Empty;

        Product unityProduct = _unityStoreController.products.WithID(product);
        return unityProduct?.metadata?.localizedPriceString;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _unityStoreController = controller;
        _initializationStatus = TaskStatus.RanToCompletion;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        _initializationStatus = TaskStatus.Faulted;
        Debug.LogError("Initialization failed with error: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        _purchaseStatus = TaskStatus.RanToCompletion;
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase failed with error: " + failureReason);
        _purchaseStatus = TaskStatus.Faulted;
    }
}
