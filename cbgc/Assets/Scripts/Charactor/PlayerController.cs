using UnityEngine;
public class PlayerController : Controller
{
    public override void Move()
    {
        Velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed + addVelocity;
        gameObject.transform.localScale = new Vector3((Player.playerTransform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? -1 : 1), 1, 1);
    }
}