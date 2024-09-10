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
    [SerializeField]
    private SpriteRenderer sr;
    private TorchLight torchLight;
    public static Transform playerTransform;
    private PlayerController controller;
    private void Start()
    {
        //resource init
        ResourceData.Init();
        playerTransform = transform;
        controller = GetComponent<PlayerController>();
        torchLight = GetComponentInChildren<TorchLight>();
    }
    private void Update()
    {
        if (isDead) return;
        CheckDarkphobia();
        //check input
        CheckKey();
        CheckMouse();
        controller.Move();
        PlayerDetectedByEnemy();
    }
    public void OnDamage(float _damage)
    {
        Enemy[] near = NearEnemy(1f);
        if (near.Length > 0) controller.KnockBack(near[0].transform.position);
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
    //게임 종료시
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
        controller.CanMove = false;
        Debug.Log($"{gameObject.name} Is Dead.");
    }
    private void StopGame() => scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver);
    //마우스 입력 감지(클릭)
    private void CheckMouse()
    {
        //벌목
        if (Input.GetMouseButton(0) && !closeChecker.NearestObject.IsUnityNull()) { CutDown(); }
        //벌목하지 않고 있는 상태
        else
        {
            //axa animation stop
            controller.CanMove = true;
            ani.SetBool("Axe", isCutDown = false);
        }
        //벌목 종료 순간
        if(Input.GetMouseButtonUp(0) && playerAttack.canAttack) controller.lookMouse = false;
        //공격
        if (Input.GetMouseButtonDown(1) && playerAttack.canAttack && !isCutDown)
        {
            //이팩트가 지속되는 동안 이팩트 바라보게
            controller.lookMouse = true;
            Invoke("DontLookMouse", 0.5f);
            playerAttack.Attack();
        }
    }
    //키 입력 감지
    private void CheckKey()
    {
        //not move
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) ani.SetBool("Run", false);
        //move
        else if (playerAttack.canAttack && !isCutDown) ani.SetBool("Run", true);
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape)) PauseManager.instance.IsPause = !PauseManager.instance.IsPause;
    }
    //이팩트 끝나면 바라보지 않게
    private void DontLookMouse()
    {
        controller.lookMouse = false;
    }
    //근처 적 반환하는 함수
    private Enemy[] NearEnemy(float range) =>
        Physics2D.OverlapCircleAll(transform.position, range)
                  .Where(collider => collider.CompareTag("Monster"))
                  .Select(enemyObj => enemyObj.GetComponent<Enemy>())
                  .ToArray();
    //플레이어가 적에게 감지됨
    private void PlayerDetectedByEnemy() { foreach (var enemy in NearEnemy(10)) enemy.PlayerDetected(); }
    //벌목
    private void CutDown()
    {
        //axa animation play
        ani.SetBool("Axe", isCutDown = true);
        //can't move while cut down
        controller.CanMove = false;
    }
}