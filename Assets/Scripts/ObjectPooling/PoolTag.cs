using UnityEngine;

namespace WPyle.ObjectPooling
{
    /// <summary>
    /// This component is added to all GameObjects that are created as a part of a pool.
    /// It IDs which pool the object came from originally so that it may be returned later.
    /// </summary>
    public class PoolTag : MonoBehaviour
    {
        //Item tag is the dictionary key of the pool it came from
        public string ItemTag { get; private set; }

        public void Init(string itemTag) => ItemTag = itemTag;
        public void ReturnToPool() => PoolManager.Instance.ReturnItem(this.gameObject);
    }
}