using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject[] inGameObj;
    public GameObject Menu;
    public GameObject EndText;

    private void Start()
    {
        if (GameSettings.GameIsStarted)
            StartGame();
        else
        {
            for (int i = 0; i < inGameObj.Length; i++)
            {
                inGameObj[i].SetActive(false);
            }
        }
    }
    public void StartGame()
    {
        for (int i = 0; i < inGameObj.Length; i++)
        {
            inGameObj[i].SetActive(true);
        }

        Menu.SetActive(false);
        GameSettings.GameIsStarted = true;
    }

    public void Exit()
    {
        Application.Quit();          
    }

    public void TurnEnd(bool enable)
    {
        EndText.SetActive(enable);
    }
}
