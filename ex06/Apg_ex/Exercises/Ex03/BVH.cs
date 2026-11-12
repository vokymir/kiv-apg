using Apg_ex.Objects;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;

namespace Apg_ex.Excercises.Ex03
{
    /// <summary>
    /// BVH tree node
    /// </summary>
    public class BVHNode
    {
        /// <summary>
        /// Bounding box
        /// </summary>
        public AABB Bounds;

        /// <summary>
        /// Children
        /// </summary>
        public BVHNode Left;
        public BVHNode Right;

        /// <summary>
        /// Contained objects
        /// </summary>
        public List<GameObject> Objects = new List<GameObject>();

        /// <summary>
        /// Is node a leaf
        /// </summary>
        public bool IsLeaf => Left == null && Right == null;
    }

    /// <summary>
    /// Bounding volume hierarchy
    /// - a tree
    /// </summary>
    public class BVH
    {
        /// <summary>
        /// Root
        /// </summary>
        public BVHNode Root { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objects"> All objects on which the BVH will be constructed </param>
        public BVH(List<GameObject> objects)
        {
            Root = BuildTree(objects);
        }

        /// <summary>
        /// Build the tree
        /// </summary>
        /// <param name="objects"> All objects on which the BVH will be constructed </param>
        /// <returns> Root </returns>
        private BVHNode BuildTree(List<GameObject> objects)
        {
            // ===== TODO =====

            // no objects -> no node
            if (objects == null || objects.Count == 0) return null;

            BVHNode node = new BVHNode();
            
            // just one object -> node is leaf
            if (objects.Count == 1)
            {
                node.Objects.Add(objects[0]);
                node.Bounds = ((Cube)objects[0]).GetWorldAABB();
                return node;
            }

            // total bounding box enclosing all objects
            AABB totalBox = ((Cube)objects[0]).GetWorldAABB();
            for (int i = 1; i < objects.Count; i++)
                totalBox = AABB.Encapsulate(totalBox, ((Cube)objects[i]).GetWorldAABB());
            node.Bounds = totalBox;

            // split along the longest axis

            // find the longest axis
            Vector3 size = totalBox.Max - totalBox.Min;
            int axis = 0; // 0 = X, 1 = Y, 2 = Z
            if (size.Y > size.X && size.Y > size.Z) axis = 1;
            else if (size.Z > size.X && size.Z > size.Y) axis = 2;

            // sort objects along chosen axis based on their translation
            objects.Sort((a, b) => a.Translation[axis].CompareTo(b.Translation[axis]));

            // find middle and separate into the children
            int mid = objects.Count / 2;
            node.Left = BuildTree(objects.GetRange(0, mid));
            node.Right = BuildTree(objects.GetRange(mid, objects.Count - mid));

            return node;
        }

        /// <summary>
        /// Start frustrum culling
        /// - enter BVH at root
        /// - use plane masking - mask starts at [1, 1, 1, 1, 1, 1]
        /// </summary>
        public List<GameObject> GetVisibleObjects(Plane[] frustumPlanes)
        {
            List<GameObject> visible = new List<GameObject>();

            // initially set all 6 planes as active: bitmask 00111111 (decimal 63)
            int[] initialMask = new int[6] { 1, 1, 1, 1, 1, 1 };

            if (Root != null)
                TraverseAndCull(Root, frustumPlanes, initialMask, visible);

            return visible;
        }

        /// <summary>
        /// Recursive BVH traversal with active plane mask updating.
        /// </summary>
        private void TraverseAndCull(BVHNode node, Plane[] planes, int[] activeMask, List<GameObject> visibleList)
        {
            // ===== TODO =====

            int[] updatedMask = new int[6];

            // test active planes
            for (int i = 0; i < 6; i++)
            {
                updatedMask[i] = activeMask[i];

                // skip plane if mask is deactivated
                if (activeMask[i] == 0)
                    continue; 
                // respective plane
                Plane plane = planes[i];

                // furthest vertex: furthest "along" plane normal
                Vector3 pVertex = new Vector3(
                    plane.Normal.X >= 0 ? node.Bounds.Max.X : node.Bounds.Min.X,
                    plane.Normal.Y >= 0 ? node.Bounds.Max.Y : node.Bounds.Min.Y,
                    plane.Normal.Z >= 0 ? node.Bounds.Max.Z : node.Bounds.Min.Z
                );

                // if furthest vertex is behind plane, whole AABB is outside plane -> CULL
                if (plane.DistanceToPoint(pVertex) < 0)
                    return;

                // closest vertex: least far along plane normal
                Vector3 nVertex = new Vector3(
                    plane.Normal.X >= 0 ? node.Bounds.Min.X : node.Bounds.Max.X,
                    plane.Normal.Y >= 0 ? node.Bounds.Min.Y : node.Bounds.Max.Y,
                    plane.Normal.Z >= 0 ? node.Bounds.Min.Z : node.Bounds.Max.Z
                );

                // closest vertex in front of plane, AABB is COMPLETELY INSIDE plane!
                // deactivate this plane for lower levels
                if (plane.DistanceToPoint(nVertex) >= 0)
                    updatedMask[i] = 0; 
            }

            // node passed frustum test
            if (node.IsLeaf)
                visibleList.AddRange(node.Objects);
            else
            {
                if (node.Left != null) TraverseAndCull(node.Left, planes, updatedMask, visibleList);
                if (node.Right != null) TraverseAndCull(node.Right, planes, updatedMask, visibleList);
            }
        }
    }
}
