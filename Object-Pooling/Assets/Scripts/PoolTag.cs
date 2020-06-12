using UnityEngine;

public class PoolTag : MonoBehaviour
{
    //Item tag is the dict key to the pool it came from
    private string itemTag;
    public string ItemTag => itemTag;

    public void Init(string tag)
    {
        itemTag = tag;
    }
}


