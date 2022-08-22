using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;

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

        if (playerAnimator == null) {
            Debug.LogError("Player Animator is null."); 
        }

        if (playerCapsuleCollider == null) {
            Debug.LogError("Player Capsule Collider2D is null.");
        }
    }

    void Update() {
        Run();
    }

    private void FlipSprite() {
        transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
    }

    private bool isPlayerMovingHorizontal() {
        return Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

    }

    private void OnJump(InputValue value) {
        bool isPlayerGrounded = playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isPlayerGrounded) { return;}
        else { 
            playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
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
