using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class GrannyAttackScript : MonoBehaviour
{
    Granny_InputActions _actions;
    GrannyController _controller;
    [SerializeField] Animator _anim;

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

    [Header("Melee")]
    public GameObject hitSphere;
    public float meleeTime = .5f;
    public float comboTimer = .75f;
    public int comboID = 1;
    public bool canMelee;

    private void Awake()
    {
        _actions = new Granny_InputActions();
        _anim = GetComponentInChildren<Animator>();   
    }

    private void Start()
    {
        _controller = GetComponent<GrannyController>();
        hitSphere.SetActive(false);
        canMelee = true;
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
            else if (canMelee)
                StartCoroutine(AttackMelee());
        }

        if (_actions.Player.Attack.IsPressed())
            isCharging = true;
        else isCharging = false;

        if (isCharging) chargeGauge += Time.deltaTime;
        comboTimer -= Time.deltaTime;
        if (comboTimer < 0)
        {
            comboTimer = 0;
            comboID = 1;
        }

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

    IEnumerator AttackMelee()
    {
        if (comboID == 1)
        {
            comboID = 2;
            _anim.SetTrigger("Swing1");
            comboTimer = 1;
        }
        else if (comboID == 2 && comboTimer > 0)
        {
            comboID = 3;
            _anim.SetTrigger("Swing2");
            comboTimer = 1;
        }

        else if (comboID == 3 && comboTimer > 0)
        {
            comboID = 1;
            _anim.SetTrigger("Swing3");
        }

        canMelee = false;
        hitSphere.SetActive(true);
        yield return new WaitForSeconds(meleeTime);
        canMelee = true;
        hitSphere.SetActive(false);
    }

    public void AttackShoot()
    {
        Rigidbody _shot;
        _shot = Instantiate(bulletReg, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
        _shot.AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Force);
    }
}
