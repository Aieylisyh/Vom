using UnityEngine;
using UnityEngine.Purchasing;
using game;
using System;

namespace com
{
    public class CodelessIapService : MonoBehaviour
    {
        public void OnPurchaseComplete(Product i)
        {
            Debug.Log("CodelessIapService OnPurchaseComplete! " + i.definition.id);
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            Debug.Log("CodelessIapService OnPurchaseFailed " + i.definition.id + " " + p);
            Debug.Log(p);
        }
    }
}