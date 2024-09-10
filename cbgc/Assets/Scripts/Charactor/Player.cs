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
    //���� �����
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
    //���콺 �Է� ����(Ŭ��)
    private void CheckMouse()
    {
        //����
        if (Input.GetMouseButton(0) && !closeChecker.NearestObject.IsUnityNull()) { CutDown(); }
        //�������� �ʰ� �ִ� ����
        else
        {
            //axa animation stop
            controller.CanMove = true;
            ani.SetBool("Axe", isCutDown = false);
        }
        //���� ���� ����
        if(Input.GetMouseButtonUp(0) && playerAttack.canAttack) controller.lookMouse = false;
        //����
        if (Input.GetMouseButtonDown(1) && playerAttack.canAttack && !isCutDown)
        {
            //����Ʈ�� ���ӵǴ� ���� ����Ʈ �ٶ󺸰�
            controller.lookMouse = true;
            Invoke("DontLookMouse", 0.5f);
            playerAttack.Attack();
        }
    }
    //Ű �Է� ����
    private void CheckKey()
    {
        //not move
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) ani.SetBool("Run", false);
        //move
        else if (playerAttack.canAttack && !isCutDown) ani.SetBool("Run", true);
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape)) PauseManager.instance.IsPause = !PauseManager.instance.IsPause;
    }
    //����Ʈ ������ �ٶ��� �ʰ�
    private void DontLookMouse()
    {
        controller.lookMouse = false;
    }
    //��ó �� ��ȯ�ϴ� �Լ�
    private Enemy[] NearEnemy(float range) =>
        Physics2D.OverlapCircleAll(transform.position, range)
                  .Where(collider => collider.CompareTag("Monster"))
                  .Select(enemyObj => enemyObj.GetComponent<Enemy>())
                  .ToArray();
    //�÷��̾ ������ ������
    private void PlayerDetectedByEnemy() { foreach (var enemy in NearEnemy(10)) enemy.PlayerDetected(); }
    //����
    private void CutDown()
    {
        //axa animation play
        ani.SetBool("Axe", isCutDown = true);
        //can't move while cut down
        controller.CanMove = false;
    }
}