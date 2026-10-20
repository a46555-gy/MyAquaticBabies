using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightManager : MonoBehaviour
{
    public Sprite ogSprite;
    public Sprite newSprite;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    public bool isToggled = false;
    private Collider2D myCollider;
    public GameObject darkBg;
    
    InputAction leftMouse;
    void Start()
    {
        leftMouse = InputSystem.actions.FindAction("MouseClick");
        myCollider =  GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    
        if (spriteRenderer != null && ogSprite != null)
        {
            spriteRenderer.sprite = ogSprite;
        }
        
        if (darkBg != null)
        {
            darkBg.SetActive(false);
        }
        
    }
    
    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            DetectClick();
        }
        
    }

    private void DetectClick()
    {
        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (rayHit.collider == myCollider) toggleLight();
        
    }
    
    
    void toggleLight()
    {
        isToggled = !isToggled; 
    
        if (isToggled)
        {
            if (spriteRenderer != null && newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }
            if (darkBg != null)
            {
                darkBg.SetActive(true);
            }
        }
        else
        {
            if (spriteRenderer != null && ogSprite != null)
            {
                spriteRenderer.sprite = ogSprite;
            }
            if (darkBg != null)
            {
                darkBg.SetActive(false);
            }
            
        }
    }

   
}