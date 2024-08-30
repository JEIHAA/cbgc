using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonefire : MonoBehaviour, IUsable
{
    WaitForSeconds wait;
    bool canUse = true;
    int fireLevel = 4;
    int FireLevel
    {
        get { return fireLevel; }
        set { fireLevel = value; transform.localScale = Vector3.one * fireLevel; }
    }
        
    // Start is called before the first frame update
    void Start()
    {
        wait = new(1f);
        StartCoroutine(FireLevelDown());
    }
    IEnumerator FireLevelDown()
    {
        while (true)
        {
            yield return wait;
            --FireLevel;
            if(FireLevel == 0)
            {
                Debug.Log("BoneFire is Dead.");
                gameObject.SetActive(false);
                break;
            }
        }
        
    }
    public IEnumerator ReUseTime(float _t)
    {
        yield return new WaitForSeconds(_t);
        canUse = true;
    }
    public void Use()
    {
        if (canUse)
        {
            canUse = false;
            StartCoroutine(ReUseTime(1f));
            FireLevel = 4;
        }

    }
}
