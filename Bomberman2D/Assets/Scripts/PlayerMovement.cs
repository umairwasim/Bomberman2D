using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [SerializeField] private Rigidbody2D playerRB;

    [SerializeField] private KeyCode moveUp = KeyCode.W;
    [SerializeField] private KeyCode moveDown = KeyCode.S;
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveRight = KeyCode.D;

    [SerializeField] private AnimationSpriteRenderer animationUp;
    [SerializeField] private AnimationSpriteRenderer animationDown;
    [SerializeField] private AnimationSpriteRenderer animationLeft;
    [SerializeField] private AnimationSpriteRenderer animationRight;
    [SerializeField] private AnimationSpriteRenderer animationDeath;

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
        if (GameManager.Instance.gameState != GameState.Playing)
            return;

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

        //set the animation based on direction
        animationUp.enabled = animationSpriteRenderer == animationUp;
        animationDown.enabled = animationSpriteRenderer == animationDown;
        animationLeft.enabled = animationSpriteRenderer == animationLeft;
        animationRight.enabled = animationSpriteRenderer == animationRight;

        activeAnimationSR = animationSpriteRenderer;
        activeAnimationSR.isIdle = currentDirection == Vector2.zero;
    }

    public void PlayDeathAnimation()
    {
        enabled = false;
        animationDeath.enabled = true;
        activeAnimationSR = animationDeath;

        StartCoroutine(PlayerDeathRoutine());
    }

    IEnumerator PlayerDeathRoutine()
    {
        yield return new WaitForSeconds(1.25f);
        gameObject.SetActive(false);
    }
}
