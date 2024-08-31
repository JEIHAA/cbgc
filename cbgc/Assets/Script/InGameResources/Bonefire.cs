using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonefire : InteractiveObject, IUsable
{
    [SerializeField] private int leftTime = 66;
    [SerializeField] private Animator lightAnim;
    [SerializeField] private Animator torchAnim;
    [SerializeField] private int maxTime = 100;

    private Animator anim;
        
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("TimeCount", 1, 1);
    }

    private void TimeCount()
    {
        Debug.Log(leftTime);
        leftTime -= 1;
        anim.SetFloat("leftTime", leftTime);
        lightAnim.SetFloat("leftTime", leftTime);
        torchAnim.SetFloat("leftTime", leftTime);
        if (leftTime <= 0)
        {
            Debug.Log("BoneFire is Dead.");
        }
    }


    public void Use()
    {
        Debug.Log("Use");
        leftTime += 10;
        if (leftTime >= maxTime) leftTime = maxTime; 
        anim.SetFloat("leftTime", leftTime);
        lightAnim.SetFloat("leftTime", leftTime);
    }

    public override void Interaction(float _time)
    {
        if (ResourceData.LogAmount >= 1)
        {
            Debug.Log("firewood -1");
            Use();
        }
        else Debug.Log("need firewood");
    }

    public IEnumerator ReUseTime(float _time)
    {
        throw new System.NotImplementedException();
    }
}
