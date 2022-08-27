using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] private int _pointsForCoinPickup = 100;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && collision.GetType() == typeof(CapsuleCollider2D)) {
            FindObjectOfType<GameSession>().AddToScore(_pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position, 0.5f);
            Destroy(gameObject);
        }
    }
}
