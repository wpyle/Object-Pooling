//Created by William Pyle 2020 http://www.wpyle.com

using UnityEngine;

namespace WPyle.ObjectPooling
{
    [CreateAssetMenu(fileName = "New Pool", menuName = "Pool")]
    public class PoolInfo : ScriptableObject
    {
        public string ID;
        public int poolSize;
        public GameObject[] gameobjects;

        public bool isExpandable = false;
        public int maxPoolSize = 1000;
    }
}