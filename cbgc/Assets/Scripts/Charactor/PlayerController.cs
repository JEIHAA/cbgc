using UnityEngine;

public class PlayerController : Controller
{
    public bool lookMouse; // 마우스 방향을 바라봐야 하는지 여부
    private float? forceLookDirection = null; // 강제로 바라봐야 할 방향 (null이면 강제 방향 없음)
    private float forceLookDuration = 0f; // 강제 시선 방향 지속 시간
    private float forceLookTimer = 0f; // 타이머

    public override void Move()
    {
        // 이동 방향 입력받기
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Velocity = inputDirection * speed + addVelocity;

        // 강제로 방향을 바라보게 해야 하는 경우
        if (forceLookDirection.HasValue)
        {
            AdjustFacingDirectionToFixed(forceLookDirection.Value);
            forceLookTimer += Time.deltaTime;
            if (forceLookTimer >= forceLookDuration)
            {
                forceLookDirection = null; // 지정된 시간 후 강제 방향 해제
                forceLookTimer = 0f; // 타이머 초기화
            }
        }
        // 일반적인 마우스 방향 조정
        else if (lookMouse)
        {
            AdjustFacingDirectionToMouse();
        }
        // 이동 방향에 따라 스케일 조정
        else if (Velocity.x != 0)
        {
            AdjustFacingDirectionToMovement();
        }
    }

    private float GetMouseDir()
    {
        //마우스가 플레이어 보다 왼쪽에 있는지 오른쪽에 있는지 리턴
        return Camera.main.ScreenToWorldPoint(Input.mousePosition).x - Player.playerTransform.position.x;
    }
    private void AdjustFacingDirectionToMouse()
    {
        // 플레이어가 마우스를 바라보게 조정
        Vector3 mouseDirX = 
        gameObject.transform.localScale = new Vector3(GetMouseDir() > 0 ? -1 : 1, 1, 1);
    }

    private void AdjustFacingDirectionToMovement()
    {
        // 이동 방향에 따라 플레이어 스케일 조정
        gameObject.transform.localScale = new Vector3(Velocity.x < 0 ? -1 : 1, 1, 1);
    }

    private void AdjustFacingDirectionToFixed(float direction)
    {
        // 강제 방향으로 플레이어 스케일 조정
        gameObject.transform.localScale = new Vector3(direction < 0 ? -1 : 1, 1, 1);
    }

    public void ForceLookInDirectionToMouse(float duration)
    {
        // 플레이어가 일정 시간 동안 마우스 방향을 바라보게 설정
        forceLookDirection = GetMouseDir();
        forceLookDuration = duration;
        forceLookTimer = 0f;
    }
}