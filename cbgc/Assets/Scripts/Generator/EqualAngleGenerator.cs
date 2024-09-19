using UnityEngine;

public class EqualAngleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] regions; // 생성할 나무 지역
    [SerializeField]
    private float radius = 5f; // 배열의 반지름
    private void Start() => SpawnArray();
    private void SpawnArray()
    {
        float offset = Random.Range(0, 360);
        for (int i = 0; i < regions.Length; i++)
        {
            float angle = offset +  i * Mathf.PI * 2 / regions.Length; // 각도 계산
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0 ) * radius; // 오브젝트 생성할 위치 계산.
            Instantiate(regions[i], position, Quaternion.identity); // 해당 위치에 오브젝트 생성
        }
    }
}
