using UnityEngine;

public class Bonefire: SetLightAnimator, IInteractiveObject
{
    private void Start()
    {
        maxTime = 100; 
        anims = GetComponentsInChildren<Animator>();
        InvokeRepeating("TimeCount", 1, 1);
    }

    private void Use()
    {
        Debug.Log("Use");
        leftTime += 10;
        if (leftTime >= maxTime) leftTime = maxTime;
        BonfireSetFloat();
    }

    public void Interaction(float _time)
    {
        if (ResourceData.LogAmount >= 1)
        {
            ResourceData.LogAmount -= 1;
            Debug.Log("firewood -1");
            Use();
        }
        else Debug.Log("need firewood");
    }


    protected override void BonfireSetFloat()
    {
        foreach (Animator anim in anims)
        {
            anim.SetFloat("LeftTime", leftTime);
        }
    }

}
