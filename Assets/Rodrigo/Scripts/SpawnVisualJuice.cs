using System.Collections;
using UnityEngine;

public class SpawnVisualJuice : MonoBehaviour
{

    [Header("Scale")]
    [SerializeField] private float startLocalScaleMultiplier = 0.25f;
    [SerializeField] private float overShootScaleMultiplier = 1.25f; 
    [SerializeField] private float retractScaleMultiplier = 0.75f;

    [Header("Duration")]
    [SerializeField] private float overShootDuration = 0.25f;
    [SerializeField] private float retractDuration = 0.15f;
    [SerializeField] private float settleDuration = 0.08f;

    [Header("Curves")]
    [SerializeField] private AnimationCurve expandCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve retractCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve settleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Vector3 _initialLocalScale;

    private void Awake()
    {
        _initialLocalScale = transform.localScale;
    }

    private void Start()
    {
        PlaySpawnTween();
    }

    public void PlaySpawnTween()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = _initialLocalScale * startLocalScaleMultiplier;
        LeanTween.scale(gameObject, _initialLocalScale * overShootScaleMultiplier, overShootDuration).setEase(expandCurve).setOnComplete(PlayRetractTween);
    }

    public void PlayRetractTween()
    {
        LeanTween.scale(gameObject, _initialLocalScale * retractScaleMultiplier, retractDuration).setEase(retractCurve).setOnComplete(PlaySettleTween);
    }

    public void PlaySettleTween()
    {
        LeanTween.scale(gameObject, _initialLocalScale, settleDuration).setEase(settleCurve).setOnComplete(SetFinalScale);
    }

    public void SetFinalScale()
    {
        transform.localScale = _initialLocalScale;
    }

    #region Interpolatio

    IEnumerator OnSpawnAnimation()
    {
        Vector3 startScale = _initialLocalScale * startLocalScaleMultiplier;
        Vector3 overShootScale = _initialLocalScale * overShootScaleMultiplier;
        Vector3 retractScale = _initialLocalScale * retractScaleMultiplier;

        transform.localScale = startScale;

        yield return InterpolateScale(startScale, overShootScale, overShootDuration, expandCurve);
        yield return InterpolateScale(overShootScale, retractScale, retractDuration, retractCurve);
        yield return InterpolateScale(retractScale, _initialLocalScale, settleDuration, settleCurve);
    }

    IEnumerator InterpolateScale(Vector3 startScale, Vector3 endScale, float duration, AnimationCurve curve)
    {
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / duration;
            progress = Mathf.Clamp01(progress);

            float curveProgress = curve.Evaluate(progress);

            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, curveProgress);

            yield return null;
        }        

        transform.localScale = endScale;
    }
    #endregion
}