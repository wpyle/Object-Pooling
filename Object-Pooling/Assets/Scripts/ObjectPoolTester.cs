//Created by William Pyle 2020 http://www.wpyle.com

using System.Collections.Generic;
using UnityEngine;
using WPyle.ObjectPooling;

public class ObjectPoolTester : MonoBehaviour
{
    public string poolKey = "Pool1";
    public List<GameObject> stuffList;

    [EditorButton("Get Object", green: true)]
    private void RequestObj()
    {
        var obj = PoolManager.Instance.CheckoutItem(poolKey);
        stuffList.Add(obj);
        obj.transform.position = new Vector3(Random.Range(0,100),Random.Range(0,100),Random.Range(0,100));
        obj.SetActive(true);
    }

    [EditorButton("Return Object", red: true)]
    private void ReturnRandObj()
    {
        var obj = stuffList[Random.Range(0, stuffList.Count)];
        stuffList.Remove(obj);
        PoolManager.Instance.ReturnItem(obj);
    }
}


