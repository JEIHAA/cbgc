using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightAnimator : MonoBehaviour
{
    [SerializeField] protected int leftTime = 0;
    public int LeftTime { get => leftTime; set => leftTime = value; }

    [SerializeField] protected int maxTime;

    protected Animator[] anims;

    protected void TimeCount()
    {
        if (leftTime > 0) leftTime -= 1;
        if (leftTime < 0) leftTime = 0;
        FireLightSetFloat();
    }

    protected virtual void FireLightSetFloat()
    {
        foreach (Animator anim in anims)
        { 
            anim.SetFloat("LeftTime", leftTime);
        }
    }
}
