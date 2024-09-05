using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    // ���� --
    [SerializeField] private Animator animator;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] outlineSprites;
    [SerializeField] private RuntimeAnimatorController defaultAnimController; // ù ��° �ִϸ����� ��Ʈ�ѷ�
    [SerializeField] private RuntimeAnimatorController outlineAnimController; // �� ��° �ִϸ����� ��Ʈ�ѷ�

    [SerializeField] private bool isNearest = false;
    public bool IsNearest
    {
        get => isNearest;
        set
        {
            // ���� ����� ���� ���� ����
            if (isNearest != value)
            {
                isNearest = value;
                DrawOutline();
            }
        }
    }
    
    private SpriteRenderer spriteRenderer;
    private int spriteNum = 0;
    // -- ����

    private void DrawOutline()
    {
        ChangeSprite();
        //ChangeAnimController(isNearest);
    }

    private void ChangeSprite()
    {
        if (isNearest)
        {
            spriteRenderer.sprite = outlineSprites[spriteNum];
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    private void ChangeAnimController(bool _isNearest)
    {
        if (isNearest)
        {
            animator.runtimeAnimatorController = outlineAnimController;
        }
        else
        {
            animator.runtimeAnimatorController = defaultAnimController;
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = defaultAnimController;
    }
}
