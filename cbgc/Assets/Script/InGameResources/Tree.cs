using System;
using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private int leftFirewood = 5;
    private TreeEffect treeEffect;
    
    private Animator animator;
    public Animator Anim => animator;

    private int axingCnt=0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        treeEffect = GetComponentInChildren<TreeEffect>();
    }

    public void Use()
    {
        --leftFirewood;
        Debug.Log("Chop!");
        ResourceData.LogAmount += 1;
        if (leftFirewood <= 0)
        {
            Invoke("Delay", 0.4f);
            gameObject.SetActive(false);
        }
    }

    public void Interaction(float _time)
    {
        if (_time > 90) return;
        if (_time > 0.5f) 
        {
            animator.SetTrigger("Hit");
            //treeEffect.PlayRandomAnimation();
            axingCnt++;
        }
        if (axingCnt > 1)
        {
            Use();
            axingCnt = 0;
        }

    }

    private void Delay()
    {
        Debug.Log("Delay");
        return;
    }

}