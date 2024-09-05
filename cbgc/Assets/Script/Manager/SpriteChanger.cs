using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    // 변수 --
    [SerializeField] private Animator animator;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] outlineSprites;
    [SerializeField] private RuntimeAnimatorController defaultAnimController; // 첫 번째 애니메이터 컨트롤러
    [SerializeField] private RuntimeAnimatorController outlineAnimController; // 두 번째 애니메이터 컨트롤러

    [SerializeField] private bool isNearest = false;
    public bool IsNearest
    {
        get => isNearest;
        set
        {
            // 값이 변경될 때만 로직 실행
            if (isNearest != value)
            {
                isNearest = value;
                DrawOutline();
            }
        }
    }
    
    private SpriteRenderer spriteRenderer;
    private int spriteNum = 0;
    // -- 변수

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
