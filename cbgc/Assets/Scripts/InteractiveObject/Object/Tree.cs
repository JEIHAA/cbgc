using System;
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private ObjectPoolManager.Pool pool;

    [SerializeField] private int leftFirewood = 5;
    [SerializeField] private int treeLevel = 3;
    public int TreeLevel => treeLevel;

    private TreeEffect treeEffect;
    private Animator animator;
    public Animator Anim => animator;

    private int axingCnt=0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        treeEffect = GetComponentInChildren<TreeEffect>();
        animator.SetInteger("TreeLevel", treeLevel);
    }

    public void Interaction(float _time)
    {
        if (_time > 90) return;
        if (_time > 1f)
        {
            DropFirewood();
            if (axingCnt > 1)
            {
                Chop();
            }
        }
    }

    private void DropFirewood()
    {
        animator.SetTrigger("Hit");
        //treeEffect.PlayRandomAnimation();
        axingCnt++;
        Debug.Log(axingCnt);
    }

    private void Chop()
    {
        Debug.Log("Chop!");
        --leftFirewood;
        ResourceData.LogAmount += 1;
        if (leftFirewood <= 0)
        {
            Invoke("Delay", 0.4f);
            ObjectPoolManager.instance.GetPool(pool).Release(gameObject);
        }
        axingCnt = 0;
        treeLevel -= 1;
        animator.SetInteger("TreeLevel", treeLevel);
    }


    private void Delay()
    {
        Debug.Log("Delay");
        return;
    }

}