using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float jumpForce = 2.5f;
    private float lastBoostTime;
    public float boostCooldown = 5.0f;
    public float boostDuration = 3.0f;

    private Rigidbody playerRb;
    private float xBound = 8.0f;
    private float yBound = 32.0f;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ConstrainPlayerPosition();
    }

    //Moves player based on directional input
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        playerRb.AddForce(Vector3.right * horizontalInput * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //Speed Boost
        if (Input.GetKeyDown(KeyCode.LeftShift) && (Time.time - lastBoostTime > boostCooldown))
        {
            speed *= 1.5f;
            lastBoostTime = Time.time;
            StartCoroutine(BoostDurationCoroutine());
        }

        // display the remaining cooldown time and the remaining boost time
        float remainingCooldown = Mathf.Max(0f, boostCooldown - (Time.time - lastBoostTime));
        Debug.Log("Boost cooldown remaining: " + remainingCooldown + " seconds.");

        if (Time.time - lastBoostTime < boostDuration)
        {
            float remainingBoostTime = boostDuration - (Time.time - lastBoostTime);
            Debug.Log("Boost remaining time: " + remainingBoostTime + " seconds.");
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
    IEnumerator BoostDurationCoroutine()
    {
        yield return new WaitForSeconds(boostDuration); // wait for the duration of the speed boost
        speed /= 2f; // reset the movement speed to its original value
    }
}

