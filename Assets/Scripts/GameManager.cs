using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float goalPosition = 8.0f;
    public Transform player;

    private string message = "Contratulations you've won";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if player is on designated position, congratulate them.
        if (player.position.x >= goalPosition)
        {
            Debug.Log(message);
        }
    }
}
