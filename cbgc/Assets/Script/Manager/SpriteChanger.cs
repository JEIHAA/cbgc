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

    [SerializeField] private bool canInteraction = false;
    public bool CanInteraction { get => canInteraction; set => canInteraction = value; }
    [SerializeField] private bool isNearest = false;
    public bool IsNearest
    {
        get => isNearest;
        set
        {
            // 값이 변경될 때만 로직 실행
            if (isNearest != value)
            {
                Debug.Log($"Changing from {isNearest} to {value}");
                isNearest = value;
                Debug.Log($"New value: {isNearest}");
                DrawOutline();
            }
        }
    }
    // -- 변수

    private SpriteRenderer spriteRenderer;
    private int spriteNum = 0;

    private void DrawOutline()
    {
        ChangeSprite();
        //ChangeAnimController(isNearest);
    }

    private void ChangeSprite()
    {
        if (isNearest)
        {
            Debug.Log("outlineSprites");
            spriteRenderer.sprite = outlineSprites[spriteNum];
        }
        else
        {
            Debug.Log("defaultSprite");
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

    public void Setdefault()
    {
        Debug.Log("false");
        isNearest = false;
        canInteraction = false;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = defaultAnimController;
    }
}
