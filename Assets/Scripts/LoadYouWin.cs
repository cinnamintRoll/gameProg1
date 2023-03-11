using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadYouWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // replace "Player" with the tag of your player object
        {
            SceneManager.LoadScene("YOU WIN");
        }
    }
}
