using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    public float hitStopDuration;
    private float _pendingStopDuration;
    public bool isFrozen;

    void Start()
    {
        isFrozen = false;
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (_pendingStopDuration != 0 && !isFrozen) StartCoroutine(HitStopTimer());
    }

    public void DoHitStop(float duration)
    {
        hitStopDuration = duration;
        _pendingStopDuration = hitStopDuration;
    }

    IEnumerator HitStopTimer()
    {
        isFrozen = true;
        var _original = Time.timeScale;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = _original;
        _pendingStopDuration = 0;
        isFrozen = false;
    }
}
