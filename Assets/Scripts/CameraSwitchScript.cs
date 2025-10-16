using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitchScript : MonoBehaviour
{
    GrannyController gCtrl;

    // Establish Cameras
    public CinemachineCamera shootCam;
    public CinemachineCamera platformerCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        gCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gCtrl.zoomedIn)
        {
            platformerCam.Priority = 0;
            shootCam.Priority = 1;
        }
        else
        {
            platformerCam.Priority = 1;
            shootCam.Priority = 0;
        }
    }
}
