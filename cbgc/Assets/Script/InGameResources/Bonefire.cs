using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonefire : MonoBehaviour, IUsable
{
    [SerializeField] private int leftTime = 100;
    private WaitForSeconds delay;
    private bool canUse = true;

    private Animator ani;
        
    void Start()
    {
        ani = GetComponent<Animator>();
        delay = new WaitForSeconds(1f);
        InvokeRepeating("TimeCount", 1, 1);
    }

    private void TimeCount()
    {   
        leftTime -= 1;
        ani.SetFloat("leftTime", leftTime);
        if (leftTime <= 0)
        {
            Debug.Log("BoneFire is Dead."); 
        }
    }

    public IEnumerator ReUseTime(float _t)
    {
        canUse = false;
        leftTime += 10;
        yield return delay;
        canUse = true;
    }


    public void Use()
    {
        if (canUse)
        {
            leftTime += 10;
            ani.SetFloat("leftTime", leftTime);
        }
    }
}
