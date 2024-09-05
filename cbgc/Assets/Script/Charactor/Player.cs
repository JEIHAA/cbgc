using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float attackRange, attackDelay;
    const float dirArrowDistance = 0.8f;
    private float deathTime = 0f;
    private float timeLimit = 2f;
    private bool canAttack = true;
    private bool isDead = false;
    public int speed = 10;

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
    [SerializeField]
    private SceneMoveManager scenemanager;
    [SerializeField]
    private TorchLight torchLight;
    Vector2 moveVec = Vector2.zero;
    Vector2 MoveVec
    {
        get { return moveVec; }
        set
        {
            if (value.magnitude > 1) moveVec = value.normalized;
            else moveVec = value;
            ani?.SetBool("Run", value.magnitude > 0.125f);
            if (value.x != 0 && canAttack) playerSprite.transform.localScale = new Vector3(value.x < 0 ? -1 : 1, 1, 1);
        }
    }
    public void OnDamage(float _damage) { GameOver(); }
    private void Start()
    {
        //resource init
        ResourceData.Init();
        GetAttachedComponents();
        MakeCompass();
        canAttack = true;
    }
    public void GameOver()
    {
        if(isDead) return;
        isDead = true;
        ani.SetBool("Axe", false);
        //death animation
        ani?.SetTrigger("Dead");
        //until animation end
        Invoke("StopGame", 1.4f);
        //player can not move
        rigid.bodyType = RigidbodyType2D.Static;
        Debug.Log($"{gameObject.name} Is Dead.");
    }
    void StopGame()
    {
        scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver);
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
    void GetAttachedComponents()
    {
        playerTransform = transform;
        rigid = GetComponent<Rigidbody2D>();
        sr = playerSprite.GetComponent<SpriteRenderer>();
        ani = playerSprite.GetComponent<Animator>();
        attackObject.transform.localScale = Vector3.one * attackRange;
    }
    void MakeCompass()
    {
        //make compass
        GameObject tmp = new GameObject("Compass");
        tmp.transform.SetParent(transform);
        campFireCompass = tmp.AddComponent<SpriteRenderer>();
        //make compass dir
        tmp = new GameObject("Compass_Dir");
        tmp.transform.SetParent(transform);
        campFireDir = tmp.AddComponent<SpriteRenderer>();
        //set sorting order
        campFireCompass.sortingOrder = 4;
        campFireDir.sortingOrder = 4;
    }
    void Update()
    {
        if (isDead) return;
        CheckDarkphobia();
        Move();
        CompassSet();
        //check input
        CheckKey();
    }
    void Move()
    {
        MoveVec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rigid.velocity = MoveVec * speed;
    }
    void CompassSet()
    {
        //to campfire dir
        var compassPos = -transform.position;
        //close to campfire
        if (Mathf.Abs(compassPos.x) > 25 || Mathf.Abs(compassPos.y) > 12)
        {
            //obj on
            campFireDir.gameObject.SetActive(true);
            campFireCompass.gameObject.SetActive(true);

            //compass pos set
            if (Mathf.Abs(compassPos.x) * 10 > Mathf.Abs(compassPos.y) * 19)
                compassPos = compassPos / Mathf.Abs(compassPos.x) * 19;
            else
                compassPos = compassPos / Mathf.Abs(compassPos.y) * 10;
            //compass dir pos & sprite set
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
        //too far (off compass)
        else
        {
            campFireDir.gameObject.SetActive(false);
            campFireCompass.gameObject.SetActive(false);
        }
    }
    void CheckKey()
    {
        //attack
        if (Input.GetKeyDown(KeyCode.Z) && canAttack) { canAttack = false; StartCoroutine(Attack()); }
        //using axe
        if (Input.GetKey(KeyCode.X)){ CutDown(); }
        else
        {
            //axa animation stop
            ani.SetBool("Axe", false);
        }
    }
    void CutDown()
    {
        //move stop while mouse button down
        rigid.velocity = Vector2.zero;
        //axa animation play
        ani.SetBool("Axe", true);
    }
    IEnumerator Attack()
    {
        //attack delay
        attackObject.SetActive(true);
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
        attackObject.SetActive(false);
    }
    IUsable obj;
    private void OnCollisionEnter2D(Collision2D collision) { collision.gameObject.TryGetComponent<IUsable>(out obj); }
    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject.TryGetComponent<IUsable>(out obj)) obj = null; }
}