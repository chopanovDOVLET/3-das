using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAP_Manager : MonoBehaviour
{
    const string coin250 = "com.Onki.TravelTile.Coin_250";
    const string coin780 = "com.Onki.TravelTile.Coin_780";
    const string coin1600 = "com.Onki.TravelTile.Coin_1600";
    const string coin4200 = "com.Onki.TravelTile.Coin_4200";
    const string coin6800 = "com.Onki.TravelTile.Coin_6800";
    const string coin12500 = "com.Onki.TravelTile.Coin_12500";
    const string bundlePack = "com.Onki.TravelTile.BundlePack";

    public void OnPurchaseComplete(Product product)
    {
        switch (product.definition.id)
        {
            case coin250: 
                ResourcesData.instance.AddCoin(250);
                break;
            case coin780: 
                ResourcesData.instance.AddCoin(780);
                break;
            case coin1600: 
                ResourcesData.instance.AddCoin(1600);
                break;
            case coin4200: 
                ResourcesData.instance.AddCoin(4200);
                break;
            case coin6800: 
                ResourcesData.instance.AddCoin(6800);
                break;
            case coin12500: 
                ResourcesData.instance.AddCoin(12500);
                break;
            case bundlePack: 
                ResourcesData.instance.AddCoin(500);
                ResourcesData.instance.AddReturnTile(5);
                ResourcesData.instance.AddMix(5);
                break;
        }
    }

    public void OnPurchaseFailure(Product product, PurchaseFailureDescription reason)
    {
        Debug.Log("Your purchase failed because " + reason);
    }
    
}
