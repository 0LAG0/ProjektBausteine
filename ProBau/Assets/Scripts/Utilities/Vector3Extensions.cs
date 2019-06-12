using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BrickIt.Vector3Extensions
{
    /// <summary>
    /// Vector3 extensions.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Method for ceiling every component of the vector.
        /// </summary>
        /// <param name="vec">The vector to ceil.</param>
        /// <returns>The vector with each component ceiled.</returns>
        public static Vector3 Ceil(this Vector3 vec)
        {
            return new Vector3(Mathf.Ceil(vec.x), Mathf.Ceil(vec.y), Mathf.Ceil(vec.z));
        }

        /// <summary>
        /// Divides the vectors components through another vectors components.
        /// </summary>
        /// <param name="vec0">Dividend</param>
        /// <param name="vec1">Divisor</param>
        /// <returns>Componentwise divided vector</returns>
        public static Vector3 DivideComponentwise(this Vector3 vec0, Vector3 vec1)
        {
            return new Vector3(vec0.x / vec1.x, vec0.y / vec1.y, vec0.z / vec1.z);
        }

        public static Vector3 ClampToPositive(this Vector3 vec0)
        {
            if (vec0.x < 0)
                vec0.x = 0;
            if (vec0.y < 0)
                vec0.y = 0;
            if (vec0.z < 0)
                vec0.z = 0;
            return vec0;
        }
    }
}
