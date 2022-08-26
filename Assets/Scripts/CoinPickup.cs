using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {
    [SerializeField] AudioClip coinPickupSFX;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && collision.GetType() == typeof(CapsuleCollider2D)) {
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, 0.5f);
            Destroy(gameObject);
            FindObjectOfType<GameSession>().AddScore();
        }
    }
}
