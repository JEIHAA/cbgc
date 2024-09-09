using System.Collections;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    private float deathTime = 0f, timeLimit = 2f;
    private bool isCutDown = false, isDead = false;
    
    [SerializeField] Animator ani;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] private SceneMoveManager scenemanager;
    private TorchLight torchLight;
    public static Transform playerTransform;
    private Controller contorller;

    private void Start()
    {
        //resource init
        ResourceData.Init();
        playerTransform = transform;
        contorller = GetComponent<Controller>();
        torchLight = GetComponentInChildren<TorchLight>();
    }
    void Update()
    {
        if (isDead) return;
        CheckDarkphobia();
        //check input
        CheckKey();
        CheckMouse();
        contorller.MoveInput();
    }

    public void OnDamage(float _damage)
    {
        torchLight.LeftTime -= (int)_damage;
    }

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
        contorller.CanMove = false;
        Debug.Log($"{gameObject.name} Is Dead.");
    }
    void StopGame() => scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver);

    void CheckMouse()
    {
        //using axe
        //if (Input.GetMouseButtonDown(0)) { StartCoroutine(CheckTree()); }
        if (Input.GetMouseButton(0)) { CutDown(); }
        //end axe
        else
        {
            //axa animation stop
            contorller.CanMove = true;
            isCutDown = false;
            ani.SetBool("Axe", false);
        }
        if (Input.GetMouseButtonDown(1) && playerAttack.canAttack && !isCutDown) { playerAttack.Attack(); }   
    }

    IEnumerator CheckTree()
    {
        isCutDown = true;
        float checkTime = 1.0f, leftTime = checkTime;
        while (isCutDown)
        {
            leftTime -= Time.deltaTime;
            if(leftTime < 0) {
                var hit = Physics2D.BoxCast(playerAttack.transform.position, Vector2.one*3, 0f, Vector2.right, 1f).transform.gameObject;
                if (!hit.IsUnityNull() && hit.CompareTag("InteractiveObject"))
                {
                    Debug.Log(hit.name);
                }
                leftTime = checkTime;
            };
            yield return null;
        }
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
        }
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape)) PauseManager.instance.IsPause = !PauseManager.instance.IsPause;
    }

    void CutDown()
    {
        isCutDown = true;
        contorller.CanMove = false;
        //axa animation play
        ani.SetBool("Axe", true);
    }
}