using System.Collections;
using TMPro;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private int health;
    [SerializeField] private int defense = 0;
    public int Defense { get => defense; set => defense = value; }
    [SerializeField]
    private float attackRange, attackDelay;
    private float deathTime = 0f, timeLimit = 2f;
    private bool canAttack = true, isCutDown = false, isDead = false;
    
    [SerializeField] Animator ani;
    [SerializeField] GameObject attackObject;
    [SerializeField] private SceneMoveManager scenemanager;
    public static Transform playerTransform;
    private PlayerContorller contorller;

    public void OnDamage(float _damage) { GameOver(); }
    private void Start()
    {
        //resource init
        ResourceData.Init();
        playerTransform = transform;

        contorller = GetComponent<PlayerContorller>();
        attackObject.transform.localScale = Vector3.one * attackRange;
        canAttack = true;
    }
    public void GameOver()
    {
        if (isDead) return;
        isDead = true;
        ani.SetBool("Axe", false);
        //death animation
        ani?.SetTrigger("Dead");
        //until animation end
        Invoke("StopGame", 1.4f);
        //player can not move
        contorller.canMove = false;
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
        //check input
        CheckKey();
        CheckMouse();
    }
    
    void CheckMouse()
    {
        //using axe
        if (Input.GetMouseButton(0)) { CutDown(); }
        //end axe
        else
        {
            //axa animation stop
            contorller.canMove = true;
            isCutDown = false;
            ani.SetBool("Axe", false);
        }
        if (Input.GetMouseButtonDown(1) && canAttack && !isCutDown) { Debug.Log(isCutDown); canAttack = false; StartCoroutine(Attack()); }
        
    }
    void CheckKey()
    {
        //dir
        var dir = Input.GetAxis("Horizontal");
        if(dir != 0 && canAttack && !isCutDown) ani.gameObject.transform.localScale = new Vector3(dir < 0 ? -1 : 1, 1, 1);

        //attack
        if (Input.GetKeyDown(KeyCode.Z) && canAttack && !isCutDown) { canAttack = false; StartCoroutine(Attack()); }
        
        //using axe
        if (Input.GetKey(KeyCode.X)) { CutDown(); }
        //end using
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
        contorller.canMove = false;
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