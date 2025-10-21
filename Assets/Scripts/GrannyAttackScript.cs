using UnityEngine;

public class GrannyAttackScript : MonoBehaviour
{
    Granny_InputActions _actions;
    GrannyController _controller;

    [Header("Bullet Prefab")]
    public Rigidbody bulletReg;
    public Rigidbody bulletCharged;

    public Transform bulletSpawn;

    private void Awake()
    {
        _actions = new Granny_InputActions();
    }

    private void Start()
    {
        _controller = GetComponent<GrannyController>(); 
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    private void Update()
    {
        if (_actions.Player.Attack.triggered)
        {
            if (_controller.zoomedIn)
                AttackShoot();
            else
                AttackMelee();
        }
    }

    public void AttackMelee()
    {

    }

    public void AttackShoot()
    {
        Rigidbody _shot;
        _shot = Instantiate(bulletReg, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
        _shot.AddForce(bulletSpawn.forward * 500, ForceMode.Force);
    }
}
