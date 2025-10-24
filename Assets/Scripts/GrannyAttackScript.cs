using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrannyAttackScript : MonoBehaviour
{
    Granny_InputActions _actions;
    GrannyController _controller;

    public float bulletSpeed = 1500f;

    [Header("Bullet Prefab")]
    public Rigidbody bulletReg;
    public Rigidbody bulletCharged;

    public Transform bulletSpawn;

    [Header("Aim")]
    public LayerMask aimColliderMask = new LayerMask();
    public Transform aimDebug;
    public MultiAimConstraint _bodyAim;

    [Header("Charging")]
    public float chargeTime = 1.5f;
    public float chargeGauge;
    [SerializeField] private bool isCharging;

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

        if (_actions.Player.Attack.IsPressed())
            isCharging = true;
        else isCharging = false;

        if (isCharging) chargeGauge += Time.deltaTime;

        if (_actions.Player.Attack.WasReleasedThisFrame())
        {
            if (chargeGauge >= chargeTime)
            {
                Rigidbody _shot;
                _shot = Instantiate(bulletCharged, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
                _shot.AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Force);
                chargeGauge = 0;
            }
            chargeGauge = 0;
        }

        if (_controller.zoomedIn) _bodyAim.weight = 1;
        else _bodyAim.weight = 0;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit, 999f, aimColliderMask))
        {
            aimDebug.position = rayCastHit.point;   
            bulletSpawn.LookAt(rayCastHit.point);
        }
    }

    public void AttackMelee()
    {

    }

    public void AttackShoot()
    {
        Rigidbody _shot;
        _shot = Instantiate(bulletReg, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
        _shot.AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Force);
    }
}
