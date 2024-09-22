using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraRangeWidth = 270f;    // 카메라가 이동할 수 있는 가로 범위
    [SerializeField]
    private float cameraRangeHeight = 140f;   // 카메라가 이동할 수 있는 세로 범위
    private const float screenWidth = 40f, screenHeight = 24f; // 스크린 사이즈
    private const float DefaultCameraZ = -10f;  // 카메라의 Z 위치 값, 기본값 설정
    private void Update()
    {
        // 특정 키가 눌려 있을 때 카메라를 ((0, 0),캠프 파이어)로 이동시킵니다.
        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.E))
        {
            transform.position = Vector3.forward * DefaultCameraZ;
        }
        else
        {
            // 플레이어의 위치를 기반으로 카메라의 위치를 클램프합니다. 벽 너머를 카메라가 비추지 않게.
            Vector3 clampedPosition = GetClampedCameraPosition(Player.playerTransform.position);
            transform.position = clampedPosition;
        }
    }

    private Vector3 GetClampedCameraPosition(Vector3 playerPosition)
    {
        // 플레이어 위치를 기반으로 X, Y 좌표를 클램프하여 카메라의 위치를 계산합니다.
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
