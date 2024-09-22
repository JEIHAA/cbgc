using System;
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private ObjectPoolManager.Pool pool;
    [SerializeField] private int treeLevel = 3;
    public int TreeLevel {
        get => treeLevel;
        set => treeLevel = value;
    }

    [SerializeField] private int axingPerLevel= 1; // 레벨 당 도끼질 가능 횟수
    [SerializeField] private int fwPerAxing = 1; // 도끼질 당 장작 수

    private Animator animator;
    public Animator Anim => animator;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D coll;

    private int axingCnt = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        coll = GetComponent<CapsuleCollider2D>();
        animator.SetInteger("TreeLevel", treeLevel);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interaction(float _time)
    {
        if (_time > 90) return;
        if (_time > 1f)
        {
            HitTree();
            if (axingCnt >= axingPerLevel)
            { 
                Chop();
            }
        }
    }

    private void HitTree()
    {
        animator.SetTrigger("Hit");
        axingCnt++;
    }

    private void Chop()
    {
        axingCnt = 0;
        Debug.Log("Chop!");
        ResourceData.LogAmount += fwPerAxing;
        treeLevel -= 1;
        SetTreeLevel();
        if (treeLevel <= 0)
        {
            Debug.Log(TreeLevel);
            Destroy(coll);
            spriteRenderer.sortingOrder = -1;
        }
    }

    private void SetTreeLevel()
    {
        animator.SetInteger("TreeLevel", treeLevel);
    }
}