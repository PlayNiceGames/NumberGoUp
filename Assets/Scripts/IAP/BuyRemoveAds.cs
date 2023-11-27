using UnityEngine;

public class BuyRemoveAds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("No_Ads", 0) == 1) HideButton();
        IAPManager.OnPurchaseDone += HideButton;
    }
    private void OnDestroy()
    {
        IAPManager.OnPurchaseDone -= HideButton;
    }

    void HideButton()
    {
        this.gameObject.SetActive(false);
    }
}
