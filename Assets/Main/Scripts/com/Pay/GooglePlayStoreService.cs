using UnityEngine;
using UnityEngine.Purchasing;
using game;
using System;

namespace com
{
    public class GooglePlayStoreService : StoreService, IStoreListener
    {
        private IGooglePlayStoreExtensions m_GpExtensions;
        IStoreController m_StoreController; // The Unity Purchasing system.

        private void Awake()
        {
            //Debug.Log("GP iap awake");
            _purchaseInProgress = false;
        }

        public override void InitBuilder()
        {
            Debug.Log("gp InitBuilder 0528");
            //Debug.Log(StandardPurchasingModule.Instance().useFakeStoreAlways);

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            // builder = ConfigurationBuilder.Instance(Google.Play.Billing.GooglePlayStoreModule.Instance());
            // Google.Play.Billing.GooglePlayStoreModule.Instance());
            Debug.Log(builder);
            Debug.Log(builder.Configure<IGooglePlayConfiguration>());
            ConfigureGoogleFraudDetection(builder.Configure<IGooglePlayConfiguration>());

            builder.Configure<IGooglePlayConfiguration>().SetServiceDisconnectAtInitializeListener(
                () =>
                {
                    Debug.Log("Unable to connect to the Google Play Billing service");
                });

            var cfg = ConfigService.instance.payConfig;
            foreach (var p in cfg.pays)
            {
                builder.AddProduct(p.iapId, p.type);
                Debug.Log("add IAP to ConfigurationBuilder  " + p.iapId);
            }

            UnityPurchasing.Initialize(this, builder);
            Debug.Log("GP iap InitBuilder end");
        }

        void ConfigureGoogleFraudDetection(IGooglePlayConfiguration googlePlayConfiguration)
        {
            Debug.Log("GP ConfigureGoogleFraudDetection");

            //To make sure the account id and profile id do not contain personally identifiable information, we obfuscate this information by hashing it.
            //var obfuscatedAccountId = HashString(user.AccountId);
            //var obfuscatedProfileId = HashString(user.ProfileId);

            //googlePlayConfiguration.SetObfuscatedAccountId(obfuscatedAccountId);
            //googlePlayConfiguration.SetObfuscatedProfileId(obfuscatedProfileId);
        }

        public override bool isEnabled()
        {
            return !_purchaseInProgress;
        }

        /// <summary>
        /// Called when Unity IAP is ready to make purchases.
        /// </summary>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("gp iap OnInitialized ok!");
            this.controller = controller;
            this.extensions = extensions;
            m_TransactionHistoryExtensions = extensions.GetExtension<ITransactionHistoryExtensions>();
            m_GpExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
            Debug.Log(m_GpExtensions);

            m_GpExtensions.RestoreTransactions(result =>
            {
                if (result)
                {
                    Debug.Log("gp RestoreTransactions result ok");
                    // This does not mean anything was restored,
                    // merely that the restoration process succeeded.
                }

                else
                {
                    Debug.Log("gp RestoreTransactions result failed");
                    // Restoration failed.
                }
            });

            LogProducts();
        }


        /// <summary>
        /// Called when Unity IAP encounters an unrecoverable initialization error.
        ///
        /// Note that this will not be called if Internet is unavailable; Unity IAP
        /// will attempt initialization until it becomes available.
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("GP OnInitializeFailed");
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
            Debug.Log("p");
            foreach (var product in controller.products.all)
            {
                LogProduct(product);
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

        Debug.Log("Is " + p.definition.id + " currently owned, according to the Google Play store? "   + m_GpExtensions.IsOwned(p));
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