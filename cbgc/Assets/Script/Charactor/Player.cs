using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField]
    Camera nowCamera;
    [SerializeField]
    Sprite[] dirSprite;
    [SerializeField]
    GameObject playerSprite, attackObject;
    Animator ani;
    [SerializeField]
    SpriteRenderer sr, campFireSR;
    SpriteRenderer campFireCompass, campFireDir;
    Rigidbody2D rigid;
    public static Transform playerTransform;
    public float AttackRange;
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
            ani?.SetBool("Run", value.magnitude > 0.125f);
            if (value.x != 0) playerSprite.transform.localScale = new Vector3(value.x < 0 ? -1 : 1, 1, 1);
        }
    }
    public int speed = 10;
    bool canAttack;
    [SerializeField] 
    private float dirArrowDistance = 15f;
    [SerializeField]
    private TorchLight torchLight;
    private float deathTime = 0f;
    private float timeLimit = 2f;
    public void OnDamage(float _damage) { GameOver(); }
    public void GameOver()
    {
        ani?.SetTrigger("Dead");
        Invoke("StopGame", 1.1f);
        rigid.bodyType = RigidbodyType2D.Static;
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
        ResourceData.Init();
        GetAttachedComponents();
        MakeCompass();
        canAttack = true;
    }
    void GetAttachedComponents()
    {
        playerTransform = transform;
        rigid = GetComponent<Rigidbody2D>();
        sr = playerSprite.GetComponent<SpriteRenderer>();
        ani = playerSprite.GetComponent<Animator>();
        attackObject.transform.localScale = Vector3.one * AttackRange;
    }
    void MakeCompass()
    {
        GameObject tmp = new GameObject("Compass");
        tmp.transform.SetParent(transform);
        campFireCompass = tmp.AddComponent<SpriteRenderer>();
        tmp = new GameObject("Compass_Dir");
        tmp.transform.SetParent(transform);
        campFireDir = tmp.AddComponent<SpriteRenderer>();
    }
    void Update()
    {
        CheckDarkphobia();
        //if (actionBlock) { return; }
        Move();
        CompassSet();
        Click();
    }
    void Move()
    {
        MoveVec = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.up;
        rigid.velocity = MoveVec * speed;
    }
    void CompassSet()
    {
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
            campFireCompass.transform.localPosition = compassPos * dirArrowDistance;
            campFireDir.transform.localPosition = compassPos;
            campFireCompass.sprite = campFireSR.sprite;
        }
        else
        {
            campFireDir.gameObject.SetActive(false);
            campFireCompass.gameObject.SetActive(false);
        }
    }
    void Click()
    {
        var mouseVec = nowCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (Input.GetMouseButtonDown(0) && canAttack) canAttack = false; StartCoroutine(Attack());
        if (Input.GetMouseButton(0)) { 
            rigid.velocity = Vector2.zero;
            ani.SetBool("Axe", true);
        }
        else { ani.SetBool("Axe", false); }
    }
    IEnumerator Attack()
    {
        attackObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
        attackObject.SetActive(false);
    }
    IUsable obj;
    private void OnCollisionEnter2D(Collision2D collision) { collision.gameObject.TryGetComponent<IUsable>(out obj); }
    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject.TryGetComponent<IUsable>(out obj)) obj = null; }
}
