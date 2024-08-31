using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Tree : MonoBehaviour, IUsable
{
    const float treeChopTimeConst = 1f;
    bool canChop = true;
    public int leftLog = 5;
    public GameObject logPrefeb;
    public void Use()
    {
        if (canChop)
        {
            canChop = false;
            StartCoroutine(ReUseTime(treeChopTimeConst));
            --leftLog;
            ResouceData.LogAmount++;
            Debug.Log("Chop!");
            if (leftLog <= 0) gameObject.SetActive(false);
        }
    }
    public IEnumerator ReUseTime(float _t)
    {
        yield return new WaitForSeconds(_t);
        canChop = true;
    }
}