using UnityEngine;

public class Bonfire: SetLightAnimator, IInteractiveObject, IDamagable
{
    private void Start()
    {
        maxTime = 100; 
        anims = GetComponentsInChildren<Animator>();
        InvokeRepeating("TimeCount", 1, 1);
        FireLightSetFloat();
    }

    public void Interaction(float _time)
    {
        if (ResourceData.LogAmount >= 1)
        {
            ResourceData.LogAmount -= 1;
            leftTime += 10;
            if (leftTime >= maxTime) leftTime = maxTime;
            FireLightSetFloat();
        }
    }

    public void OnDamage(float _damage)
    {
        leftTime -= (int)_damage;
    }

    protected override void FireLightSetFloat()
    {
        foreach (Animator anim in anims)
        {
            anim.SetFloat("LeftTime", leftTime);
        }
    }

}
