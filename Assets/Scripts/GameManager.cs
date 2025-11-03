using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GrannyController grannyController;
    GrannyAttackScript grannyAttack;
    public GameObject aimPanel;

    //Charge Meter Setup
    public Image chargeIcon;
    public float chargeMeter;
    public float chargeTimer = 2;

    private void Awake()
    {
        grannyController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyController>();
        grannyAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyAttackScript>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (grannyController.zoomedIn)
            aimPanel.SetActive(true);
        else
            aimPanel.SetActive(false);

        chargeMeter = grannyAttack.chargeGauge - 1f;
        chargeIcon.fillAmount = chargeMeter;

        if (chargeIcon.fillAmount < 1)
        {
            chargeIcon.color = Color.red;
        }
        else
        {
            chargeIcon.color = Color.green;
        }
    }
}
