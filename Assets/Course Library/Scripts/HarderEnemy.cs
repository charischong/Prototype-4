using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//more difficult enemy, once hit then player move fast n rnadom spawn
public class HarderEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Rigidbody harderEnemyRb;
    private GameObject player;
    // public bool hasPowerup;
    private float powerupStrength = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        harderEnemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }
    private void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag("Player")){

        Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 awayFromEnemy =  (collision.gameObject.transform.position - transform.position);

        // Debug.Log("Collided with" + collision.gameObject.name + "with powerup set to " + hasPowerup);
        playerRigidbody.AddForce(awayFromEnemy * powerupStrength, ForceMode.Impulse);
    }
        
    }
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        harderEnemyRb.AddForce(lookDirection * speed); 
        
        if (transform.position.y < -10){
            Destroy(gameObject);
        }
    }
}
