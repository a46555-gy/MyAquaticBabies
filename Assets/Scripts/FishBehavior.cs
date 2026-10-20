using UnityEngine;

public class FishBehavior : CreatureBehavior
{
    public bool spriteFacesRight = true;

    protected override void OnMoved(Vector3 delta)
    {
        if (Mathf.Abs(delta.x) < 0.0001f)
        {
            return;
        }

        if (spriteFacesRight)
        {
            spriteRenderer.flipX = delta.x < 0f;
        }
        else
        {
            spriteRenderer.flipX = delta.x > 0f;
        }
    }
}