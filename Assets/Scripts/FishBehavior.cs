using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public float maxFullness = 20f; 
    float fullnessVal;

    float needsTime;
    public float needsTimeReset;
    public float needsTimeStep;

    public GameManager myManager;
    public BarManager barManager;
    public LightManager lightManager;

    Vector3 targetPos;
    bool moving;
    
    private SpriteRenderer spriteRenderer;
    public Sprite sleepSprite;
    public Sprite ogSprite;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fullnessVal = maxFullness;
        needsTime = needsTimeReset;
        barManager.SetHunger(fullnessVal, maxFullness);
    }

    void Update()
    {
        needsTime -= needsTimeStep * Time.deltaTime;
        if (needsTime < 0)
        {
            IncrementNeeds();
        }
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 2f * Time.deltaTime);
        }

        if (lightManager.isToggled)
        {
            spriteRenderer.sprite = sleepSprite;
        }
        else
        {
            spriteRenderer.sprite = ogSprite;
        }
    }

    void IncrementNeeds()
    {
        fullnessVal = Mathf.Max(fullnessVal - 1f, 0f);
        needsTime = needsTimeReset;
        barManager.SetHunger(fullnessVal, maxFullness);

        if (fullnessVal <= 10)
        {
            findFood();
        }

        if (fullnessVal <= 5)
        {
            barManager.Damage(20);
            barManager.cheerDown(80);
        }
        else
        {
            barManager.Heal(40);
            barManager.cheerUp(40);
        }

        if (spriteRenderer.sprite == sleepSprite)
        {
            barManager.Heal(25);
        }
    }

    void findFood()
    {
        float dist = float.MaxValue;
        GameObject closestFood = null;
        foreach (GameObject food in myManager.allFoods)
        {
            float d = Vector3.Distance(transform.position, food.transform.position);
            if (d < dist)
            {
                dist = d;
                closestFood = food;
            }
        }

        if (closestFood != null)
        {
            targetPos = closestFood.transform.position;
            moving = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            myManager.DestroyFood(collision.gameObject);
            fullnessVal = Mathf.Min(fullnessVal + 4f, maxFullness);
            moving = false;
            barManager.SetHunger(fullnessVal, maxFullness);
            barManager.Heal(40);
        }
    }
}