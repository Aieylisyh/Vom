using UnityEngine;
using UnityEngine.Purchasing;
using vom;
using System;
using System.Collections.Generic;

namespace com
{
    public class AppstoreService : StoreService, IStoreListener
    {
        private IAppleExtensions m_AppleExtensions;

        private void Awake()
        {
            _purchaseInProgress = false;
        }

        public override void InitBuilder()
        {
            Debug.Log("apple UnityPurchasing InitBuilder");
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            var cfg = ConfigSystem.instance.payConfig;
            foreach (var p in cfg.pays)
            {
                builder.AddProduct(p.iapId, p.type);
                Debug.Log("add IAP to ConfigurationBuilder  " + p.iapId);
            }

            // Initialize Unity IAP...
            UnityPurchasing.Initialize(this, builder);
        }

        public override bool isEnabled()
        {
            var module = StandardPurchasingModule.Instance();

            // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and
            // developer ui (initialization, purchase, failure code setting). These correspond to
            // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
            //module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            var builder = ConfigurationBuilder.Instance(module);

            bool canMakePayments = builder.Configure<IAppleConfiguration>().canMakePayments;
            if (!canMakePayments)
            {
                return false;
            }

            return !_purchaseInProgress;

            // Use the products defined in the IAP Catalog GUI.
            // E.g. Menu: "Window" > "Unity IAP" > "IAP Catalog", then add products, then click "App Store Export".
            var catalog = ProductCatalog.LoadDefaultCatalog();
            foreach (var product in catalog.allValidProducts)
            {
                if (product.allStoreIDs.Count > 0)
                {
                    var ids = new IDs();
                    foreach (var storeID in product.allStoreIDs)
                    {
                        ids.Add(storeID.id, storeID.store);
                    }
                    builder.AddProduct(product.id, product.type, ids);
                }
                else
                {
                    builder.AddProduct(product.id, product.type);
                }
            }
        }

        /// <summary>
        /// Called when Unity IAP is ready to make purchases.
        /// </summary>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("iap OnInitialized");
            this.controller = controller;
            this.extensions = extensions;
            m_TransactionHistoryExtensions = extensions.GetExtension<ITransactionHistoryExtensions>();

            //On Apple platforms users must enter their password to retrieve previous transactions 
            //so your application must provide users with a button letting them do so. 
            //During this process the ProcessPurchase 
            //method of your IStoreListener will be invoked for any items the user already owns.
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            m_AppleExtensions.RestoreTransactions(result =>
            {
                if (result)
                {
                    Debug.Log("ios RestoreTransactions result ok");
                    // This does not mean anything was restored, merely that the restoration process succeeded.
                }
                else
                {
                    Debug.Log("ios RestoreTransactions result failed");
                    // Restoration failed.
                }
            });
            // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
            // On non-Apple platforms this will have no effect; OnDeferred will never be called.
            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

            LogProducts();

            Dictionary<string, string> product_details = m_AppleExtensions.GetProductDetails();
            foreach (var i in product_details)
            {
                Debug.Log(i.Key + ": " + i.Value);
            }
            // Set all these products to be visible in the user's App Store according to Apple's Promotional IAP feature
            // https://developer.apple.com/library/content/documentation/NetworkingInternet/Conceptual/StoreKitGuide/PromotingIn-AppPurchases/PromotingIn-AppPurchases.html
            //如果卖的东西很有诱惑性，可以使用Apple's Promotional IAP feature 这里卖的都是钻石 就算了
            //m_AppleExtensions.SetStorePromotionVisibility(item, AppleStorePromotionVisibility.Show);

#if SUBSCRIPTION_MANAGER
        Dictionary<string, string> introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();
#endif
            // Sample code for expose product sku details for apple store
            //Dictionary<string, string> product_details = m_AppleExtensions.GetProductDetails();

        }

        /// <summary>
        /// iOS Specific.
        /// This is called as part of Apple's 'Ask to buy' functionality,
        /// when a purchase is requested by a minor and referred to a parent
        /// for approval.
        ///
        /// When the purchase is approved or rejected, the normal purchase events
        /// will fire.
        /// </summary>
        /// <param name="item">Item.</param>
        private void OnDeferred(Product item)
        {
            Debug.Log("Purchase deferred: " + item.definition.id);
        }

        /// <summary>
        /// Called when Unity IAP encounters an unrecoverable initialization error.
        ///
        /// Note that this will not be called if Internet is unavailable; Unity IAP
        /// will attempt initialization until it becomes available.
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed");
            Debug.Log(error);
            switch (error)
            {
                case InitializationFailureReason.AppNotKnown:
                    Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                    break;
                case InitializationFailureReason.PurchasingUnavailable:
                    // Ask the user if billing is disabled in device settings.
                    Debug.Log("Billing disabled!");
                    break;
                case InitializationFailureReason.NoProductsAvailable:
                    // Developer configuration error; check product metadata.
                    Debug.Log("No products available for purchase!");
                    break;
            }
        }

        public override Product GetIapProduct(string id)
        {
            Debug.Log("GetIapProduct " + id);
            if (controller == null || controller.products == null || controller.products.all == null)
            {
                return null;
            }

            foreach (var product in controller.products.all)
            {
                //LogProduct(product);
                if (product != null && id == product.definition.id)
                    return product;
            }

            return null;
        }

        public override void OnPurchaseClicked(string productId)
        {
            if (controller == null)
            {
                Debug.LogError("Purchasing is not initialized");
                return;
            }
            if (controller.products.WithID(productId) == null)
            {
                Debug.LogError("No product has id " + productId);
                return;
            }
            if (_purchaseInProgress)
            {
                Debug.Log("!OnPurchaseClicked while _purchaseInProgress " + productId);
                return;
            }

            Debug.Log("!OnPurchaseClicked " + productId);
            _purchaseInProgress = true;
            controller.InitiatePurchase(productId);
        }

        public override void OnPurchaseClicked(string productId, Action sucCb, Action failCb)
        {
            _sucCb = sucCb;
            _failCb = failCb;
            OnPurchaseClicked(productId);
        }
        //PurchaseProcessingResult.Complete 
        //The application has finished processing the purchase and should not be informed of it again.

        //PurchaseProcessingResult.Pending 
        //The application is still processing the purchase and ProcessPurchase
        //will be called again the next time the Application starts, 
        //unless the ConfirmPendingPurchase function of IStoreController is called.

        /// <summary>
        /// Called when a purchase completes.
        ///
        /// May be called at any time after OnInitialized().
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Debug.Log("Purchase OK: " + args.purchasedProduct.definition.id);
            Debug.Log("Receipt: " + args.purchasedProduct.receipt);
            Debug.Log("ProcessPurchase 发放iap奖励");
            //TODO  try to use PayoutDefinitions later

            _sucCb?.Invoke();
            if (args.purchasedProduct.definition.payouts != null)
            {
                Debug.Log("Purchase complete, paying out based on defined payouts");
                foreach (var payout in args.purchasedProduct.definition.payouts)
                {
                    Debug.Log(string.Format("Granting {0} {1} {2} {3}", payout.quantity, payout.typeString, payout.subtype, payout.data));
                }
            }

            _purchaseInProgress = false;
            Product product = args.purchasedProduct;
            LogProduct(product);
            Debug.Log($"Purchase Complete - Product: {product.definition.id}");

            // Indicate if we have handled this purchase.
            //   PurchaseProcessingResult.Complete: ProcessPurchase will not be called
            //     with this product again, until next purchase.
            //   PurchaseProcessingResult.Pending: ProcessPurchase will be called
            //     again with this product at next app launch. Later, call
            //     m_Controller.ConfirmPendingPurchase(Product) to complete handling
            //     this purchase. Use to transactionally save purchases to a cloud
            //     game service.
#if DELAY_CONFIRMATION
        StartCoroutine(ConfirmPendingPurchaseAfterDelay(args.purchasedProduct));
        return PurchaseProcessingResult.Pending;
#else
            return PurchaseProcessingResult.Complete;
#endif

            //You must not return PurchaseProcessingResult.Complete 
            //if you are selling consumable products and fulfilling them from a server (for example, providing currency in an online game).

            //单机游戏可以在这里返回Complete
            //Unity IAP本质上是一个云端的服务，不是本地的一套程序 因此需要通知unity IAP一些状态
            //如果是网络游戏，进一步参考这里https://docs.unity3d.com/Packages/com.unity.purchasing@4.1/manual/UnityIAPProcessingPurchases.html

            //PurchaseProcessingResult.Pending
            //The application is still processing the purchase and ProcessPurchase will be called again the next time the Application starts, 
            // unless the ConfirmPendingPurchase function of IStoreController is called.

            //为了防止用户恶意破解游戏内容，通过修改游戏数值来获得付费内容，可以参考功能Receipt Obfuscation
            //https://docs.unity3d.com/Packages/com.unity.purchasing@4.1/manual/UnityIAPValidatingReceipts.html
        }

#if DELAY_CONFIRMATION
    private HashSet<string> m_PendingProducts = new HashSet<string>();

    private IEnumerator ConfirmPendingPurchaseAfterDelay(Product p)
    {
        m_PendingProducts.Add(p.definition.id);
        Debug.Log("Delaying confirmation of " + p.definition.id + " for 5 seconds.");

		var end = Time.time + 5f;

		while (Time.time < end) {
			yield return null;
			var remaining = Mathf.CeilToInt (end - Time.time);
			UpdateProductPendingUI (p, remaining);
		}

        Debug.Log("Confirming purchase of " + p.definition.id);
        controller.ConfirmPendingPurchase(p);
        m_PendingProducts.Remove(p.definition.id);
    }
#endif

        /// <summary>
        /// Called when a purchase fails.
        /// </summary>
        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            Debug.Log("OnPurchaseFailed iap失败");
            Debug.Log(p);
            _failCb?.Invoke();
            _purchaseInProgress = false;
            if (p == PurchaseFailureReason.PurchasingUnavailable)
            {
                // IAP may be disabled in device settings.
            }
        }
        //Unity IAP provides purchase receipts as a JSON hash containing the following keys and values:
    }
}