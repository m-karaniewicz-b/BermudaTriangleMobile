using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    internal EnemyTypeData enemyType;

    public static List<MovingTarget> respawnableMovingTargets = new List<MovingTarget>();

    internal bool isActive = false;

    [Header("References")]
    public GameObject mainBody;
    public LineRenderer line;
    public ParticleSystem particleTrail;
    private SpriteRenderer sr;

    private const float START_DELAY_DEFAULT = 2f;

    private float creationTime = -Mathf.Infinity;
    private bool hasBeenVisible = false;

    private float startDelay = Mathf.Infinity;
    private Vector2 direction;

    private float movementSpeed = 3;
    private int lifeModOnKill = 0;
    private int lifeModOnEscape = 0;
    private int lifeModOnEyeCollision = 0;
    private int pointModOnKill = 1;
    private int pointModOnEscape = 0;
    private int pointModOnEyeCollision = 0;
    private bool canBeKilled = true;
    private bool collideWithEye = false;
    private bool destroyOnEyeCollision = false;

    //Visuals
    private const float PRTCL_TRAIL_BASE_RATE = 10f;
    private Color color1;
    private Color color2;


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
                    Escape();
                }
            }
        }
    }

    public void Init(Vector2 startPoint, Vector2 endPoint, EnemyTypeData type)
    {
        StopAllCoroutines();

        if (respawnableMovingTargets.Contains(this)) respawnableMovingTargets.Remove(this);

        gameObject.name = type.name;
        creationTime = Time.time;
        startDelay = START_DELAY_DEFAULT;
        hasBeenVisible = false;

        //Pass or apply EnemyTypeData values
        movementSpeed = type.movementSpeed;
        lifeModOnKill = type.lifeModOnKill;
        lifeModOnEscape = type.lifeModOnEscape;
        lifeModOnEyeCollision = type.lifeModOnEyeCollision;
        pointModOnKill = type.pointModOnKill;
        pointModOnEscape = type.pointModOnEscape;
        pointModOnEyeCollision = type.pointModOnEyeCollision;
        canBeKilled = type.canBeKilled;
        destroyOnEyeCollision = type.destroyOnEyeCollision;
        collideWithEye = type.collideWithEye;

        if(type.sprite!=null)sr.sprite = type.sprite;
        color1 = type.color1;
        color2 = type.color2;

        sr.color = color1;

        //Indicator
        line.gameObject.SetActive(type.indicatorOn);
        if(type.indicatorOn)
        {
            line.startColor = color1;
            line.endColor = color1;
            line.SetPositions(new Vector3[2] { startPoint, endPoint });
        }


        //Position and direction
        transform.position = startPoint;
        direction = (endPoint - startPoint).normalized;
        transform.eulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(direction, Vector2.up));

        StartCoroutine(InitFinalSequence());
    }

    private IEnumerator InitFinalSequence()
    {
        yield return new WaitForSeconds(startDelay);

        isActive = true;
        mainBody.SetActive(true);
        SetParticleTrailEmissionSpeed(movementSpeed * PRTCL_TRAIL_BASE_RATE);

        yield return null;
    }

    private void Escape()
    {
        DestroySelf();
        GameManager.instance.SetMoney(GameManager.moneyTotal + pointModOnEscape);
        GameManager.instance.SetLivesCurrent(GameManager.livesCurrent + lifeModOnEscape);
    }

    public void Kill()
    {
        if (!canBeKilled) return;
        DestroySelf();
        GameManager.instance.SetMoney(GameManager.moneyTotal + pointModOnKill);
        GameManager.instance.SetLivesCurrent(GameManager.livesCurrent + lifeModOnKill);
    }

    public void Collide()
    {
        if (!collideWithEye) return;
        if (destroyOnEyeCollision) DestroySelf();
        GameManager.instance.SetMoney(GameManager.moneyTotal + pointModOnEyeCollision);
        GameManager.instance.SetLivesCurrent(GameManager.livesCurrent + lifeModOnEyeCollision);
    }

    private void DestroySelf()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collideWithEye) return;
        //Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");
        
        if(collision.collider.CompareTag(EyeCenter.HITBOX_TAG))
        {
            Collide();
        }
    }
}
