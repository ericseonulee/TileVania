using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && collision.GetType() == typeof(CapsuleCollider2D)) {
            Destroy(gameObject);
            FindObjectOfType<GameSession>().AddScore();
        }
    }
}
