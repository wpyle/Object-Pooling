//Created by William Pyle 2020 http://www.wpyle.com

using System.Collections.Generic;
using UnityEngine;
using WPyle.ObjectPooling;

public class ObjectPoolTester : MonoBehaviour
{
    public string poolKey = "Pool1";
    public List<GameObject> stuffList;

    [EditorButton("Get Object", green: true)]
    private void RequestObj() => stuffList.Add(PoolManager.Instance.CheckoutItem(poolKey));

    [EditorButton("Return Object", red: true)]
    private void ReturnRandObj()
    {
        GameObject obj = stuffList[Random.Range(0, stuffList.Count)];
        stuffList.Remove(obj);
        PoolManager.Instance.ReturnItem(obj);
    }
}


