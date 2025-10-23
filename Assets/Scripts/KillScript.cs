using UnityEngine;

public class KillScript : MonoBehaviour
{
    public float killTime = 3f;

    private void Start()
    {
        Destroy(gameObject, killTime);
    }
}
