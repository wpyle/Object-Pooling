using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO:
/// -Check to make sure when an object is requested that the pool has an available object to give
///     -If it doesn't create a new object (if the pool is expandable, otherwise I guess throw an error and return null?)
/// -Change the dict to reference a class that contains a queue so that you also have access to other info.
///         -How many objects are a part of that pool (checked out and not)
///         -if the pool is expandable
///         -The max amount the pool can expand to
/// 
///     
/// -Pools that contain more than one type of game object spawn objects into the pool randomly. Should I offer extra options as to how
///     objects are spawned into the pool?
/// </summary>
public class PoolManager : MonoBehaviour
{
    private class Pool
    {
        private PoolInfo info;

        private Transform poolParent;
        private int nameCounter = 0; //Just for incrementing the name of the pool member
        
        //The total amount of objects that are a part of this pool. Includes objects that have been checked out
        private int currentPoolSize = 0;

        private Queue<GameObject> queue = new Queue<GameObject>();

        public Pool(PoolInfo poolInfo, Transform parentTransform)
        {
            this.info = poolInfo;

            poolParent = new GameObject().transform;
            poolParent.name = poolInfo.ID;
            poolParent.SetParent(parentTransform);

            Init();
        }

        private void Init()
        {
            //Create GameObjects in queue
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

            newObj.name = info.ID + " " + nameCounter.ToString();
            nameCounter++;
            newObj.SetActive(false);
            newObj.AddComponent<PoolTag>().Init(info.ID);
            newObj.transform.SetParent(poolParent);

            AddToQueue(newObj);
        }

        private void AddToQueue(GameObject obj)
        {
            currentPoolSize++;
            queue.Enqueue(obj);
        }

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

    public static PoolManager Instance { get; private set; }

    public PoolInfo[] poolInfos;

    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this); }

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

    //TODO: Will need to do more work here to assure we have items to give out
    public GameObject CheckoutItem(string key) => pools[key].RequestFromQueue();

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
}




