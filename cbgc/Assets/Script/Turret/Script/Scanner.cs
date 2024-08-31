using UnityEngine;
public class Scanner : MonoBehaviour
{
    public static Transform ScanNearestTarget<T>(Transform _center, float _range)
    {
        Transform nearTarget = _center;
        float distance = _range;
        RaycastHit2D[] scannedTarget = Physics2D.CircleCastAll(_center.position, _range, Vector2.zero);
        if (scannedTarget.Length == 0) return null;
        foreach (var target in scannedTarget)
        {
            if (target.transform.TryGetComponent<T>(out T tmp) && Vector3.Distance(target.transform.position, _center.position) < distance)
                nearTarget = target.transform;
        }
        return nearTarget;
    }
}