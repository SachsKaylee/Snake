using UnityEngine;

/// <summary>
///     Provides utility methods for dealing with vectors.
/// </summary>
public static class VectorUtils
{
    /// <summary>
    ///     Checks if the values of a vector 2 is equal to values of a vector
    ///     3. The z value of the vector 3 has to be 0.
    /// </summary>
    public static bool Vector2EqualsVector3(Vector2 v2, Vector3 v3, float z = 0)
    {
        if (v3.z != z)
            return false;
        return v2.x == v3.x && v2.y == v3.y;
    }
}