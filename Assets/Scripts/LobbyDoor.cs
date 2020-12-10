using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LobbyDoor : MonoBehaviour
{
    public int LevelNumber = 1;

    public bool isClosed = true;
    public bool isFinalDoor = false;

    private void Start()
    {
        if (!isClosed && isFinalDoor)
            Destroy(gameObject);
        else if (!isClosed)
            GetComponent<TilemapRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isClosed && collision.CompareTag("Player"))
        {
            if (!isFinalDoor)
            {
                PlayerPrefs.SetInt("Level", LevelNumber);
                SceneManager.LoadScene(1);
            }
        }
    }
}
