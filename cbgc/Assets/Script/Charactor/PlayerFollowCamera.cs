using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField]
    private float camRangeWidth, camRangeheight;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(Player.playerTransform.position.x, -camRangeWidth / 2, camRangeWidth / 2),
            Mathf.Clamp(Player.playerTransform.position.y, -camRangeheight / 2, camRangeheight / 2), -20);
    }
}
