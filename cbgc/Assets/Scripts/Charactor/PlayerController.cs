using UnityEngine;
public class PlayerController : Controller
{
    [SerializeField]
    SpriteRenderer sr;
    public override void Move()
    {
        //Input
        Velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed + addVelocity;
        //이팩트 방향 결정
        gameObject.transform.localScale = new Vector3((Player.playerTransform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? -1 : 1), 1, 1);
        //스프라이트 방향 결정
        if(Velocity.x != 0)
        {
            if (gameObject.transform.localScale.x < 0) sr.flipX = Velocity.x > 0;
            else sr.flipX = Velocity.x < 0;
        }
    }
}