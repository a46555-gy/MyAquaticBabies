using UnityEngine;

public class CrabBehavior : CreatureBehavior
{
    protected override Vector3 NextPosition(Vector3 target, float speed)
    {
        Vector3 pos = transform.position;
        float step = speed * Time.deltaTime;

        if (Mathf.Abs(target.x - pos.x) > 0.01f)
        {
            pos.x = Mathf.MoveTowards(pos.x, target.x, step);
        }
        else
        {
            pos.y = Mathf.MoveTowards(pos.y, target.y, step);
        }

        return pos;
    }
}