using UnityEngine;

public class OctopusBehavior : CreatureBehavior
{
    public float rollDegreesPerUnit = 360f;
    private float rollDirection = 1f;

    protected override void OnMoved(Vector3 delta)
    {
        if (Mathf.Abs(delta.x) > 0.0001f)
        {
            rollDirection = Mathf.Sign(delta.x);
        }

        transform.Rotate(0f, 0f, -rollDirection * delta.magnitude * rollDegreesPerUnit);
    }

    protected override void OnStateEntered(Behavior newState)
    {
        if (newState == Behavior.Sleeping || newState == Behavior.Scared || newState == Behavior.Dead)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}