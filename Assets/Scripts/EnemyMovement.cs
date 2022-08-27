using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour {
    public enum EnemyState {
        Idling,
        Moving
    }
    private EnemyState _enemyState = EnemyState.Idling;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private int _enemyLives = 10;
    private bool _isEnemyOnGround;
    private bool _isStateEnded = true;
    private bool _isFlickerEnabled = false;
    private Vector3 _currentScale;
    private Rigidbody2D _enemyRigidBody2D;
    private Animator _enemyAnimator;
    private SpriteRenderer _enemySpriteRenderer;

    void Start() {
        _currentScale = transform.localScale;
        if (gameObject.tag == "BossGoobie") {
            _moveSpeed = 7f;
            _enemyLives = 10;
        }

        _enemyRigidBody2D = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<Animator>();
        _enemySpriteRenderer = GetComponent<SpriteRenderer>();

        if (_enemyRigidBody2D == null) {
            Debug.LogError("Enemy RigidBody 2D is null.");
        }

        if (_enemyAnimator == null) {
            Debug.LogError("Enemy Animator is null.");
        }

        if (_enemySpriteRenderer == null) {
            Debug.LogError("Enemy Sprite Renderer is null.");
        }
    }

    void Update() {
        if (gameObject.tag == "BossGoobie") {
            InvokeBossMoveStates();
            BossGoobieMove();
        }
        else {
            EnemyMove();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Ground") {
            _moveSpeed = -_moveSpeed;
            EnemyFlipSprite();
        }
    }

    private void EnemyFlipSprite() {
        _currentScale = new Vector2(-(_currentScale.x), _currentScale.y);
        transform.localScale = _currentScale;
    }

    private void EnemyMove() {
        _enemyRigidBody2D.velocity = new Vector2(_moveSpeed, 0);
    }

    private bool isEnemyDead(int damage) {
        _enemyLives -= damage;
        if (_enemyLives == 0) { return true; }
        return false;
    }

    private void disableFlicker() {
        _isFlickerEnabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Arrow") {
            _isFlickerEnabled = true;
            StartCoroutine(hitFlickerRoutine());
            Invoke("disableFlicker", 0.5f);
            Destroy(other.gameObject);
            if (isEnemyDead(Arrow.arrowDamage())) {
                Destroy(gameObject);
            }
        }
    }

    private void BossGoobieMove() {
        switch (_enemyState) {
            case EnemyState.Idling:
                _enemyRigidBody2D.velocity = Vector2.zero;
                break;
            case EnemyState.Moving:
                EnemyMove();
                break;
        }
    }

    private void InvokeBossMoveStates() {
        if (_isStateEnded) {
            switch (_enemyState) {
                case EnemyState.Idling:
                    RandomState();
                    Invoke("EndState", Random.Range(1f, 2f));
                    break;
                case EnemyState.Moving:
                    RandomState();
                    Invoke("EndState", Random.Range(2f, 3f));
                    break;
            }
        }
    }

    private void RandomState() {
        int percentChance = Random.Range(0, 3);

        _isStateEnded = true;

        switch (_enemyState) {
            case EnemyState.Idling:
                if (percentChance == 0) {
                    _enemyAnimator.SetBool("isIdling", false);
                    _enemyAnimator.SetBool("isMoving", true);
                    _enemyState = EnemyState.Moving;
                }
                else {
                    _enemyState = EnemyState.Idling;
                }
                _isStateEnded = false;
                break;
            case EnemyState.Moving:
                if (percentChance > 0) {
                    _enemyAnimator.SetBool("isMoving", false);
                    _enemyAnimator.SetBool("isIdling", true);
                    _enemyState = EnemyState.Idling;
                }
                else {
                    _enemyState = EnemyState.Moving;
                }
                _isStateEnded = false;
                break;
        }
    }

    private void EndState() {
        _isStateEnded = !_isStateEnded;
    }

    IEnumerator hitFlickerRoutine() {
        while (_isFlickerEnabled) {
            _enemySpriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _enemySpriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
