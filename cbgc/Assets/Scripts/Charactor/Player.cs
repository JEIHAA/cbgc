using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    private float deathTime = 0f, timeLimit = 2f;
    private bool isCutDown = false, isDead = false;
    [SerializeField] 
    private Animator ani;
    [SerializeField] 
    private PlayerAttack playerAttack;
    [SerializeField] 
    private SceneMoveManager scenemanager;
    [SerializeField] 
    private CloseChecker closeChecker;
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
    private void Update()
    {
        if (isDead) return;
        CheckDarkphobia();
        //check input
        CheckKey();
        CheckMouse();
        contorller.Move();
        PlayerDetectedByEnemy();
    }
    public void OnDamage(float _damage)
    {
        Enemy[] near = NearEnemy(1f);
        if (near.Length > 0) contorller.KnockBack(near[0].transform.position);
        torchLight.LeftTime -= (int)_damage;
    }
    private void CheckDarkphobia()
    {
        if (LightData.TorchLeftTime <= 0)
        {
            deathTime += Time.deltaTime;
            if (deathTime > timeLimit) GameOver();
        }
        else deathTime = 0;
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
    private void StopGame() => scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver);
    private void CheckMouse()
    {
        //using axe
        if (Input.GetMouseButton(0) && !closeChecker.NearestObject.IsUnityNull()) { CutDown(); }
        //end axe
        else
        {
            //axa animation stop
            contorller.CanMove = true;
            ani.SetBool("Axe", isCutDown = false);
        }
        if (Input.GetMouseButtonDown(1) && playerAttack.canAttack && !isCutDown) { playerAttack.Attack(); }
    }
    private void CheckKey()
    {
        //not move
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) ani.SetBool("Run", false);
        //move
        else if (playerAttack.canAttack && !isCutDown) ani.SetBool("Run", true);
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape)) PauseManager.instance.IsPause = !PauseManager.instance.IsPause;
    }
    private Enemy[] NearEnemy(float range) =>
        Physics2D.OverlapCircleAll(transform.position, range)
                  .Where(collider => collider.CompareTag("Monster"))
                  .Select(enemyObj => enemyObj.GetComponent<Enemy>())
                  .ToArray();
    private void PlayerDetectedByEnemy() { foreach (var enemy in NearEnemy(10)) enemy.PlayerDetected(); }
    private void CutDown()
    {
        //axa animation play
        ani.SetBool("Axe", isCutDown = true);
        //can't move while cut down
        contorller.CanMove = false;
    }
}