using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTester : MonoBehaviour
{
    public MyObject myObject;
    public List<MyObject> objList = new List<MyObject>();
    
    private void Start()
    {
        //Create a new pool of a specific kind of object. Specify how many to keep alive in the pool (100)
        ObjectPoolManager.CreateNewPool(myObject, 100);

        //Request 10 objects from the pool
        objList.Add(ObjectPoolManager.GetObjects(typeof(MyObject), 10));

        //Return 10 objects to the pool
        foreach(var obj in objList)
        {
            obj.ReturnToPool();
        }

        //Request 110 objects from the pool (should return 110 after generating 10 more)
        objList.Add(ObjectPoolManager.GetObjects(typeof(MyObject), 110));

        //Return 110 objects to the pool (maybe it should remove those 10 objects?)
        foreach (var obj in objList)
        {
            obj.ReturnToPool();
        }
    }
}

public class ObjectPool
{
    List<GameObject> pool = new List<GameObject>();


}

public class ObjectPoolManager
{
    //Create a hash table of pools where the key is the type?

    public static void CreateNewPool(GameObject myObject, int number)
    {
        throw new NotImplementedException();
    }

    internal static List<GameObject> GetObjects(Type type, int v)
    {
        throw new NotImplementedException();
    }
}

public interface IPoolable
{

}

