using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private float deathTime = 0f;
    private const float timeLimit = 2f;
    private bool isCutDown = false; // ������ ���� �ִ��� Ȯ��
    private bool isDead = false; // ����ߴ��� Ȯ��

    [SerializeField] private Animator ani; // �ִϸ����� ������Ʈ
    [SerializeField] private PlayerAttack playerAttack; // �÷��̾� ���� ��ũ��Ʈ
    [SerializeField] private SceneMoveManager scenemanager; // ��� �̵� �Ŵ���
    [SerializeField] private CloseChecker closeChecker; // ��ó ��ü üũ��

    private TorchLight torchLight; // ȶ�� ����Ʈ ������Ʈ
    public static Transform playerTransform; // �÷��̾��� ��ȯ
    private PlayerController controller; // �÷��̾� ��Ʈ�ѷ�

    private void Start()
    {
        // �ڿ� �� ������Ʈ �ʱ�ȭ
        ResourceData.Init();
        playerTransform = transform;
        controller = GetComponent<PlayerController>();
        torchLight = GetComponentInChildren<TorchLight>();
    }

    private void Update()
    {
        if (isDead) return; // �÷��̾ �׾��ٸ� �ƹ� �۾��� ���� ����

        // ���� ��Ŀ���� ó��
        CheckDarkphobia();
        HandleInput();
        controller.Move();
        HandleEnemyDetection();
    }

    public void OnDamage(float damage)
    {
        // ��ó �� ã��
        Enemy[] near = FindNearbyEnemies(1f);
        if (near.Length > 0)
        {
            // ���� ��ġ�� �÷��̾ �о��
            controller.KnockBack(near[0].transform.position);
        }
        // ȶ���� ���� �ð� ����
        torchLight.LeftTime -= (int)damage;
    }

    private void CheckDarkphobia()
    {
        // ȶ���� �ð��� �� ���������� Ȯ��
        if (LightData.TorchLeftTime <= 0)
        {
            deathTime += Time.deltaTime;
            if (deathTime > timeLimit)
            {
                TriggerGameOver(); // ���� ���� ó��
            }
        }
        else
        {
            deathTime = 0; // ��� ���� �ð� �ʱ�ȭ
        }
    }

    public void TriggerGameOver()
    {
        if (isDead) return; // �̹� �׾��ٸ� �ƹ� �۾��� ���� ����

        isDead = true;
        ani.SetBool("Axe", false); // ���� �ִϸ��̼� ��Ȱ��ȭ
        ani?.SetTrigger("Dead"); // ���� �ִϸ��̼� Ʈ����
        Invoke("EndGame", 1.4f); // �ִϸ��̼��� ������ ���� ����
        controller.CanMove = false; // �÷��̾� �̵� ��Ȱ��ȭ

        Debug.Log($"{gameObject.name} is dead."); // ����� �޽���
    }

    private void EndGame()
    {
        scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver); // ���� ���� ������� �̵�
    }

    private void HandleInput()
    {
        HandleMouseInput(); // ���콺 �Է� ó��
        HandleKeyInput(); // Ű �Է� ó��
    }

    private void HandleMouseInput()
    {
        // ���� �׼� ó��
        if (Input.GetMouseButton(0) && !closeChecker.NearestObject.IsUnityNull())
        {
            StartCuttingDown();
        }
        else
        {
            StopCuttingDown();
        }

        // ���� �׼� ó��
        if (Input.GetMouseButtonUp(0) && playerAttack.canAttack)
        {
            controller.lookMouse = false; // ���콺 �ü� ó��
        }

        if (Input.GetMouseButtonDown(1) && playerAttack.canAttack && !isCutDown)
        {
            controller.lookMouse = true; // ���� �� ���콺 �ü� ó��
            Invoke("StopLookingMouse", 0.5f); // ���� ����Ʈ ���� �� �ü� ó��
            playerAttack.Attack();
        }
    }

    private void HandleKeyInput()
    {
        // �̵� ���� �� �ִϸ��̼� ����
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            ani.SetBool("Run", false); // �̵����� ���� ��
        }
        else if (playerAttack.canAttack && !isCutDown)
        {
            ani.SetBool("Run", true); // �̵��� ��
        }

        // ���� �Ͻ����� ó��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseManager.instance.IsPause = !PauseManager.instance.IsPause;
        }
    }

    private void StopLookingMouse()
    {
        controller.lookMouse = false; // ���� ����Ʈ ���� �� ���콺 �ü� ó��
    }

    private Enemy[] FindNearbyEnemies(float range)
    {
        // ������ ���� �� ���� ã�� �Լ�
        return Physics2D.OverlapCircleAll(transform.position, range)
                        .Where(collider => collider.CompareTag("Monster"))
                        .Select(collider => collider.GetComponent<Enemy>())
                        .ToArray();
    }

    private void HandleEnemyDetection()
    {
        // ��ó �� ���� ó��
        foreach (var enemy in FindNearbyEnemies(10))
        {
            enemy.PlayerDetected(); // ���� �÷��̾ ����
        }
    }

    private void StartCuttingDown()
    {
        ani.SetBool("Axe", isCutDown = true); // ���� �ִϸ��̼� Ȱ��ȭ
        controller.CanMove = false; // ���� �� �̵� ��Ȱ��ȭ
    }

    private void StopCuttingDown()
    {
        ani.SetBool("Axe", isCutDown = false); // ���� �ִϸ��̼� ��Ȱ��ȭ
        controller.CanMove = true; // ���� ���� �� �̵� Ȱ��ȭ
    }
}
