using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSizeColl: MonoBehaviour
{
    private CircleCollider2D coll;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ResizeColl();
    }

    public void ResizeColl()
    {
        if (spriteRenderer.sprite != null)
        {
            float newRadius = CalculateRadius(spriteRenderer.sprite);
            if (Mathf.Abs(coll.radius - newRadius) > Mathf.Epsilon)
            {
                UpdateColliderRadius();
            }
        }
    }

    void UpdateColliderRadius()
    {
        coll.radius = CalculateRadius(spriteRenderer.sprite);
    }

    float CalculateRadius(Sprite sprite)
    {
        // ��������Ʈ�� ��踦 ������ ���� ū �������� ���
        // ��� ũ�� �� �� ū ���� ���������� ���
        float maxBound = Mathf.Max(sprite.bounds.extents.x, sprite.bounds.extents.y);
        return maxBound;
    }
}
