using UnityEngine;

public class SelectRandom : MonoBehaviour
{
    public bool isEnemy;
    public int CountToLeave = 1;

    private void Start()
    {
        if (isEnemy)
            CountToLeave += GameManager.instance.LevelNumber;

        while (transform.childCount > CountToLeave)
        {
            Transform childToDestroy = transform.GetChild(Random.Range(0, transform.childCount));
            DestroyImmediate(childToDestroy.gameObject);
        }
    }
}
