using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    private Rigidbody2D _enemyRigidBody2D;
    void Start()
    {
        _enemyRigidBody2D = GetComponent<Rigidbody2D>();

        if (_enemyRigidBody2D == null) {
            Debug.LogError("Enemy RigidBody 2D is null.");
        }
    }

    void Update()
    {
        _enemyRigidBody2D.velocity = new Vector2(_moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _moveSpeed = -_moveSpeed;
        EnemyFlipSprite();
    }

    private void EnemyFlipSprite() {
        transform.localScale = new Vector2(-Mathf.Sign(_enemyRigidBody2D.velocity.x), 1f);
    }
}
