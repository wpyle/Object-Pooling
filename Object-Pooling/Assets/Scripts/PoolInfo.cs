//Created by William Pyle 2020 http://www.wpyle.com

using UnityEngine;

namespace WPyle.ObjectPooling
{
    //TODO: instead of having a pool ID just use the name of the scriptable object as the pool key?
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