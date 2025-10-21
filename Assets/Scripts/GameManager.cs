using UnityEngine;

public class GameManager : MonoBehaviour
{
    GrannyController grannyController;
    public GameObject aimPanel;

    private void Awake()
    {
        grannyController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grannyController.zoomedIn)
            aimPanel.SetActive(true);
        else
            aimPanel.SetActive(false);
    }
}
