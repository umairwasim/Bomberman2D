using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;

    public float moveSpeed = 5f;

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    public AnimationSpriteRenderer animationUp;
    public AnimationSpriteRenderer animationDown;
    public AnimationSpriteRenderer animationLeft;
    public AnimationSpriteRenderer animationRight;

    private AnimationSpriteRenderer activeAnimationSR;

    private Vector2 currentDirection = Vector2.down;

    private void Awake()
    {
        if (playerRB == null)
            playerRB = GetComponent<Rigidbody2D>();
        activeAnimationSR = animationDown;
    }

    private void Update()
    {
        if (Input.GetKey(moveUp))
            SetDirection(Vector2.up, animationUp);
        else if (Input.GetKey(moveDown))
            SetDirection(Vector2.down, animationDown);
        else if (Input.GetKey(moveLeft))
            SetDirection(Vector2.left, animationLeft);
        else if (Input.GetKey(moveRight))
            SetDirection(Vector2.right, animationRight);
        else
            //if we aren't pressing any key
            SetDirection(Vector2.zero, activeAnimationSR);

    }
    private void FixedUpdate()
    {
        Vector2 currentPosition = playerRB.position;
        Vector2 movePosition = moveSpeed * Time.fixedDeltaTime * currentDirection;
        playerRB.MovePosition(currentPosition + movePosition);
    }

    private void SetDirection(Vector2 newDirection, AnimationSpriteRenderer animationSpriteRenderer)
    {
        currentDirection = newDirection;

        animationUp.enabled = animationSpriteRenderer == animationUp;
        animationDown.enabled = animationSpriteRenderer == animationDown;
        animationLeft.enabled = animationSpriteRenderer == animationLeft;
        animationRight.enabled = animationSpriteRenderer == animationRight;

        activeAnimationSR = animationSpriteRenderer;
        activeAnimationSR.isIdle = currentDirection == Vector2.zero;

        //set the animation based on direction
    }
}
