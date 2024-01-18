using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    Transform place;
    public Transform Parent;
    [SerializeField] RectTransform tilePrefab;
    [SerializeField] List<RectTransform> pointList;

    public RectTransform layer;

    public Vector3 MapSize;
    public float xSpace, ySpace, zSpace;

    private Vector3 placeSize;
    private Vector3 placePos;

    private List<RectTransform> layers = new List<RectTransform>();

    public void generate()
    {
        pointList = new List<RectTransform>();
        layers = new List<RectTransform>();
        placeSize = new Vector3(MapSize.x, MapSize.y, MapSize.z);

        if (Parent.childCount >= 1)
        {
            for (int i = 0; i <= Parent.childCount; i++)
            {
                DestroyImmediate(Parent.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < placeSize.z; i++)
        {
            RectTransform newLayer = Instantiate(layer, Parent.position, Quaternion.Euler(Vector3.zero), Parent);
            newLayer.name = "Layer " + i;
            newLayer.localPosition = Vector3.zero;
            layers.Add(newLayer);
        }

        for (int x = 1; x <= placeSize.x; x++)
        {
            for (int y = 1; y <= placeSize.y; y++)
            {
                for (int z = 1; z <= placeSize.z; z++)
                {

                    RectTransform newTile = Instantiate(tilePrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), layers[z - 1].transform);
                    tilePrefab.localScale = Vector3.one;
                    Vector3 tilePosition = new Vector3(xSpace * x, ySpace * y, zSpace * z);
                    newTile.anchoredPosition3D = tilePosition;
                    pointList.Add(newTile);


                }
            }
        }

        int cont = (int)(MapSize.x * MapSize.z);
        float distance = Vector3.Distance(pointList[0].position, pointList[1].position) + 0.05f;

        /* if (!b)
         {
             for (int i = 0; i < pointList.Count; i++)
             {
                 PointController Pscript = pointList[i].GetComponent<PointController>();

                 if (Pscript.index < pointList.Count)
                 {
                     Transform checkleft = pointList[Pscript.index];
                     float Disleft = Vector3.Distance(pointList[i].position, checkleft.position);
                     if (Disleft < distance)
                         Pscript.left = checkleft.GetComponent<PointController>();
                 }

                 if (Pscript.index - 2 >= 0)
                 {
                     Transform checkright = pointList[Pscript.index - 2];
                     float Disright = Vector3.Distance(pointList[i].position, checkright.position);
                     if (Disright < distance)
                         Pscript.right = checkright.GetComponent<PointController>();
                 }

                 if (Pscript.index - MapSize.x > 0)
                 {
                     Transform checkback = pointList[Pscript.index - (int)MapSize.x - 1];
                     float Disback = Vector3.Distance(pointList[i].position, checkback.position);
                     if (Disback < distance)
                         Pscript.back = checkback.GetComponent<PointController>();
                 }

                 if (Pscript.index + MapSize.x <= pointList.Count)
                 {
                     Transform checkforward = pointList[Pscript.index + (int)MapSize.x - 1];
                     float Disforward = Vector3.Distance(pointList[i].position, checkforward.position);

                     if (Disforward < distance)
                         Pscript.forward = checkforward.GetComponent<PointController>();
                 }

                 if (Pscript.index <= MapSize.x || (Pscript.index > cont - MapSize.x && Pscript.index < cont + 1))
                 {
                     int d = 0;
                     Pscript.egg = true;
                     for (int j = 0; j < MapSize.y - 1; j++)
                     {
                         d += cont;
                         pointList[Pscript.index + d - 1].GetComponent<PointController>().egg = true;
                     }
                 }

                 if (Pscript.index > cont)
                     Pscript.underPoint = pointList[i - cont];

                 if (Pscript.index <= pointList.Count - cont)
                     Pscript.upperPoint = pointList[i + cont].GetComponent<PointController>();

             }
             b = true;
         }*/


    }
}