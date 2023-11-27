using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Events;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private IStoreController m_StoreController;          // The Unity Purchasing system.
    private IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private Product product = null;
    private IAppleExtensions m_AppleExtensions;
    private IGooglePlayStoreExtensions m_GoogleExtensions;
    private string No_Ads = "com.playnicegames.numbergoup.removeads";
    private bool return_complete = true;
    private ConfigurationBuilder builder;
    public static UnityAction OnPurchaseDone;

    void Awake()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(No_Ads, ProductType.NonConsumable);
        MyDebug("Starting Initialized...");
        UnityPurchasing.Initialize(this, builder);
        ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
        foreach (ProductCatalogItem product in catalog.allProducts)
        {
            MyDebug("Product = " + product.id);
        }

        if (m_StoreController.products.WithID(No_Ads) != null && m_StoreController.products.WithID(No_Ads).hasReceipt)
        {
            MyDebug("remove ads purchased already");
            PlayerPrefs.SetInt("No_Ads", 1);
        }
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyRemoveAds()
    {
        MyDebug("Its here to buy remove ads");
        if (PlayerPrefs.GetInt("No_Ads", 0) == 0)
        {
            BuyProductID(No_Ads);
        }
        else
        {
            OnPurchaseDone?.Invoke();
        }
    }

    void CompletePurchase()
    {
        if (product == null)
            MyDebug("Cannot complete purchase, product not initialized.");
        else
        {
            PlayerPrefs.SetInt("No_Ads", 1);
            OnPurchaseDone?.Invoke();
            m_StoreController.ConfirmPendingPurchase(product);
            //FetchProducts();
            MyDebug("Completed purchase with " + product.definition.id.ToString());
        }
    }

    public void RestorePurchases()
    {
        //m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
        //    if (result)
        //    {
        //        MyDebug("Restore purchases succeeded.");
        //    }
        //    else
        //    {
        //        MyDebug("Restore purchases failed.");
        //    }
        //});
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            UnityEngine.Purchasing.Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                MyDebug(string.Format("Purchasing product:" + product.definition.id.ToString()));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                MyDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            MyDebug("BuyProductID FAIL. Not initialized.");
        }
    }

    public void ListProducts()
    {
        foreach (UnityEngine.Purchasing.Product item in m_StoreController.products.all)
        {
            if (item.receipt != null)
            {
                MyDebug("Receipt found for Product = " + item.definition.id.ToString());
            }
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        MyDebug("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_GoogleExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        Dictionary<string, string> dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

        foreach (UnityEngine.Purchasing.Product item in controller.products.all)
        {
            if (item.receipt != null)
            {
                string intro_json = (dict == null || !dict.ContainsKey(item.definition.storeSpecificId)) ? null : dict[item.definition.storeSpecificId];

                if (item.definition.type == ProductType.Subscription)
                {
                    SubscriptionManager p = new SubscriptionManager(item, null);
                    SubscriptionInfo info = p.getSubscriptionInfo();
                    MyDebug("SubInfo: " + info.getProductId().ToString());
                    MyDebug("getExpireDate: " + info.getExpireDate().ToString());
                    MyDebug("isSubscribed: " + info.isSubscribed().ToString());
                }
            }
        }
    }

    void OnPurchaseDeferred(Product product)
    {

        MyDebug("Deferred product " + product.definition.id.ToString());
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        MyDebug("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        product = args.purchasedProduct;

        //var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

        // var result = validator.Validate(args.purchasedProduct.receipt);

        // foreach (IPurchaseReceipt productReceipt in result)
        // {
        //     MyDebug("Valid receipt for " + productReceipt.productID.ToString());
        // }

        //MyDebug("Validate = " + result.ToString());

        MyDebug("Receipt:" + product.receipt.ToString());

        if (return_complete)
        {
            CompletePurchase();
            MyDebug(string.Format("ProcessPurchase: Complete. Product:" + args.purchasedProduct.definition.id + " - " + product.transactionID.ToString()));
            return PurchaseProcessingResult.Complete;
        }
        else
        {
            MyDebug(string.Format("ProcessPurchase: Pending. Product:" + args.purchasedProduct.definition.id + " - " + product.transactionID.ToString()));
            return PurchaseProcessingResult.Pending;
        }

    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
    {
        MyDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    private void MyDebug(string debug)
    {
        Debug.Log(debug);
        //myText.text += "\r\n" + debug;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        MyDebug("Initialization failed :" + message);
        //throw new NotImplementedException();
    }
}
