using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    private PlayerMovement _player;
    private Rigidbody2D _arrowRigidbody;
    [SerializeField] private float _arrowSpeed = 20f;
    private float _playerHorizontalSpeed;
    private static int _arrowDamage = 1;
    public static int arrowDamage() {
        return _arrowDamage;
    }

    void Start() {
        _arrowRigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();

        if (_arrowRigidbody == null) {
            Debug.LogError("Arrow Rigidbody 2D is null.");
        }

        if (_player == null) {
            Debug.LogError("Can not find PlayerMovement.");
        }

        _playerHorizontalSpeed = _player.transform.localScale.x;
        FlipSprite();
        Destroy(gameObject, 2.5f);

    }

    void Update() {
            _arrowRigidbody.velocity = new Vector2(_playerHorizontalSpeed * _arrowSpeed, 0); ;
        }

    private void FlipSprite() {
        transform.localScale = new Vector2(Mathf.Sign(_playerHorizontalSpeed), 1f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
