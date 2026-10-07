using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightManager : MonoBehaviour
{
    public Sprite ogSprite;
    public Sprite newSprite;
    
    public Color ogBgColor = Color.teal;
    public Color newBgColor = Color.black;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    public bool isToggled = false;
    private Collider2D myCollider;
    
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
    
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = ogBgColor;
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
            if (mainCamera != null)
            {
                mainCamera.backgroundColor = newBgColor;
            }
        }
        else
        {
            if (spriteRenderer != null && ogSprite != null)
            {
                spriteRenderer.sprite = ogSprite;
            }
            if (mainCamera != null)
            {
                mainCamera.backgroundColor = ogBgColor;
            }
        }
    }

   
}