using System.Collections;
using TMPro;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private int health;
    [SerializeField] private int defense = 0;
    public int Defense { get => defense; set => defense = value; }
    
    private float deathTime = 0f, timeLimit = 2f;
    private bool isCutDown = false, isDead = false;
    
    [SerializeField] Animator ani;
    [SerializeField] PlayerAttack playerAttack;
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
        if (Input.GetMouseButtonDown(1) && playerAttack.canAttack && !isCutDown) { playerAttack.Attack(); }
        
    }
    void CheckKey()
    {
        //dir
        var xDir = Input.GetAxis("Horizontal");
        var yDir = Input.GetAxis("Vertical");
        //not move
        if (xDir == 0 && yDir == 0) ani.SetBool("Run", false);
        //move
        else if (playerAttack.canAttack && !isCutDown)
        {
            ani.SetBool("Run", true);
            if(xDir != 0) ani.gameObject.transform.localScale = new Vector3(xDir < 0 ? -1 : 1, 1, 1);
        }
        //attack
        if (Input.GetKeyDown(KeyCode.Z) && playerAttack.canAttack && !isCutDown) { playerAttack.Attack(); }
        
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
    
    IUsable obj;
    private void OnCollisionEnter2D(Collision2D collision) { collision.gameObject.TryGetComponent<IUsable>(out obj); }
    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject.TryGetComponent<IUsable>(out obj)) obj = null; }
}