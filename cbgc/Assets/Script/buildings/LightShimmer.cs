using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShimmer : MonoBehaviour
{
    private SpriteRenderer sprite;
    private SpriteMask mask;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
    }

    private void Update()
    {
        SetShimmerSize();
    }

    private void SetShimmerSize()
    {
        mask.sprite = sprite.sprite;
    }

}
