using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float runSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D playerRigidbody;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();

        if (playerRigidbody == null) {
            Debug.LogError("player Rigidbody2D is null.");
        }
    }

    void Update() {
        Run();
        FlipSprite();
    }

    private void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    private void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;
    }

    private void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
    }
}
