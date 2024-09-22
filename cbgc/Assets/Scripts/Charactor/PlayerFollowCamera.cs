using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraRangeWidth = 270f;    // ī�޶� �̵��� �� �ִ� ���� ����
    [SerializeField]
    private float cameraRangeHeight = 140f;   // ī�޶� �̵��� �� �ִ� ���� ����
    private const float screenWidth = 40f, screenHeight = 24f; // ��ũ�� ������
    private const float DefaultCameraZ = -10f;  // ī�޶��� Z ��ġ ��, �⺻�� ����
    private void Update()
    {
        // Ư�� Ű�� ���� ���� �� ī�޶� ((0, 0),ķ�� ���̾�)�� �̵���ŵ�ϴ�.
        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.E))
        {
            transform.position = Vector3.forward * DefaultCameraZ;
        }
        else
        {
            // �÷��̾��� ��ġ�� ������� ī�޶��� ��ġ�� Ŭ�����մϴ�. �� �ʸӸ� ī�޶� ������ �ʰ�.
            Vector3 clampedPosition = GetClampedCameraPosition(Player.playerTransform.position);
            transform.position = clampedPosition;
        }
    }

    private Vector3 GetClampedCameraPosition(Vector3 playerPosition)
    {
        // �÷��̾� ��ġ�� ������� X, Y ��ǥ�� Ŭ�����Ͽ� ī�޶��� ��ġ�� ����մϴ�.
        float clampedX = Mathf.Clamp(
            playerPosition.x,
            -((cameraRangeWidth - screenWidth) / 2),
            (cameraRangeWidth - screenWidth) / 2
        );
        float clampedY = Mathf.Clamp(
            playerPosition.y,
            -((cameraRangeHeight - screenHeight) / 2),
            (cameraRangeHeight - screenHeight) / 2
        );
        return new Vector3(clampedX, clampedY, DefaultCameraZ);
    }
}
