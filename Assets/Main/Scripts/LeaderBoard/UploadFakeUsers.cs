using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UploadFakeUsers : MonoBehaviour
{
    private const string webURL = "https://focalboard.bellicreative.com/order_uch"; 
    private string[] names =
    {
        "Aýdyň", "Azat", "Baýram", "Begenç", "Geldi", "Dowlet", "Ylýas", "Gurbanmyrat", "Nurmuhammet",
        "Pirmyrat", "Röwşen", "Sapa", "Saparmurat", "Serdar", "Süleýman", "Täçgeldi", "Toýly", "Ýalkym",
        "Ýazmyrat", "Ýunus", "Aleksey", "Aleksandr", "Arslan", "Begzad", "Farhad", "Ýagmyr", "Yslam",
        "Aýgül", "Bahar", "Gurt", "Gül", "Jahan", "Mähri", "Nurjan", "Ogulbibi", "Parwana", "Selbi",
        "Soltan", "Täzegül", "Ýulduz", "Aýjemal", "Bibigül", "Ýanyl", "Mähriban", "Nury", "Oguljan",
        "Parahat", "Babahan", "Baky", "Balkan", "Baýly", "Batyr", "Bägül", "Bahar", "Bibi", "Bike",
        "Guwanç", "Hatyja", "Hurma", "Mahym", "Melek", "Nowgül", "Maýa", "Nazar", "Nadyr", "Nobat",
        "Nowruz", "Nohur", "Omar", "Osman", "Öwez", "Pena", "Hurma", "Gylyç", "Garly", "Gandym",
        "Erkin", "Fazyl", "Emin", "Ene", "Ejegyz", "Durmuş", "Daýanç", "Çary", "Joşgun", "Jelil",
        "Dileg", "Dursun", "Durdy", "Derýa", "Juma", "Didar", "Abadan", "Aýsere", "Altyn", "Akjagül",
        "Aýlar", "Aýnur", "Aksoltan"
    };
    
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            var username = $"{i}_{names[Random.Range(0, 100)]}";
            var score = Random.Range(0, 1001);
            var icon = Random.Range(0, 6);
            StartCoroutine(Add(username, score, icon));
        }
    }

    public IEnumerator Add(string username, int score, int icon)
    {
        WWW www = new WWW(webURL + "/add/" + username + "/" + score + "/" + icon);
        yield return www;
        
        if (string.IsNullOrEmpty(www.error))
            print("Upload " + username + " data Successful");
        else 
            print("Error " + username + " data uploading" + www.error);
    }
}



