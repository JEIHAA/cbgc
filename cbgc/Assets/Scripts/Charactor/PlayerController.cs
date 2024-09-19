using UnityEngine;

public class PlayerController : Controller
{
    public bool lookMouse; // ���콺 ������ �ٶ���� �ϴ��� ����
    private float? forceLookDirection = null; // ������ �ٶ���� �� ���� (null�̸� ���� ���� ����)
    private float forceLookDuration = 0f; // ���� �ü� ���� ���� �ð�
    private float forceLookTimer = 0f; // Ÿ�̸�

    public override void Move()
    {
        // �̵� ���� �Է¹ޱ�
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Velocity = inputDirection * speed + addVelocity;

        // ������ ������ �ٶ󺸰� �ؾ� �ϴ� ���
        if (forceLookDirection.HasValue)
        {
            AdjustFacingDirectionToFixed(forceLookDirection.Value);
            forceLookTimer += Time.deltaTime;
            if (forceLookTimer >= forceLookDuration)
            {
                forceLookDirection = null; // ������ �ð� �� ���� ���� ����
                forceLookTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
        }
        // �Ϲ����� ���콺 ���� ����
        else if (lookMouse)
        {
            AdjustFacingDirectionToMouse();
        }
        // �̵� ���⿡ ���� ������ ����
        else if (Velocity.x != 0)
        {
            AdjustFacingDirectionToMovement();
        }
    }

    private float GetMouseDir()
    {
        //���콺�� �÷��̾� ���� ���ʿ� �ִ��� �����ʿ� �ִ��� ����
        return Camera.main.ScreenToWorldPoint(Input.mousePosition).x - Player.playerTransform.position.x;
    }
    private void AdjustFacingDirectionToMouse()
    {
        // �÷��̾ ���콺�� �ٶ󺸰� ����
        Vector3 mouseDirX = 
        gameObject.transform.localScale = new Vector3(GetMouseDir() > 0 ? -1 : 1, 1, 1);
    }

    private void AdjustFacingDirectionToMovement()
    {
        // �̵� ���⿡ ���� �÷��̾� ������ ����
        gameObject.transform.localScale = new Vector3(Velocity.x < 0 ? -1 : 1, 1, 1);
    }

    private void AdjustFacingDirectionToFixed(float direction)
    {
        // ���� �������� �÷��̾� ������ ����
        gameObject.transform.localScale = new Vector3(direction < 0 ? -1 : 1, 1, 1);
    }

    public void ForceLookInDirectionToMouse(float duration)
    {
        // �÷��̾ ���� �ð� ���� ���콺 ������ �ٶ󺸰� ����
        forceLookDirection = GetMouseDir();
        forceLookDuration = duration;
        forceLookTimer = 0f;
    }
}