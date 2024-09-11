using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private float deathTime = 0f;
    private const float timeLimit = 2f;
    private bool isCutDown = false; // 나무를 베고 있는지 확인
    private bool isDead = false; // 사망했는지 확인

    [SerializeField] private Animator ani; // 애니메이터 컴포넌트
    [SerializeField] private PlayerAttack playerAttack; // 플레이어 공격 스크립트
    [SerializeField] private SceneMoveManager scenemanager; // 장면 이동 매니저
    [SerializeField] private CloseChecker closeChecker; // 근처 객체 체크기

    private TorchLight torchLight; // 횃불 라이트 컴포넌트
    public static Transform playerTransform; // 플레이어의 변환
    private PlayerController controller; // 플레이어 컨트롤러

    private void Start()
    {
        // 자원 및 컴포넌트 초기화
        ResourceData.Init();
        playerTransform = transform;
        controller = GetComponent<PlayerController>();
        torchLight = GetComponentInChildren<TorchLight>();
    }

    private void Update()
    {
        if (isDead) return; // 플레이어가 죽었다면 아무 작업도 하지 않음

        // 게임 메커니즘 처리
        CheckDarkphobia();
        HandleInput();
        controller.Move();
        HandleEnemyDetection();
    }

    public void OnDamage(float damage)
    {
        // 근처 적 찾기
        Enemy[] near = FindNearbyEnemies(1f);
        if (near.Length > 0)
        {
            // 적의 위치로 플레이어를 밀어내기
            controller.KnockBack(near[0].transform.position);
        }
        // 횃불의 남은 시간 감소
        torchLight.LeftTime -= (int)damage;
    }

    private void CheckDarkphobia()
    {
        // 횃불의 시간이 다 떨어졌는지 확인
        if (LightData.TorchLeftTime <= 0)
        {
            deathTime += Time.deltaTime;
            if (deathTime > timeLimit)
            {
                TriggerGameOver(); // 게임 종료 처리
            }
        }
        else
        {
            deathTime = 0; // 어둠 공포 시간 초기화
        }
    }

    public void TriggerGameOver()
    {
        if (isDead) return; // 이미 죽었다면 아무 작업도 하지 않음

        isDead = true;
        ani.SetBool("Axe", false); // 도끼 애니메이션 비활성화
        ani?.SetTrigger("Dead"); // 죽음 애니메이션 트리거
        Invoke("EndGame", 1.4f); // 애니메이션이 끝나면 게임 종료
        controller.CanMove = false; // 플레이어 이동 비활성화

        Debug.Log($"{gameObject.name} is dead."); // 디버그 메시지
    }

    private void EndGame()
    {
        scenemanager.LoadScene(SceneMoveManager.SceneName.GameOver); // 게임 오버 장면으로 이동
    }

    private void HandleInput()
    {
        HandleMouseInput(); // 마우스 입력 처리
        HandleKeyInput(); // 키 입력 처리
    }

    private void HandleMouseInput()
    {
        // 벌목 액션 처리
        if (Input.GetMouseButton(0) && !closeChecker.NearestObject.IsUnityNull())
        {
            StartCuttingDown();
        }
        else
        {
            StopCuttingDown();
        }

        // 공격 액션 처리
        if (Input.GetMouseButtonUp(0) && playerAttack.canAttack)
        {
            controller.lookMouse = false; // 마우스 시선 처리
        }

        if (Input.GetMouseButtonDown(1) && playerAttack.canAttack && !isCutDown)
        {
            controller.lookMouse = true; // 공격 중 마우스 시선 처리
            Invoke("StopLookingMouse", 0.5f); // 공격 이펙트 종료 후 시선 처리
            playerAttack.Attack();
        }
    }

    private void HandleKeyInput()
    {
        // 이동 중일 때 애니메이션 설정
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            ani.SetBool("Run", false); // 이동하지 않을 때
        }
        else if (playerAttack.canAttack && !isCutDown)
        {
            ani.SetBool("Run", true); // 이동할 때
        }

        // 게임 일시정지 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseManager.instance.IsPause = !PauseManager.instance.IsPause;
        }
    }

    private void StopLookingMouse()
    {
        controller.lookMouse = false; // 공격 이펙트 종료 후 마우스 시선 처리
    }

    private Enemy[] FindNearbyEnemies(float range)
    {
        // 지정된 범위 내 적을 찾는 함수
        return Physics2D.OverlapCircleAll(transform.position, range)
                        .Where(collider => collider.CompareTag("Monster"))
                        .Select(collider => collider.GetComponent<Enemy>())
                        .ToArray();
    }

    private void HandleEnemyDetection()
    {
        // 근처 적 감지 처리
        foreach (var enemy in FindNearbyEnemies(10))
        {
            enemy.PlayerDetected(); // 적이 플레이어를 감지
        }
    }

    private void StartCuttingDown()
    {
        ani.SetBool("Axe", isCutDown = true); // 벌목 애니메이션 활성화
        controller.CanMove = false; // 벌목 중 이동 비활성화
    }

    private void StopCuttingDown()
    {
        ani.SetBool("Axe", isCutDown = false); // 벌목 애니메이션 비활성화
        controller.CanMove = true; // 벌목 종료 후 이동 활성화
    }
}
