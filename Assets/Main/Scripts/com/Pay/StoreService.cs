using UnityEngine;
using UnityEngine.Purchasing;
using System;

namespace com
{
    public class StoreService : MonoBehaviour
    {
        protected IStoreController controller;
        protected ITransactionHistoryExtensions m_TransactionHistoryExtensions;
        protected IExtensionProvider extensions;//provided to your application when Unity IAP initializes successfully.

        protected Action _sucCb;
        protected Action _failCb;

        protected bool _purchaseInProgress;

        public virtual void InitBuilder()
        {

        }

        public virtual bool isEnabled()
        {
            return false;
        }

        public virtual void OnPurchaseClicked(string productId)
        {
        }

        public virtual void OnPurchaseClicked(string productId, Action sucCb, Action failCb)
        {
        }

        public virtual Product GetIapProduct(string id)
        {
            return null;
        }

        public void LogProducts()
        {
            Debug.Log("内购 LogProducts");
            Debug.Log(controller);
            Debug.Log(controller.products);
            Debug.Log(controller.products.all);
            foreach (var product in controller.products.all)
            {
                LogProduct(product);
            }
        }

        public static void LogProduct(Product product)
        {
            Debug.Log("内购 LogProduct");
            if (product == null)
            {
                Debug.Log("product null");
                return;
            }
            // TextFormat.LogObj(product);
            Debug.Log(string.Join(" - ",
               new[]
               {
                        product.metadata.localizedTitle,//Fake title for diamond_v1_1
                         product.metadata.localizedPriceString,
                        product.metadata.localizedDescription,
                        product.metadata.isoCurrencyCode,
                        product.metadata.localizedPrice.ToString(),
                        product.metadata.localizedPriceString,
                        product.transactionID,
                        product.receipt
               }));

            Debug.Log(product.definition.id);//diamond_v1_1
            Debug.Log(product.definition.storeSpecificId);//diamond_v1_1
            Debug.Log(product.availableToPurchase);//true
            Debug.Log(product.hasReceipt);

            if (product.definition.payout != null)
            {
                Debug.Log(product.definition.payout.data);
                Debug.Log(product.definition.payout.subtype);
                Debug.Log(product.definition.payout.quantity);
            }
        }

    }
}