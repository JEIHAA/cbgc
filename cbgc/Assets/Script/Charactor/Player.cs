using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField]
    Camera nowCamera;
    [SerializeField]
    Sprite[] dirSprite;
    [SerializeField]
    GameObject playerSprite, knockbackObject;
    Animator ani;
    [SerializeField]
    SpriteRenderer sr, campFireCompass, campFireSR, campFireDir;
    Rigidbody2D rigid;
    public static Transform playerTransform;
    public float AttackRange;
    [SerializeField]
    private float camRangeWidth, camRangeheight;
    [SerializeField]
    private SceneMoveManager scenemanager;
    private bool actionBlock = true;
    Vector2 moveVec = Vector2.zero;
    Vector2 MoveVec
    {
        get { return moveVec; }
        set
        {
            if (value.magnitude > 1) moveVec = value.normalized;
            else moveVec = value;
            ani.SetBool("Run", value.magnitude > 0.125f);
            if (value.x != 0) playerSprite.transform.localScale = new Vector3(value.x < 0 ? -1 : 1, 1, 1);
        }
    }
    public int speed = 10;
    bool touchable = true;

    public TorchLight torchLight;
    private float deathTime = 0f;
    private float timeLimit = 2f;




    public void OnDamage() { GameOver(); }
    public void GameOver()
    {
        ani?.SetTrigger("Dead");
        ani = null;
        Invoke("StopGame", 1.1f);
        Destroy(rigid);
        Debug.Log($"{gameObject.name} Is Dead.");
    }
    void StopGame()
    {
        scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver);
    }
    IEnumerator BoolChange(float time)
    {
        yield return new WaitForSeconds(time);
        actionBlock = !actionBlock;
    }

    private void CheckDarkphobia()
    {
        if (torchLight.LifeTime <= 0)
        {
            Debug.Log(deathTime);
            deathTime += Time.deltaTime;
            if (deathTime > timeLimit) GameOver();
        }
        else
        {
            deathTime = 0;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        playerTransform = transform;
        rigid = GetComponent<Rigidbody2D>();
        sr = playerSprite.GetComponent<SpriteRenderer>();
        ani = playerSprite.GetComponent<Animator>();
        StartCoroutine(BoolChange(.5f));
    }

    void Update()
    {
        CheckDarkphobia();

        if (actionBlock) { return; }
        MoveVec = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.up;
        rigid.velocity = MoveVec * speed;
        var compassPos = -transform.position;
        if (Mathf.Abs(compassPos.x) > 25 || Mathf.Abs(compassPos.y) > 12)
        {
            campFireDir.gameObject.SetActive(true);
            campFireCompass.gameObject.SetActive(true);
                
            if (Mathf.Abs(compassPos.x) * 10 > Mathf.Abs(compassPos.y) * 19)
                compassPos = compassPos / Mathf.Abs(compassPos.x) * 19;
            else
                compassPos = compassPos / Mathf.Abs(compassPos.y) * 10;

            if (Mathf.Abs(compassPos.x) > 15)
            {
                if (compassPos.x < -15) campFireDir.sprite = dirSprite[2];
                if (compassPos.x > 15) campFireDir.sprite = dirSprite[3];
            }
            else
            {
                if (compassPos.y > 8) campFireDir.sprite = dirSprite[0];
                if (compassPos.y < -8) campFireDir.sprite = dirSprite[1];
            }

            campFireCompass.transform.localPosition = compassPos;
            campFireDir.transform.localPosition = compassPos * 0.8f;
            campFireCompass.sprite = campFireSR.sprite;
        }
        else
        {
            campFireDir.gameObject.SetActive(false);
            campFireCompass.gameObject.SetActive(false);
        }
        nowCamera.transform.position = new Vector3(Mathf.Clamp(transform.position.x, -camRangeWidth / 2, camRangeWidth / 2),
                                                Mathf.Clamp(transform.position.y, -camRangeheight / 2, camRangeheight / 2)) + Vector3.back * 20;
        var mouseVec = nowCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //animation off
        if (Input.GetMouseButton(0))
        {
            rigid.velocity = Vector2.zero;
            if (touchable)
            {
                touchable = false;
                //animation on
                if (!ani.GetBool("Axe"))
                {
                    ani.SetBool("Axe", true);
                    knockbackObject.SetActive(true);
                }
                Invoke("Touchable", 1f);
                Debug.DrawRay(transform.position, mouseVec);
                var layhit = Physics2D.Raycast(transform.position, mouseVec, AttackRange);
                layhit.collider?.gameObject.GetComponent<Enemy>()?.OnDamage();
            }
        }
        if (Input.GetMouseButtonUp(0)) ani.SetBool("Axe", false);
    }
    void Touchable() { touchable = true; knockbackObject.SetActive(false); }
    IUsable obj;
    private void OnCollisionEnter2D(Collision2D collision) { collision.gameObject.TryGetComponent<IUsable>(out obj); }
    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject.TryGetComponent<IUsable>(out obj)) obj = null; }
}
