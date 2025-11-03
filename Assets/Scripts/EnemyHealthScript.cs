using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyHealthScript : MonoBehaviour
{
    public int enemyHealth = 5;

    public SkinnedMeshRenderer _render;
    [SerializeField] Animator _anim;
    public Material baseMat;
    public Material hitFlashMat;

    void Start()
    {
        //_render = GetComponent<SkinnedMeshRenderer>();
        baseMat = _render.material;
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HitSphere") StartCoroutine(TakeDamage(1));
        if (other.gameObject.tag == "RegularBullet")
        {
            StartCoroutine(TakeDamage(2));
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "ChargedBullet") StartCoroutine(TakeDamage(20));
    }

    IEnumerator TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth > 0)
        {
            transform.DOPunchPosition(Vector3.forward, .2f, 15);
            _anim.SetTrigger("Damage");
            _render.material = hitFlashMat;
            yield return new WaitForSeconds(.02f);
            HitStopManager.Instance.DoHitStop(.15f);
            yield return new WaitForSeconds(.15f);
            _render.material = baseMat;
        }
        else Destroy(gameObject);
    }
}
