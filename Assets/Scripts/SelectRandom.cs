using UnityEngine;

public class SelectRandom : MonoBehaviour
{
    public bool isEnemy;
    public int CountToLeave = 1;

    private void Start()
    {
        if (isEnemy)
            CountToLeave += GameManager.instance.LevelNumber;

        if (CountToLeave > transform.childCount)
            CountToLeave = transform.childCount;

        while (transform.childCount > CountToLeave)
        {
            Transform childToDestroy = transform.GetChild(Random.Range(0, transform.childCount));
            DestroyImmediate(childToDestroy.gameObject);
        }
    }
}
