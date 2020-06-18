//Created by William Pyle 2020 http://www.wpyle.com

using System.Collections.Generic;
using UnityEngine;

namespace WPyle.ObjectPooling
{
    /// <summary>
    /// Handles pools of objects. Other classes should use the pool manager to request pooled objects.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [SerializeField] private PoolInfo[] poolInfos;
        
        //TODO: Is there something other than a string I could use as the key?
        private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

        private void Awake()
        {
            if (Instance == null)
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
            foreach (var info in poolInfos)
            {
                Pool pool = new Pool(info, this.transform);
                pools.Add(info.ID, pool);
            }
        }

        /// <summary>
        /// Call to retrieve a GameObject from a pool.
        /// </summary>
        /// <param name="key">Which pool you want to GameObject to come from.</param>
        /// <returns></returns>
        public GameObject CheckoutItem(string key) => pools[key].RequestFromQueue();

        /// <summary>
        /// Pass an object back into the pool it came from. Object's not created from a pool will be destroyed.
        /// </summary>
        /// <param name="gameObj">Object to return.</param>
        public void ReturnItem(GameObject gameObj)
        {
            var gameObjectTag = gameObj.GetComponent<PoolTag>();
            if (gameObjectTag != null)
            {
                pools[gameObjectTag.ItemTag].ReturnItem(gameObj);
            }
            else
            {
                Debug.LogError($"Warning: Trying to return object {gameObj.name} without pool tag. Object has been destroyed. " +
                    $"Make sure you are not destroying the tag or trying to return an object that never belonged to a pool.");
                Destroy(gameObj);
            }
        }
        
        private class Pool
        {
            private PoolInfo _info;
            //The transform parent of pool objects in the hierarchy 
            private Transform _poolParent;
            //For incrementing the name of the pool member
            private int _nameCounter = 0;
            //The total amount of objects that are a part of this pool. Includes objects that have been checked out.
            private int _currentPoolSize = 0;

            private Queue<GameObject> _queue = new Queue<GameObject>();

            public Pool(PoolInfo poolInfo, Transform parentTransform)
            {
                this._info = poolInfo;

                //Setup transform that will be used as the parent for all pool members
                _poolParent = new GameObject().transform;
                _poolParent.name = poolInfo.ID;
                _poolParent.SetParent(parentTransform);

                Init();
            }

            private void Init()
            {
                for (var i = 0; i < _info.poolSize; i++)
                {
                    CreatePoolMember();
                }
            }

            private void CreatePoolMember()
            {
                var newObj = Instantiate(
                        _info.gameobjects[UnityEngine.Random.Range(0, _info.gameobjects.Length - 1)],
                        Vector3.zero,
                        Quaternion.identity);

                newObj.SetActive(false);
                newObj.AddComponent<PoolTag>().Init(_info.ID);
                newObj.transform.SetParent(_poolParent);
                newObj.name = _info.ID + " " + _nameCounter.ToString();
                _nameCounter++;

                AddToQueue(newObj);
            }

            private void AddToQueue(GameObject obj)
            {
                _currentPoolSize++;
                _queue.Enqueue(obj);
            }
            
            public GameObject RequestFromQueue()
            {
                GameObject obj = null;

                if (_queue.Count > 0)
                {
                    obj = _queue.Dequeue();
                }
                else
                {
                    if (_info.isExpandable && _currentPoolSize < _info.maxPoolSize)
                    {
                        CreatePoolMember();
                        obj = RequestFromQueue();
                    }
                    else
                    {
                        Debug.LogError($"Warning: Trying to check out too many items from pool '{_info.ID}'.");
                    }
                }
                return obj;
            }

            public void ReturnItem(GameObject gameObject)
            {
                gameObject.SetActive(false);
                gameObject.transform.position = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.SetParent(_poolParent);
                _queue.Enqueue(gameObject);
            }
        }
    }
}