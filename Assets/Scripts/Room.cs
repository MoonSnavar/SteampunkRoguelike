using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isClosed = false;
    public GameObject DoorUp;
    public GameObject DoorDown;
    public GameObject DoorLeft;
    public GameObject DoorRight;
    public GameObject Enemies;

    private bool doorStateSaved = false;
    private bool DU = false;
    private bool DD = false;
    private bool DL = false;
    private bool DR = false;

    public void RotateRandomly()
    {
        int count = Random.Range(0, 2);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 180, 0);

            GameObject tmp = DoorDown;
            DoorDown = DoorUp;
            DoorUp = tmp;
            tmp = DoorRight;
            DoorRight = DoorLeft;
            DoorLeft = tmp;
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
            DoorUp.SetActive(true);
            DoorDown.SetActive(true);
            DoorLeft.SetActive(true);
            DoorRight.SetActive(true);            
        }
    }
    public void OpenRoom()
    {
        if (isClosed && Enemies.transform.childCount == 1)
        {
            DoorUp.SetActive(DU);
            DoorDown.SetActive(DD);
            DoorLeft.SetActive(DL);
            DoorRight.SetActive(DR);
            isClosed = false;
            //сундук открыть            
        }
    }
    private void SaveDoorLastState()
    {
        if (!doorStateSaved)
        {
            DU = DoorUp.activeSelf;
            DD = DoorDown.activeSelf;
            DL = DoorLeft.activeSelf;
            DR = DoorRight.activeSelf;

            doorStateSaved = true;
        }
    }    
}
