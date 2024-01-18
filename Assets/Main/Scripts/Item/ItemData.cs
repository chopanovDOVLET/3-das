using UnityEngine;

[System.Serializable]
public class ItemData : MonoBehaviour
{
    public enum Type { None, Dutar, Wheat, WaterMelon, Melon, Gopuz, BlueEye, Gulyaka, Jug, Pattern, Cotton, Tamdyr, Grape, Tahya, Semeni, Gol }
    public enum ItemState { None, Active, Inactive, Sellected, Collected }
    public enum EObstacle { None, Gum, Chain, Freeze }

    
    public static string[] sideNames = new string[6] { "front", "back", "left", "right", "up", "down" };

}