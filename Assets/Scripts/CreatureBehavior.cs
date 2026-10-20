using UnityEngine;

public abstract class CreatureBehavior : MonoBehaviour
{
    public enum Behavior
    {
        Idle,
        Sleeping,
        SeekingFood,
        Scared,
        Dead
    }
    
    [SerializeField] private Behavior state = Behavior.Idle;
    public Behavior State => state;
    public bool asleep => state == Behavior.Sleeping;

    public float maxFullness = 20f;
    public float hungryThreshold = 10f;
    public float starvingThreshold = 5f;
    public float foodValue = 4f;
    private float fullnessVal;

    public float needsTimeReset;
    public float needsTimeStep;
    private float needsTime;
    
    public float moveSpeed = 2f;
    public float wanderSpeed = 1f;
    public float wanderInterval = 2f;

    public Vector2 areaCenter = Vector2.zero;
    public Vector2 areaSize = new Vector2(8f, 4f);

    
    public GameManager myManager;
    public BarManager barManager;
    public LightManager lightManager;
    public ShakeManager shakeManager;

    public Sprite ogSprite;
    public Sprite hungrySprite;
    public Sprite sleepSprite;
    public Sprite scaredSprite;
    public Sprite deadSprite;

    protected SpriteRenderer spriteRenderer;

    private GameObject targetFood;
    private Vector3 wanderTarget;
    private float wanderTimer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fullnessVal = maxFullness;
        needsTime = needsTimeReset;
        barManager.SetHunger(fullnessVal, maxFullness);

        EnterState(state);
    }

    protected virtual void Update()
    {
        if (state == Behavior.Dead)
        {
            return;
        }

        if (barManager.hp <= 0f)
        {
            Die();
            return;
        }

        TickNeeds();
        CheckGlobalTransitions();
        UpdateState();
    }

    protected virtual Vector3 NextPosition(Vector3 target, float speed)
    {
        return Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    protected virtual void OnMoved(Vector3 delta)
    {
    }

    protected virtual void OnStateEntered(Behavior newState)
    {
    }

    void ChangeState(Behavior newState)
    {
        if (state == Behavior.Dead)
        {
            return;
        }

        if (newState == state)
        {
            return;
        }

        ExitState(state);
        state = newState;
        EnterState(newState);
    }

    void EnterState(Behavior newState)
    {
        switch (newState)
        {
            case Behavior.Idle:
                SetSprite(ogSprite);
                PickWanderTarget();
                break;

            case Behavior.SeekingFood:
                
                break;

            case Behavior.Sleeping:
                SetSprite(sleepSprite);
                break;

            case Behavior.Scared:
                if (scaredSprite != null)
                {
                    SetSprite(scaredSprite);
                }
                else
                {
                    SetSprite(ogSprite);
                }
                break;

            case Behavior.Dead:
                SetSprite(deadSprite);
                break;
        }

        OnStateEntered(newState);
    }

    void ExitState(Behavior oldState)
    {
        if (oldState == Behavior.SeekingFood)
        {
            targetFood = null;
        }
    }

    void CheckGlobalTransitions()
    {
        if (shakeManager.handActive)
        {
            ChangeState(Behavior.Scared);
        }
        else if (lightManager.isToggled)
        {
            ChangeState(Behavior.Sleeping);
        }
        else if (state == Behavior.Scared || state == Behavior.Sleeping)
        {
            ChangeState(Behavior.Idle);
        }
    }

    void UpdateState()
    {
        switch (state)
        {
            case Behavior.Idle:
                UpdateIdle();
                break;

            case Behavior.SeekingFood:
                UpdateSeekingFood();
                break;

            case Behavior.Scared:
                barManager.happy = 0;
                break;
        }
    }

    void UpdateIdle()
    {
        if (fullnessVal <= hungryThreshold)
        {
            if (TryStartSeekingFood())
            {
                return;
            }
        }

        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            PickWanderTarget();
        }

        MoveToward(wanderTarget, wanderSpeed);
    }

    void PickWanderTarget()
    {
        wanderTimer = wanderInterval;
        wanderTarget = new Vector3(Random.Range(areaCenter.x - areaSize.x / 2f, areaCenter.x + areaSize.x / 2f),
            Random.Range(areaCenter.y - areaSize.y / 2f, areaCenter.y + areaSize.y / 2f), transform.position.z);
    }

    void UpdateSeekingFood()
    {
        if (targetFood == null || !targetFood.activeInHierarchy || !IsInsideArea(targetFood.transform.position))
        {
            ChangeState(Behavior.Idle);
            return;
        }

        MoveToward(targetFood.transform.position, moveSpeed);
    }

    void MoveToward(Vector3 target, float speed)
    {
        Vector3 before = transform.position;
        transform.position = NextPosition(target, speed);
        OnMoved(transform.position - before);
    }

    bool TryStartSeekingFood()
    {
        GameObject closest = FindClosestFood();

        if (closest == null)
        {
            return false;
        }

        targetFood = closest;
        ChangeState(Behavior.SeekingFood);
        return true;
    }

    GameObject FindClosestFood()
    {
        float dist = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject food in myManager.allFoods)
        {
            if (food == null || !food.activeInHierarchy || !IsInsideArea(food.transform.position))
            {
                continue;
            }

            float d = Vector3.Distance(transform.position, food.transform.position);

            if (d < dist)
            {
                dist = d;
                closest = food;
            }
        }

        return closest;
    }

    bool IsInsideArea(Vector3 pos)
    {
        return Mathf.Abs(pos.x - areaCenter.x) <= areaSize.x / 2f &&
               Mathf.Abs(pos.y - areaCenter.y) <= areaSize.y / 2f;
    }

    void TickNeeds()
    {
        needsTime -= needsTimeStep * Time.deltaTime;

        if (needsTime < 0f)
        {
            IncrementNeeds();
        }
    }

    void IncrementNeeds()
    {
        if (state != Behavior.Sleeping)
        {
            fullnessVal = Mathf.Max(fullnessVal - 1f, 0f);
        }
        needsTime = needsTimeReset;
        barManager.SetHunger(fullnessVal, maxFullness);

        if (fullnessVal <= starvingThreshold)
        {
            barManager.Damage(20);
            barManager.cheerDown(80);
        }
        else
        {
            barManager.Heal(40);
            barManager.cheerUp(40);
        }

        if (state == Behavior.Sleeping)
        {
            barManager.Heal(25);
        }
        if (fullnessVal <= hungryThreshold && state == Behavior.Idle)
        {
            SetSprite(hungrySprite);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == Behavior.Dead)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Food"))
        {
            Eat(collision.gameObject);
        }
    }

    void Eat(GameObject food)
    {
        if (!food.activeInHierarchy)
        {
            return;
        }

        food.SetActive(false);
        myManager.DestroyFood(food);
        fullnessVal = Mathf.Min(fullnessVal + foodValue, maxFullness);
        barManager.SetHunger(fullnessVal, maxFullness);
        barManager.Heal(40);

        if (state == Behavior.SeekingFood)
        {
            ChangeState(Behavior.Idle);
        }
    }

    public void Die()
    {
        ChangeState(Behavior.Dead);
    }

    void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}