using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isLobby;
    public int Keys = 0;
    public int LevelNumber;
    public GameObject Canvas;
    public Transform Camera;
    public Transform KeysImages;
    public LobbyDoor[] lobbyDoors;
    public Item[] items;

    private void Awake()
    {
        instance = this;

        Keys = PlayerPrefs.GetInt("Keys");
        LevelNumber = PlayerPrefs.GetInt("Level");
        print("Keys - " + Keys);
        if (isLobby)
        {
            if (Keys == 0)
                lobbyDoors[0].isClosed = false;
            else
            {
                for (int i = 0; i <= Keys; i++)
                {
                    lobbyDoors[i].isClosed = false;
                }
            }
        }        
    }
    private void Start()
    {
        if (isLobby)
        {
            for (int i = 0; i < Keys - 1; i++)
            {
                if (i < 3)
                    KeysImages.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void Back()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void GetSavedItems()
    {
        int savedItems = 1;
        while (savedItems < 4)
        {
            if (PlayerPrefs.HasKey(savedItems.ToString() + "SavedItemId"))
            {
                int id = PlayerPrefs.GetInt(savedItems.ToString() + "SavedItemId");
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].Id == id)
                    {
                        int rarity = PlayerPrefs.GetInt(savedItems.ToString() + "SavedItemRarity");
                        items[i].Rarity = rarity;
                        Inventory.instance.AddNewItem(items[i], false);
                        break;
                    }
                }
            }
            
            savedItems += 1;
        }
    }
}
