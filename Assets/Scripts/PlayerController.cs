using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

    private float speed = 3.0f;
    private float jumpForce = 2.5f;
    private bool doubleJumpAvailable = true;
    private float secondJumpForce = 1.5f;
    public bool isOnGround = true;
    private float lastBoostTime;
    public float boostCooldown = 5.0f;
    public float boostDuration = 3.0f;

    private Rigidbody playerRb;
    private float xBound = 10.0f;
    private float yBound = 32.0f;

    public bool hasPowerup = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ConstrainPlayerPosition();
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
    }

    IEnumerator BoostDurationCoroutine()
    {
        yield return new WaitForSeconds(boostDuration); // wait for the duration of the speed boost
        speed /= 1.5f; // reset the movement speed to its original value
    }

    //Moves player based on directional input
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        playerRb.AddForce(Vector3.right * horizontalInput * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                doubleJumpAvailable = true;
            }
            else if (doubleJumpAvailable)
            {
                playerRb.AddForce(Vector3.up * secondJumpForce, ForceMode.Impulse);
                doubleJumpAvailable = false;
            }
        }

        //Speed Boost
        if (Input.GetKeyDown(KeyCode.LeftShift) && (Time.time - lastBoostTime > boostCooldown))
        {
            speed *= 1.5f;
            lastBoostTime = Time.time;
            StartCoroutine(BoostDurationCoroutine());
        }
    }

    //Prevents player from leaving the screen (for future development purposes)
    void ConstrainPlayerPosition()
    {
        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }

        if (transform.position.y > yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("OutOfBounds"))
        {
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            Debug.Log(hasPowerup);
        }

    }
}
