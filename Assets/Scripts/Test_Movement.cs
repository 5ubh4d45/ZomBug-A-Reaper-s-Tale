using UnityEngine;


public class Test_Movement : MonoBehaviour
{
    [SerializeField] private Player player;
    public Rigidbody2D rb;

    Vector2 movement;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (player.DialogueUI != null && player.DialogueUI.IsOpen) return;

        //gathaering the inputs for player
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(moveX, moveY);

        movement = new Vector2(moveX, moveY).normalized;


        //movement
        rb.velocity = new Vector2(movement.x * player.MoveSpeed, rb.velocity.y);
    }


}
