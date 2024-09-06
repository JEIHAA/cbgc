using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject fastMonsterPrefeb, eyeMonsterPrefeb, treePrefeb;
    public int initialPoolSize = 10;
    private ObjectPool<GameObject> objectPool;

    void Start() => objectPool = new ObjectPool<GameObject>(
        CreatePooledObject,
        OnTakeFromPool,
        OnReturnToPool,
        OnDestroyPoolObject,
        false,
        initialPoolSize,
        initialPoolSize
    );
    private GameObject CreatePooledObject() => Instantiate(fastMonsterPrefeb);
    private void OnTakeFromPool(GameObject obj) => obj.SetActive(true);
    private void OnReturnToPool(GameObject obj) => obj.SetActive(false);
    private void OnDestroyPoolObject(GameObject obj) => Destroy(obj);
    public GameObject GetObject() => objectPool.Get();
    public void ReturnObject(GameObject obj) => objectPool.Release(obj);
}
