using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;

    // hard mode

    public float hangingTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionDist;
    public bool hasPowerIcon;
    private bool smashing = false;
    private float floorY;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
      float forwardInput = Input.GetAxis("Vertical");
      playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
      powerupIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);

      // hard mode
      if (hasPowerIcon == true && Input.GetKeyDown(KeyCode.Space) && !smashing)
      {
        smashing = true;
        StartCoroutine(Smash());
      }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Powerup")){
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
        else if (other.CompareTag("PowerIcon")){
            hasPowerIcon = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }
    IEnumerator PowerupCountdownRoutine () {
        yield return new WaitForSeconds(7);
        hasPowerup = false; 
        hasPowerIcon = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup){

            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer =  (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with" + collision.gameObject.name + "with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
        
    }
    
    IEnumerator Smash(){

        var enemies = FindObjectsOfType<Enemy>();
        var harderEnemies = FindObjectsOfType<HarderEnemy>();
        
        //Store the y position before taking off
        floorY = transform.position.y;
        // Debug.Log(hangingTime);
        // Debug.Log(Time.time);

        //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangingTime;
        // Debug.Log(jumpTime);

        while (Time.time < jumpTime){
        {
        //move the player up while still keeping their x velocity.
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        //Now move the player down
        while (transform.position.y > floorY)
        {
            yield return new WaitForSeconds(1);
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        

            //Cycle through all enemies.
            for (int i = 0; i < enemies.Length; i++)
            {
            //Apply an explosion force that originates from our position.
                if(enemies[i] != null)
                    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                    transform.position, explosionDist, 0.0f, ForceMode.Impulse);
            }

            for (int i = 0; i < harderEnemies.Length; i++)
            {
                if(harderEnemies[i] != null)
                    harderEnemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                    transform.position, explosionDist, 0.0f, ForceMode.Impulse);
            }

        //We are no longer smashing, so set the boolean to false
        smashing = false;
        }
        }

    }
}
