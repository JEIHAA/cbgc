using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    public int speed = 10;
    [SerializeField]
    private float attackRange, attackDelay;
    private float deathTime = 0f, timeLimit = 2f;
    private bool canAttack = true, isCutDown = false, isDead = false;
    Rigidbody2D rigid;
    [SerializeField] Animator ani;
    [SerializeField] GameObject attackObject;
    [SerializeField] private SceneMoveManager scenemanager;
    public static Transform playerTransform;
    
    Vector2 Velocity
    {
        set
        {
            rigid.velocity = value;
            ani?.SetBool("Run", value.magnitude > 0.125f);
            if (value.x != 0 && canAttack && !isCutDown) ani.gameObject.transform.localScale = new Vector3(value.x < 0 ? -1 : 1, 1, 1);
        }
    }
    public void OnDamage(float _damage) { GameOver(); }
    private void Start()
    {
        //resource init
        ResourceData.Init();
        playerTransform = transform;
        rigid = GetComponent<Rigidbody2D>();
        attackObject.transform.localScale = Vector3.one * attackRange;
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
    void StopGame() => scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver);
    
    private void CheckDarkphobia()
    {
        if (LightData.TorchLeftTime <= 0)
        {
            deathTime += Time.deltaTime;
            if (deathTime > timeLimit) GameOver();
        }
        else
        {
            deathTime = 0;
        }
    }
    void Update()
    {
        if (isDead) return;
        CheckDarkphobia();
        Move();
        //check input
        CheckKey();
        CheckMouse();
    }
    void Move() => Velocity = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized * speed;
    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(1) && canAttack && !isCutDown) { canAttack = false; StartCoroutine(Attack()); }
        //using axe
        if (Input.GetMouseButton(0)) { CutDown(); }
        //end axe
        else
        {
            //axa animation stop
            isCutDown = false;
            ani.SetBool("Axe", false);
        }
    }
    void CheckKey()
    {
        //attack
        if (Input.GetKeyDown(KeyCode.Z) && canAttack && !isCutDown) { canAttack = false; StartCoroutine(Attack()); }
        //using axe
        if (Input.GetKey(KeyCode.X)){ CutDown(); }
        //end axe
        else
        {
            //axa animation stop
            isCutDown = false;
            ani.SetBool("Axe", false);
        }
    }
    void CutDown()
    {
        isCutDown = true;
        //move stop while mouse button down
        Velocity = Vector2.zero;
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