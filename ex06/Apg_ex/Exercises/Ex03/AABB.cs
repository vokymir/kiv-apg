using OpenTK.Mathematics;

namespace Apg_ex.Excercises.Ex03
{
    /// <summary>
    /// Axis aligned bouding box
    /// </summary>
    public struct AABB
    {
        /// <summary>
        /// Minimum
        /// </summary>
        public Vector3 Min;
        /// <summary>
        /// Maximum
        /// </summary>
        public Vector3 Max;

        /// <summary>
        /// Constructor
        /// </summary>
        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Join two AABB together
        /// </summary>
        /// <param name="a"> First AABB </param>
        /// <param name="b"> Second AABB </param>
        /// <returns></returns>
        public static AABB Encapsulate(AABB a, AABB b)
        {
            return new AABB(
                Vector3.ComponentMin(a.Min, b.Min),
                Vector3.ComponentMax(a.Max, b.Max)
            );
        }
    }
}