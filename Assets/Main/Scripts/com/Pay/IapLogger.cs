using UnityEngine;
using UnityEngine.Purchasing;
using game;
using System;

namespace com
{
    public class IapLogger : MonoBehaviour, IStoreListener
    {
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("IapLogger OnInitialized");
            Debug.Log(controller);
            Debug.Log(extensions);
            Debug.Log(controller.products.all.Length);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("IapLogger OnInitializeFailed");
            Debug.Log(error.ToString());
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log("IapLogger OnPurchaseFailed");
            Debug.Log("OnPurchaseFailed iap 失败 " + product.definition.id + " " + failureReason);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log("IapLogger ProcessPurchase");
            Debug.Log(purchaseEvent.purchasedProduct.definition.id);
            Debug.Log(purchaseEvent.purchasedProduct.metadata.localizedDescription);
            return PurchaseProcessingResult.Complete;
        }
    }
}