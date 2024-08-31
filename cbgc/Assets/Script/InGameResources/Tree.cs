using System;
using System.Collections;
using UnityEngine;

public class Tree : InteractiveObject
{
    private Animator animator;
    public Animator Anim => animator;

    public int leftFirewood = 5;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Use()
    {
        --leftFirewood;
        Debug.Log("Chop!");
        if (leftFirewood <= 0) gameObject.SetActive(false);
    }

    public IEnumerator ReUseTime(float _time)
    {
        yield return new WaitForSeconds(_time);
    }
    

    public override void Interaction(float _time)
    {
        if (_time > 1f)
        {
            Debug.Log("Tree Interaction");
            animator.SetTrigger("Hit");
            Use();
        }
    }
}