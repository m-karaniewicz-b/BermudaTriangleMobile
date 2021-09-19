using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("References")]
    public GameObject mainBody;
    public LineRenderer line;
    public ParticleSystem particleTrail;
    private SpriteRenderer sr;

    private const float START_DELAY_DEFAULT = 2f;
    private const float PRTCL_TRAIL_BASE_RATE = 10f;

    private float creationTime;
    internal bool isActive;
    private bool hasBeenVisible;

    private float startDelayDuration;
    private Vector2 direction;

    internal EnemyTypeData enemyType;
    private float movementSpeed;
    private int lifeModOnKill;
    private int lifeModOnEscape;
    private int lifeModOnEyeCollision;
    private int pointModOnKill;
    private int pointModOnEscape;
    private int pointModOnEyeCollision;
    private bool canBeKilled;
    private bool collideWithEye;
    private bool destroyOnEyeCollision;

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

        if (EntityManager.Instance.respawnableMovingTargets.Contains(this)) EntityManager.Instance.respawnableMovingTargets.Remove(this);

        enemyType = type;
        gameObject.name = type.name;
        creationTime = Time.time;
        startDelayDuration = START_DELAY_DEFAULT;
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
        yield return new WaitForSeconds(startDelayDuration);

        isActive = true;
        mainBody.SetActive(true);
        SetParticleTrailEmissionSpeed(movementSpeed * PRTCL_TRAIL_BASE_RATE);

        yield return null;
    }

    private void Escape()
    {
        DestroySelf();
        GameManager.Instance.SetMoney(GameManager.Instance.moneyTotal + pointModOnEscape);
        GameManager.Instance.SetLivesCurrent(GameManager.Instance.livesCurrent + lifeModOnEscape);
    }

    public void Kill()
    {
        if (!canBeKilled) return;
        DestroySelf();
        GameManager.Instance.SetMoney(GameManager.Instance.moneyTotal + pointModOnKill);
        GameManager.Instance.SetLivesCurrent(GameManager.Instance.livesCurrent + lifeModOnKill);
    }

    public void Collide()
    {
        if (!collideWithEye) return;
        if (destroyOnEyeCollision) DestroySelf();
        GameManager.Instance.SetMoney(GameManager.Instance.moneyTotal + pointModOnEyeCollision);
        GameManager.Instance.SetLivesCurrent(GameManager.Instance.livesCurrent + lifeModOnEyeCollision);
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

        EntityManager.Instance.respawnableMovingTargets.Add(this);

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
