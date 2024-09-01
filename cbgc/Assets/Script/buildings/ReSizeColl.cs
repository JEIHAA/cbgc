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
        // 스프라이트의 경계를 가져와 가장 큰 반지름을 계산
        // 경계 크기 중 더 큰 값을 반지름으로 사용
        float maxBound = Mathf.Max(sprite.bounds.extents.x, sprite.bounds.extents.y);
        return maxBound;
    }
}
