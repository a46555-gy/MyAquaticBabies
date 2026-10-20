using UnityEngine;
using System.Collections;
using System.Reflection;

public class ShakeManager : MonoBehaviour
{
    public float duration = 1f;

    public AnimationCurve curve;
    
    public bool start = false;

    public GameObject hands;

    public bool handActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hands != null)
        {
            hands.SetActive(false);
            handActive = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (hands != null)
            {
                hands.SetActive(true);
                handActive = true;
            }
            start = false;
            StartCoroutine(Shaking());
        }
        
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        
        transform.position = startPosition;
        if (hands != null)
        {
            hands.SetActive(false);
            handActive = false;
        }
    }
}
