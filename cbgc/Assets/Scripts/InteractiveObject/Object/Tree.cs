using System;
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private ObjectPoolManager.Pool pool;

    [SerializeField] private int treeLevel = 3;
    public int TreeLevel => treeLevel;

    private CapsuleCollider2D coll;
    private Animator animator;
    public Animator Anim => animator;

    private int axingCnt=0;

    private void Start()
    {
        coll = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        animator.SetInteger("TreeLevel", treeLevel);
    }

    public void Interaction(float _time)
    {
        if (_time > 90) return;
        if (_time > 1f)
        {
            DropFirewood();
            Chop();
        }
    }

    private void DropFirewood()
    {
        animator.SetTrigger("Hit");
        axingCnt++;
        Debug.Log(axingCnt);
    }

    private void Chop()
    {
        Debug.Log("Chop!");
        ResourceData.LogAmount += 1;
        if (treeLevel <= 0)
        {
            Destroy(coll);
        }
        axingCnt = 0;
        treeLevel -= 1;
        animator.SetInteger("TreeLevel", treeLevel);
    }
}