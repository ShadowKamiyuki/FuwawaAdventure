using UnityEngine;

public class LineOfSight
{
    public bool IsInRange(Transform self, Transform target, float distance)
    {
        return Vector3.Distance(self.position, target.position) < distance;
    }

    public bool IsInAngle(Transform self, Transform target, float angle)
    {
        // direction vector from self to the target
        Vector3 dir = target.position - self.position;

        return Vector3.Angle(self.forward, dir) < angle / 2;
    }

    public bool CheckObstacles(Transform self, Transform target, LayerMask obstacles)
    {
        Vector3 dir = target.position - self.position;

        return !Physics.Raycast(self.position, dir.normalized, dir.magnitude, obstacles);
    }
}
