using UnityEngine;

public class SteearingBehaviours
{
    public static Vector3 Seek(Transform self, Vector3 target)
    {
        Vector3 dir = target - self.position;
        dir.y = 0;
        return dir.normalized;
    }

    public static Vector3 Flee(Transform self, Vector3 target)
    {
        Vector3 dir = self.position - target;
        dir.y = 0;
        return dir.normalized;
    }

    public static Vector3 Arrive(Transform self, Vector3 target, float slowRadius)
    {
        Vector3 dir = target - self.position;
        float distance = dir.magnitude;

        if (distance < 0.01f)
            return Vector3.zero;

        float speedMultiplier = Mathf.Clamp01(distance / slowRadius);
        return dir.normalized * speedMultiplier;
    }

    private static Vector3 CalculateFuturePosition(Transform self, Transform target, Rigidbody targetRB, float maxPredictionTime)
    {
        Vector3 targetVelocity = Vector3.zero;
        targetVelocity = targetRB.velocity;

        Vector3 toTarget = target.position - self.position;
        toTarget.y = 0;

        float distance = toTarget.magnitude;
        float predictionTime = Mathf.Clamp(distance / 5f, 0f, maxPredictionTime);

        return target.position + targetVelocity * predictionTime;
    }

    public static Vector3 Pursue(Transform self, Transform target, Rigidbody targetRB, float maxPredictionTime)
    {
        Vector3 futurePos = CalculateFuturePosition(self, target, targetRB, maxPredictionTime);
        return Seek(self, futurePos);
    }

    public static Vector3 Evade(Transform self, Transform target, Rigidbody targetRB, float maxPredictionTime)
    {
        Vector3 futurePos = CalculateFuturePosition(self, target, targetRB, maxPredictionTime);
        return Flee(self, futurePos);
    }

    public static Vector3 Wander(Vector3 currentDirection, float maxAngleChange)
    {
        float randomAngle = Random.Range(-maxAngleChange, maxAngleChange);
        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);

        Vector3 newDirection = rotation * currentDirection;
        newDirection.y = 0f;

        return newDirection.normalized;
    }
}
