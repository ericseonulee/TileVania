using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    private bool isPlayerClimbing;
    private bool isPlayerOnLadder;
    private bool isPlayerOnGround;
    private bool canJump;
    private float gravityScaleAtStart = 1f;

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerCapsuleCollider;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();

        if (playerRigidbody == null) {
            Debug.LogError("Player Rigidbody2D is null.");
        }
        gravityScaleAtStart = playerRigidbody.gravityScale;


        if (playerAnimator == null) {
            Debug.LogError("Player Animator is null."); 
        }

        if (playerCapsuleCollider == null) {
            Debug.LogError("Player Capsule Collider2D is null.");
        }
    }

    void Update() {
        isPlayerOnLadder = playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        isPlayerOnGround = playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        canJump = isPlayerOnGround || isPlayerClimbing;

        Run();

        if (isPlayerOnLadder && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))) {
            isPlayerClimbing = true;
        }
        ClimbLadder();
    }

    private void ClimbLadder() {
        if (!isPlayerOnLadder) {
            isPlayerClimbing = false;
            playerAnimator.SetBool("isClimbing", false);
            playerRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }

        if (isPlayerClimbing) {
            Vector2 playerVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);
            playerRigidbody.velocity = playerVelocity;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) {
                playerRigidbody.gravityScale = 0;
                playerAnimator.SetBool("isClimbing", true);
            }
            
            if (isPlayerMovingVertical()) {
                if (isPlayerOnGround) {
                    playerAnimator.SetBool("isClimbing", false);
                }// also stop animator when top of ladder.

                playerAnimator.StopPlayback();
            }
            else {
                playerAnimator.StartPlayback();
            }
        }
    }

    private void FlipSprite() {
        transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
    }

    private bool isPlayerMovingHorizontal() {
        return Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

    }

    private bool isPlayerMovingVertical() {
        return Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;
    }

    private void OnJump(InputValue value) {
        if (!canJump) { return;}

        playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
    }

    private void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    private void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;
        
        if (isPlayerMovingHorizontal()) {
            FlipSprite();
            playerAnimator.SetBool("isRunning", true);
        }
        else {
            playerAnimator.SetBool("isRunning", false);
        }
    }
}
