using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class AlgaePrefab : MonoBehaviour
{
    private Collider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        myCollider.isTrigger = true;  
    }

    void Update()
    {
        if (!ButtonManager.IsCleaning) return;
        if (Pointer.current == null || !Pointer.current.press.wasPressedThisFrame) return;

        if (myCollider.OverlapPoint(ButtonManager.FingerWorldPosition))
        {
            Destroy(gameObject);
        }
    }
    
    
}