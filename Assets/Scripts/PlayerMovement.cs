using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float _climbSpeed = 5f;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _jumpSpeed = 20f;
    [SerializeField] private Vector2 _deathKick = new Vector2 (10f, 20f);
    [SerializeField] private bool _isAlive = true;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Transform _bow;
    private bool _isPlayerClimbing;
    private bool _isPlayerOnLadder;
    private bool _isPlayerOnGround;
    private bool _canJump;
    private float _gravityScaleAtStart = 1f;
    

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        if (playerRigidbody == null) {
            Debug.LogError("Player Rigidbody2D is null.");
        }
        _gravityScaleAtStart = playerRigidbody.gravityScale;


        if (playerAnimator == null) {
            Debug.LogError("Player Animator is null."); 
        }

        if (playerBodyCollider == null) {
            Debug.LogError("Player Capsule Collider2D is null.");
        }

        if (playerFeetCollider == null) {
            Debug.LogError("Player Box Collider2D is null.");
        }
    }

    void Update() {
        if (Input.GetKey(KeyCode.R)) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        if (!_isAlive) { return; };

        _isPlayerOnLadder = playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        _isPlayerOnGround = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        _canJump = _isPlayerOnGround || _isPlayerClimbing;

        Run();

        if (_isPlayerOnLadder && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))) {
            _isPlayerClimbing = true;
        }
        ClimbLadder();
        PlayerDie();
    }

    private void ClimbLadder() {
        if (!_isPlayerOnLadder) {
            _isPlayerClimbing = false;
            playerAnimator.SetBool("isClimbing", false);
            playerRigidbody.gravityScale = _gravityScaleAtStart;
            playerAnimator.StopPlayback();
            return;
        }

        if (_isPlayerClimbing) {
            Vector2 playerVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * _climbSpeed);
            playerRigidbody.velocity = playerVelocity;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) {
                playerRigidbody.gravityScale = 0;
                playerAnimator.SetBool("isClimbing", true);
            }
            
            if (isPlayerMovingVertical()) {
                if (_isPlayerOnGround) {
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

    private void OnFire(InputValue value) {
        if (!_isAlive || _isPlayerClimbing) { return;}

        if (Input.GetKey(KeyCode.A)) {
            playerAnimator.SetTrigger("isShooting");
            Instantiate(_arrow, _bow.position, transform.rotation);
        }
        if (Input.GetKeyUp(KeyCode.A)) {
            playerAnimator.SetTrigger("isNotShooting");
        }
    }

    private void OnJump(InputValue value) {
        if (!_isAlive || !_canJump) { return;}

        if (_isPlayerClimbing && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))) {
            playerAnimator.SetBool("isClimbing", false);
            _isPlayerClimbing = false;
            playerRigidbody.velocity += new Vector2(0f, _jumpSpeed * 0.8f);
            playerRigidbody.gravityScale = _gravityScaleAtStart;
        }
        else {
            playerRigidbody.velocity += new Vector2(0f, _jumpSpeed);
        }
    }

    private void OnMove(InputValue value) {
        if (!_isAlive) { return; };
        moveInput = value.Get<Vector2>();
    }
    private void PlayerDie() {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            _isAlive = false;
            playerAnimator.SetTrigger("playerDeath");
            playerRigidbody.velocity = _deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * _runSpeed, playerRigidbody.velocity.y);
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
