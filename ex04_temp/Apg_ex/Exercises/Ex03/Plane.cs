using OpenTK.Mathematics;

namespace Apg_ex.Excercises.Ex03
{
    /// <summary>
    /// Plane in 3D
    /// </summary>
    public struct Plane
    {
        /// <summary>
        /// Normal of the plane 
        /// </summary>
        public Vector3 Normal;
        /// <summary>
        /// Distance from origin
        /// </summary>
        public float Distance;

        /// <summary>
        /// Constructor of plane ax+by+cz+d=0
        /// </summary>
        /// <param name="a"> Normal x </param>
        /// <param name="b"> Normal y </param>
        /// <param name="c"> Normal z </param>
        /// <param name="d"> Distance </param>
        public Plane(float a, float b, float c, float d)
        {
            Vector3 n = new Vector3(a, b, c);
            float length = n.Length;
            Normal = n / length;
            Distance = d / length;
        }

        /// <summary>
        /// Distance from plane to a point
        /// - positive if point on the same side as normal
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float DistanceToPoint(Vector3 point)
        {
            return Vector3.Dot(Normal, point) + Distance;
        }
    }
}
