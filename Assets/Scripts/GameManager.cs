using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    InputAction leftMouse;

    public GameObject foodObj;
    
    public List<GameObject> allFoods = new List<GameObject>();
    
    public DragDropManager drag;
    
    public RectTransform rectTransform;

   
    void Start()
    {
        leftMouse = InputSystem.actions.FindAction("MouseClick");
    }

    void Update()
    {
        if (rectTransform != null)
        {
            if (drag.endDragged && !drag.atStart)
            {
                Vector2 localPos = rectTransform.anchoredPosition;
                Vector3 worldPos = rectTransform.position;
                makeFood(worldPos);
            }
        }
    }

    void makeFood(Vector3 newPos)
    {
        //Vector3 newPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        newPos.z = 0;
        Instantiate(foodObj, newPos, Quaternion.identity);
        allFoods.Add(Instantiate(foodObj, newPos, Quaternion.identity));

        drag.returnToStart();
    }

    public void DestroyFood(GameObject food)
    {
        allFoods.Remove(food);
        Destroy(food);
    }
}
