using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles pools of objects. Other classes should use the pool manager to request pooled objects.
/// </summary>
public class PoolManager : MonoBehaviour
{

    public static PoolManager Instance { get; private set; }

    [SerializeField] private PoolInfo[] poolInfos;

    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        GeneratePools();
    }

    private void GeneratePools()
    {
        foreach(var info in poolInfos)
        {
            Pool pool = new Pool(info, this.transform);
            pools.Add(info.ID, pool);
        }
    }

    /// <summary>
    /// Call to retrieve a Gameobject from a pool.
    /// </summary>
    /// <param name="key">Which pool you want to gameobject to come from.</param>
    /// <returns></returns>
    public GameObject CheckoutItem(string key) => pools[key].RequestFromQueue();

    /// <summary>
    /// Pass an object back into the pool it came from. Object's not created from a pool will be destroyed.
    /// </summary>
    /// <param name="gameObject">Object to return.</param>
    public void ReturnItem(GameObject gameObject)
    {
        PoolTag tag = gameObject.GetComponent<PoolTag>();
        if (tag != null)
        {
            pools[tag.ItemTag].ReturnItem(gameObject);
        }
        else
        {
            Debug.LogError($"Warning: Trying to return object {gameObject.name} without pool tag. Object has been destroyed. " +
                $"Make sure you are not destroying the tag or trying to return an object that never belonged to a pool.");
            Destroy(gameObject);
        }
    }


    private class Pool
    {
        private PoolInfo info;

        private Transform poolParent;
        //For incrementing the name of the pool member
        private int nameCounter = 0; 
        //The total amount of objects that are a part of this pool. Includes objects that have been checked out.
        private int currentPoolSize = 0;

        private Queue<GameObject> queue = new Queue<GameObject>();

        public Pool(PoolInfo poolInfo, Transform parentTransform)
        {
            this.info = poolInfo;

            //Setup transform that will be used as the parent for all pool members
            poolParent = new GameObject().transform;
            poolParent.name = poolInfo.ID;
            poolParent.SetParent(parentTransform);

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < info.poolSize; i++)
            {
                CreatePoolMember();
            }
        }

        private void CreatePoolMember()
        {
            GameObject newObj = Instantiate(
                    info.gameobjects[UnityEngine.Random.Range(0, info.gameobjects.Length - 1)],
                    Vector3.zero,
                    Quaternion.identity);

            newObj.SetActive(false);
            newObj.AddComponent<PoolTag>().Init(info.ID);
            newObj.transform.SetParent(poolParent);
            newObj.name = info.ID + " " + nameCounter.ToString();
            nameCounter++;

            AddToQueue(newObj);
        }

        private void AddToQueue(GameObject obj)
        {
            currentPoolSize++;
            queue.Enqueue(obj);
        }

        
        //HACK?: Both of these methods are only called by the Pool Manager and it seems kind of weird
        public GameObject RequestFromQueue()
        {
            GameObject obj = null;

            if (queue.Count > 0)
            {
                obj = queue.Dequeue();
            }
            else
            {
                if (info.isExpandable && currentPoolSize < info.maxPoolSize)
                {
                    CreatePoolMember();
                    obj = RequestFromQueue(); //HACK?: This recursively calls this method. Not sure how big of an issue that might be
                }
                else
                {
                    Debug.LogError($"Warning: Trying to check out too many items from pool '{info.ID}'.");
                }
            }
            return obj;
        }
        public void ReturnItem(GameObject gameObject) => queue.Enqueue(gameObject);

    }
}

/// <summary>
/// This component is added to all GameObjects that are created as a part of a pool.
/// It IDs which pool the object came from originally so that it may be returned later.
/// </summary>
public class PoolTag : MonoBehaviour
{
    //Item tag is the dictionary key of the pool it came from
    private string itemTag;
    public string ItemTag => itemTag;

    public void Init(string tag)
    {
        itemTag = tag;
    }
}




