using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightAnimator : MonoBehaviour
{
    [SerializeField] protected int leftTime = 0;
    [SerializeField] protected int maxTime;

    protected Animator[] anims;


    protected void TimeCount()
    {
        if (leftTime > 0) leftTime -= 1;
        BonfireSetFloat();
        if (leftTime <= 0)
        {
            Debug.Log("BoneFire is Dead.");
        }
    }

    protected virtual void BonfireSetFloat()
    {
        foreach (Animator anim in anims)
        { 
            anim.SetFloat("LeftTime", leftTime);
        }
    }
}
