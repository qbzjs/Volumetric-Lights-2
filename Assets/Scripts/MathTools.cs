using UnityEngine;

public class MathTools
{
    public static Vector3 GetPointFromAngleAndDistance(Vector3 startingPoint, float yAngleDegrees, float distance)
    {
        // Convert the Y angle to radians
        float yAngleRadians = yAngleDegrees * Mathf.Deg2Rad;

        // Calculate the X and Z offsets using trigonometry
        float xOffset = distance * Mathf.Sin(yAngleRadians);
        float zOffset = distance * Mathf.Cos(yAngleRadians);

        // Create a new Vector3 with the calculated offsets and the same Y position as the starting point
        Vector3 newPoint = new Vector3(startingPoint.x + xOffset, startingPoint.y, startingPoint.z + zOffset);

        return newPoint;
    }
    
    public static Vector3 RotatePointAroundCenter(Vector3 center, float angle, Vector3 point)
    {
        // Convert the angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate the sin and cosine of the angle
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        // Translate the point to the origin
        Vector3 translatedPoint = point - center;

        // Apply the rotation
        float x = translatedPoint.x * cos - translatedPoint.z * sin;
        float z = translatedPoint.x * sin + translatedPoint.z * cos;

        // Translate the point back to its original position
        Vector3 rotatedPoint = new Vector3(x, point.y, z) + center;

        return rotatedPoint;
    }
}