using UnityEngine;

public class EqualAngleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] regions; // ������ ���� ����
    [SerializeField]
    private float radius = 5f; // �迭�� ������
    private void Start() => SpawnArray();
    private void SpawnArray()
    {
        float offset = Random.Range(0, 360);
        for (int i = 0; i < regions.Length; i++)
        {
            float angle = offset +  i * Mathf.PI * 2 / regions.Length; // ���� ���
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0 ) * radius; // ������Ʈ ������ ��ġ ���.
            Instantiate(regions[i], position, Quaternion.identity); // �ش� ��ġ�� ������Ʈ ����
        }
    }
}
