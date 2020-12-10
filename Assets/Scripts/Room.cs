using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform chests;
    public bool isSecretRoom = false;
    public bool isLastRoom = false;
    public bool isClosed = false;
    public GameObject DoorExit;
    public GameObject DoorUp;
    public GameObject DoorDown;
    public GameObject DoorLeft;
    public GameObject DoorRight;
    public GameObject Enemies;

    public GameObject SecretDoorUp;
    public GameObject SecretDoorDown;
    public GameObject SecretDoorLeft;
    public GameObject SecretDoorRight;

    private bool doorStateSaved = false;
    private bool DU = false;
    private bool DD = false;
    private bool DL = false;
    private bool DR = false;

    private void Start()
    {
        if (isSecretRoom)
        {
            if (!DoorUp.activeSelf)
                SecretDoorUp.SetActive(true);
            if (!DoorDown.activeSelf)
                SecretDoorDown.SetActive(true);
            if (!DoorLeft.activeSelf)
                SecretDoorLeft.SetActive(true);
            if (!DoorRight.activeSelf)
                SecretDoorRight.SetActive(true);
        }
    }

    public void RotateRandomly()
    {
        int count = Random.Range(0, 2);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 0, 180);

            GameObject tmp = DoorDown;
            DoorDown = DoorUp;
            DoorUp = tmp;
            tmp = DoorRight;
            DoorRight = DoorLeft;
            DoorLeft = tmp;

            for (int z = 0; z < transform.childCount; z++)
            {
                if (transform.GetChild(z).tag != "Walls" && transform.GetChild(z).tag != "Exit")
                    transform.GetChild(z).Rotate(0,0,180);
            }
        }
    }
    public void SetActiveEnemies(bool active)
    {
        Enemies.SetActive(active);
        CloseRoom();
    }
    public void CloseRoom()
    {
        if (isClosed)
        {
            SaveDoorLastState();
            if (DoorUp != null)
                DoorUp.SetActive(true);
            if (DoorDown != null)
                DoorDown.SetActive(true);
            if (DoorLeft != null)
                DoorLeft.SetActive(true);
            if (DoorRight != null)
                DoorRight.SetActive(true);            
        }
    }
    public void OpenRoomAndChest()
    {
        if (gameObject == null)
            return;

        if (Enemies.transform.childCount == 1)
        {
            if (isClosed)
            {
                if (DoorUp != null)
                    DoorUp.SetActive(DU);
                if (DoorDown != null)
                    DoorDown.SetActive(DD);
                if (DoorLeft != null)
                    DoorLeft.SetActive(DL);
                if (DoorRight != null)
                    DoorRight.SetActive(DR);
                isClosed = false;
                
                if (isLastRoom)
                    DoorExit.SetActive(false);
            }
            if (chests.childCount != 0)
            {
                if (isLastRoom)               
                    chests.GetChild(0).gameObject.SetActive(true);

                chests.GetChild(0).GetComponent<Chest>().OpenChest();
            }
        }
    }
    private void SaveDoorLastState()
    {
        if (!doorStateSaved)
        {
            if (DoorUp != null)
                DU = DoorUp.activeSelf;
            if (DoorDown != null)
                DD = DoorDown.activeSelf;
            if (DoorLeft != null)
                DL = DoorLeft.activeSelf;
            if (DoorRight != null)
                DR = DoorRight.activeSelf;

            doorStateSaved = true;
        }
    }    
}
