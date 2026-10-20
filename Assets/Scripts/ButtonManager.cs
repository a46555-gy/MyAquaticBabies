using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ButtonType { Shake, Clean }

public class ButtonManager : MonoBehaviour
{
    public ButtonType buttonType;
    
    public Sprite ogSprite;
    public Sprite newSprite;

    public ShakeManager shaker;

    public GameObject cursorImage;
    public bool hideCursor = true;
    
    public float time = 0.5f;
    public Vector3 pressedScaleModifier = new Vector3(1.2f, 1.2f, 1f);
    public float lerpSpeed = 10f;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Collider2D myCollider;
    private Vector3 originalScale;
    private Coroutine buttonPressCoroutine;
    public bool isCleaning;
    
    public static bool IsCleaning { get; private set; } 
    public static Vector2 FingerWorldPosition { get; private set; }  

    public Transform fingerTip;

    void Start()
    {
        IsCleaning = false;
        
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        originalScale = transform.localScale;

        if (spriteRenderer != null && ogSprite != null)
        {
            spriteRenderer.sprite = ogSprite;
        }

        if (cursorImage != null)
        {
            cursorImage.SetActive(false);
        }
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            DetectClick();

        switch (buttonType)
        {
            case ButtonType.Shake:
                ResetShakeSprite();
                break;
            case ButtonType.Clean:
                FollowCursor();
                break;
        }
    }

    private void DetectClick()
    {
        Vector2 screenPos = Pointer.current.position.ReadValue();
        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(screenPos));

        if (rayHit.collider != myCollider) return;

        switch (buttonType)
        {
            case ButtonType.Shake:
                PressShake();
                break;
            case ButtonType.Clean:
                ToggleClean();
                break;
        }
    }

    private void PressShake()
    {
        if (buttonPressCoroutine != null)
            StopCoroutine(buttonPressCoroutine);

        buttonPressCoroutine = StartCoroutine(ButtonPressSequence());

        if (shaker != null)
            shaker.start = true;
    }

    private void ResetShakeSprite()
    {
        if (shaker == null || spriteRenderer == null || ogSprite == null) return;

        if (!shaker.handActive && buttonPressCoroutine == null && spriteRenderer.sprite != ogSprite)
        {
            spriteRenderer.sprite = ogSprite;
        }
    }

    private IEnumerator ButtonPressSequence()  //lerping buttons
    {
        if (spriteRenderer != null && newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }

        Vector3 targetScale = Vector3.Scale(originalScale, pressedScaleModifier);

        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
            yield return null;
        }
        transform.localScale = targetScale;

        yield return new WaitForSeconds(time);

        while (Vector3.Distance(transform.localScale, originalScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * lerpSpeed);
            yield return null;
        }
        transform.localScale = originalScale;

        if (spriteRenderer != null && ogSprite != null)
        {
            spriteRenderer.sprite = ogSprite;
        }

        buttonPressCoroutine = null;
    }

    private void ToggleClean()
    {
        isCleaning = !isCleaning;
        IsCleaning = isCleaning;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isCleaning ? newSprite : ogSprite;
        }

        if (cursorImage != null)
        {
            cursorImage.SetActive(isCleaning);
            if (isCleaning)
            {
                FollowCursor();
            } 
        }

        if (hideCursor)
        {
             Cursor.visible = !isCleaning;
        }
           
    }

    private void FollowCursor()
    {
        if (!isCleaning || cursorImage == null || Pointer.current == null) return;

        Vector2 screenPos = Pointer.current.position.ReadValue();
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(screenPos);
        mouseWorld.z = 0f;

        
        Vector3 offset = fingerTip != null ? fingerTip.position - cursorImage.transform.position : Vector3.zero;
        offset.z = 0f;
        cursorImage.transform.position = mouseWorld - offset;
        
        FingerWorldPosition = fingerTip != null ? (Vector2)fingerTip.position : (Vector2)mouseWorld;
    }

    void OnDisable()
    {
      
        if (buttonType == ButtonType.Clean)
            Cursor.visible = true;
    }
}