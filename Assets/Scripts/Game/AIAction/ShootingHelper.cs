using UnityEngine;

public static class ShootingHelper
{
    public static Vector2 GetShootVector2(Vector2 positionOfTheGun, Vector2 positionOfTheTarget, Vector2 velocityOfTheTarget, float bulletSpeed)
        => GetShootVector2(bulletSpeed, velocityOfTheTarget, positionOfTheTarget.x, positionOfTheTarget.y, positionOfTheGun.x, positionOfTheGun.y);
    
    /// <summary>
    /// Gets an direction of an bullet
    /// </summary>
    /// <param name="u">Bullet speed</param>
    /// <param name="v">Velocity of the target</param>
    /// <param name="x0">X of target</param>
    /// <param name="y0">Y of target</param>
    /// <param name="xs">X of start</param>
    /// <param name="ys">Y of start</param>
    /// <returns></returns>
    public static Vector2 GetShootVector2(float u, Vector2 v, float x0, float y0, float xs, float ys)
    {
        float dx = x0 - xs;
        float dy = y0 - ys;

        float a = v.sqrMagnitude - u * u;
        float b = 2 * (v.x * dx + v.y * dy); // yes, it's a dot product
        float c = dx * dx + dy * dy;

        float D = b * b - 4 * a * c;

        if (D < 0) // no solutions exist
            return Vector2.zero;

        float t = (-b - Mathf.Sqrt(D)) / (2 * a);

        // the condition can only be met when a > 0, which means that
        if (t < 0) // when t < 0 both solutions are negative
            return Vector2.zero;

        // find the shooting direction
        return ((new Vector2(x0, y0) + v * t) - new Vector2(xs, ys)).normalized;
    }
}
