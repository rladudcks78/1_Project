using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    Vector2 inputVec;

    [SerializeField]
    float speed = 5.0f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);


        if (inputVec.x != 0)
        {
            sr.flipX = inputVec.x < 0;
        }
    }
}
