using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject Canvas;
    public int LevelNumber;
    public Transform Camera;

    private void Awake()
    {
        instance = this;
    }
}
