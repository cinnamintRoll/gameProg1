using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakable"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }
}
