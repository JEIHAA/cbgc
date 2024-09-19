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

    private Animator animator;
    public Animator Anim => animator;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D coll;

    private int axingCnt=0;
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

    private void Update()
    {
        
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
    }

    private void Chop()
    {
        Debug.Log("Chop!");
        ResourceData.LogAmount += 1;
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