using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    public static List<MovingTarget> respawnableMovingTargets = new List<MovingTarget>();

    internal bool isActive = false;

    private SpriteRenderer sr;

    public GameObject mainBody;
    public LineRenderer line;
    public ParticleSystem particleTrail;

    private float particleTrailBaseRate = 10;

    private int scoreSubtractionOnEscape = 3;

    private float creationTime = -Mathf.Infinity;
    private bool hasBeenVisible = false;

    private Vector2 direction;
    private float movementSpeed;
    private float startDelay = Mathf.Infinity;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        isActive = false;
        mainBody.SetActive(false);
        SetParticleTrailEmissionSpeed(0);

        line.gameObject.SetActive(false);
        //line.material.SetTextureScale("_MainTex", new Vector2(0.4f,0.4f));
    }

    private void Update()
    {
        if (isActive)
        {
            transform.position += (Vector3)(direction.normalized * movementSpeed * Time.deltaTime);

            if (sr.IsVisibleFrom(Camera.main))
            {
                if (!hasBeenVisible) hasBeenVisible = true;
            }
            else
            {
                if (hasBeenVisible && isActive)
                {
                    DestroySelf();
                    GameManager.instance.AddPoints(-scoreSubtractionOnEscape);
                }
            }
        }
    }
    public void Init(Vector2 startPoint, Vector2 endPoint, float delay, float speed)
    {
        StopAllCoroutines();

        if (respawnableMovingTargets.Contains(this)) respawnableMovingTargets.Remove(this);

        creationTime = Time.time;
        startDelay = delay;

        line.gameObject.SetActive(true);
        line.SetPositions(new Vector3[2] { startPoint, endPoint });

        hasBeenVisible = false;
        movementSpeed = speed;
        transform.position = startPoint;
        direction = (endPoint - startPoint).normalized;
        transform.eulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(direction, Vector2.up));

        StartCoroutine(InitEndSequence());
    }

    private IEnumerator InitEndSequence()
    {
        yield return new WaitForSeconds(startDelay);

        isActive = true;
        mainBody.SetActive(true);
        SetParticleTrailEmissionSpeed(movementSpeed * particleTrailBaseRate);

        yield return null;
    }
    public void DestroySelf()
    {
        StartCoroutine(DestroySelfSequence());
    }
    private IEnumerator DestroySelfSequence()
    {
        isActive = false;
        mainBody.SetActive(false);
        SetParticleTrailEmissionSpeed(0);

        float lineFadeoutDuration = 1.5f;
        float timer = 0;

        Color defaultLineColor = line.startColor;
        Color fadeoutFinalColor = new Color(0.5f, 0.5f, 0.5f, 0);
        //Color fadeoutFinalColor = new Color(defaultLineColor.r,defaultLineColor.g, defaultLineColor.b, 0); ;

        while (timer < lineFadeoutDuration)
        {
            line.startColor = Color.Lerp(defaultLineColor, fadeoutFinalColor, timer / lineFadeoutDuration);
            line.endColor = Color.Lerp(defaultLineColor, fadeoutFinalColor, timer / lineFadeoutDuration);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        line.gameObject.SetActive(false);
        line.startColor = defaultLineColor;
        line.endColor = defaultLineColor;

        float duration = 5f;
        yield return new WaitForSeconds(duration);

        respawnableMovingTargets.Add(this);

        yield return null;
    }
    private void SetParticleTrailEmissionSpeed(float rate)
    {
        var emission = particleTrail.emission;
        emission.rateOverTime = rate;
    }
}
