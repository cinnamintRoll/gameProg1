using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float speed = 5.0f;
    public float distance = 5.0f;
    public float leftThreshold = -0.5f;
    public float rightThreshold = 8.7f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f; // Add a variable for projectile speed
    public float shootInterval = 3.0f;
    private bool movingRight = true;
    private float startX;
    private float lastShotTime;
    private GameManager gameManager;

    void Start()
    {
        startX = transform.position.x;
        lastShotTime = Time.time;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Set the direction of movement based on the movingRight variable
        Vector3 direction = movingRight ? Vector3.right : Vector3.left;
        // Move the enemy in the current direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Check if the enemy has reached a threshold position
        if (transform.position.x >= rightThreshold)
        {
            movingRight = false;
        }
        else if (transform.position.x <= leftThreshold)
        {
            movingRight = true;
        }

        // Check if the enemy has moved beyond the distance limit
        if (Mathf.Abs(transform.position.x - startX) > distance)
        {
            // Reverse the direction of movement
            movingRight = !movingRight;
            // Reset the start position to the current position
            startX = transform.position.x;
        }

        // Check if it's time to shoot again
        if (Time.time > lastShotTime + shootInterval)
        {
            // Shoot the projectile
            GameObject projectile = Instantiate(projectilePrefab, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = transform.right * projectileSpeed; // Set the speed of the projectile

            // Reset the timer
            lastShotTime = Time.time;
        }
    }

    // Called when the projectile collides with another object
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the player object
            Destroy(collision.gameObject);
            gameManager.GameOver();
        }
    }
}