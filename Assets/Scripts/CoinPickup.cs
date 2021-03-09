using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int coinPoints = 100;
    int collisionCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionCount++;
        if(collisionCount<=1)
        {
            FindObjectOfType<GameSession>().AddToScore(coinPoints);
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }

    }
}
