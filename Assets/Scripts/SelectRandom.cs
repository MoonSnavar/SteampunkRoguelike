using UnityEngine;

public class SelectRandom : MonoBehaviour
{
    public int CountToLeave = 1;

    private void Start()
    {
        CountToLeave += GameManager.instance.LevelNumber;

        while (transform.childCount > CountToLeave)
        {
            Transform childToDestroy = transform.GetChild(Random.Range(0, transform.childCount));
            DestroyImmediate(childToDestroy.gameObject);
        }
    }
}
