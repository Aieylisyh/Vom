using UnityEngine;
using UnityEngine.Purchasing;
using System;
using vom;

namespace com
{
    public class PayService : MonoBehaviour
    {
        public static PayService instance { get; private set; }

        public AppstoreService appstoreService;
        public GooglePlayStoreService googlePlayStoreService;

        private void Awake()
        {
            instance = this;
        }

        public StoreService store
        {
            get
            {
                //Debug.Log("Get");
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    //Debug.Log("IPhonePlayer");
                    return appstoreService;
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    //Debug.Log("Android");
                    return googlePlayStoreService;
                }

                // Debug.Log("none");
                return null;
            }
        }

        private void Start()
        {
            //to run Fake Store
            //Set StandardPurchasingModule.Instance().useFakeStoreAlways to true

            StandardPurchasingModule.Instance().useFakeStoreAlways = false;

            Debug.Log("PayService Start " + Application.platform + " " + this.gameObject);
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.Log("iap IPhonePlayer");
                appstoreService.gameObject.SetActive(true);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("iap Android");
                googlePlayStoreService.gameObject.SetActive(true);
            }
        }

        public void InitBuilder()
        {
            if (store == null)
                return;

            //Debug.Log("InitBuilder");
            store.InitBuilder();
        }

        public bool isEnabled()
        {
            if (store == null)
                return false;

            return store.isEnabled();
        }

        /////////////////////////////////buy process///////////////////
        ///
        // Example method called when the user presses a 'buy' button
        // to start the purchase process.
        public void OnPurchaseClicked(string productId)
        {
            if (store == null)
                return;

            store.OnPurchaseClicked(productId);
        }

        public void OnPurchaseClicked(string productId, Action sucCb, Action failCb)
        {
            if (store == null)
                return;

            store.OnPurchaseClicked(productId, sucCb, failCb);
        }

        public string GetIapIdByCommodity(string commodityId)
        {
            Debug.Log("GetIapIdByCommodity " + commodityId);
            var cfg = ConfigSystem.instance.payConfig;
            foreach (var p in cfg.pays)
            {
                if (p.commodityId == commodityId)
                {
                    Debug.Log(p.iapId);
                    return p.iapId;
                }
            }

            return "";
        }

        public Product GetIapProduct(string id)
        {
            Debug.Log("GetIapProduct " + id);
            Debug.Log(store);
            if (store == null)
                return null;

            return store.GetIapProduct(id);
        }
    }
}