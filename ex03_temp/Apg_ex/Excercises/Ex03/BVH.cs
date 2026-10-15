using OpenTK_tutorial.Objects;
using OpenTK.Mathematics;
using Apg_ex.Objects;

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

            // a. no objects -> no node (null)
            if (objects.Count == 0) { return null; }


            BVHNode node = new BVHNode();

            // b. just one object -> node is leaf
            // - add list objects to node.Objects
            // - compute node AABB -> set node.Bounds
            if (objects.Count == 1) { 
                var obj = objects[0];
                node.Objects.Add(obj);
                node.Bounds = ((Objects.Cube)obj).GetWorldAABB();
                return node;
            }

            // c. more objects -> inner node
            // - compute total bounding box enclosing all objects -> set node.Bounds
            //      - Cube has method GetWorldAABB, AABB has method Encapsulate
            // - we do NOT have to store all objects in the node
            Vector3 globalMin = ((Objects.Cube)objects[0]).GetWorldAABB().Min; // TODO: simon ma tohle hezci, tak to predelat
            Vector3 globalMax = ((Objects.Cube)objects[0]).GetWorldAABB().Max;
            foreach (GameObject obj in objects) { 
                globalMin = Vector3.ComponentMin(globalMin, ((Objects.Cube)obj).GetWorldAABB().Min);
                globalMax = Vector3.ComponentMax(globalMax, ((Objects.Cube)obj).GetWorldAABB().Max);
            }
            node.Bounds.Min = globalMin;
            node.Bounds.Max = globalMax;


            // - split along the longest axis
            //      - find the longest axis
            //      - sort objects along longest axis based on their translation (position in space)
            //      - find middle object and separate list objects into the children -> recursion
            int ax = 0;
            Vector3 ax_lens = globalMax - globalMin;
            if (ax_lens.X >= ax_lens.Y && ax_lens.X >= ax_lens.Z) { ax = 0; }
            if (ax_lens.Y >= ax_lens.X && ax_lens.Y >= ax_lens.Z) { ax = 1; }
            if (ax_lens.Z >= ax_lens.X && ax_lens.Z >= ax_lens.Y) { ax = 2; }

            List<GameObject> smallerNodes = new();
            List<GameObject> biggerNodes = new();

            float middleCoord = globalMax[ax] - ax_lens[ax] / 2 ;

            foreach (GameObject obj in objects) {
                Cube c = obj as Cube;
                var aabb = c.GetWorldAABB();
                if (aabb.Min[ax] >= middleCoord) { 
                    biggerNodes.Add(obj);
                }
                if (aabb.Min[ax] < middleCoord)
                {
                    smallerNodes.Add(obj);
                }
            }

            node.Left = BuildTree(smallerNodes);
            node.Right = BuildTree(biggerNodes);

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

            // test active planes
            // - for each plane:
            //      - skip plane if plane is deactivated
            //      - find furthest vertex (furthest "along the plane normal")
            //          - if furthest vertex is behind plane -> whole AABB is outside => CULL AABB and its children
            //      - find closest vertex (closest "along the plane normal")
            //          - if closest vertex in front of plane -> AABB is completely inside => DEACTIVATE PLANE for children
            //      - class Plane has method DistanceToPoint
            var newMask = new int[6];
            Array.Copy(activeMask, newMask, planes.Length);
            for (int i = 0; i < planes.Length; ++i) { 
                if (activeMask[i] == 0) { continue; }
                // TODO: this is awful, needs redoing nicer in for-loop for xyz as [i] :0
                Vector3 signs = new Vector3(planes[i].Normal.X > 0 ? 1 : -1, planes[i].Normal.Y > 0 ? 1 : -1, planes[i].Normal.Z > 0 ? 1 : -1);
                Vector3 closest = new Vector3(signs.X > 0 ? node.Bounds.Max.X : node.Bounds.Min.X, signs.Y > 0 ? node.Bounds.Max.Y : node.Bounds.Min.Y, signs.Z > 0 ? node.Bounds.Min.Z : node.Bounds.Max.Z);
                Vector3 furthest = new Vector3(signs.X > 0 ? node.Bounds.Min.X : node.Bounds.Min.X, signs.Y > 0 ? node.Bounds.Min.Y : node.Bounds.Max.Y, signs.Z > 0 ? node.Bounds.Max.Z : node.Bounds.Min.Z);
                if (signs.X < 0)
                {
                    closest.X = node.Bounds.Max.X;
                    furthest.X = node.Bounds.Min.X;
                }
                else { 
                    closest.X = node.Bounds.Min.X;
                    furthest.X = node.Bounds.Max.X;
                }

                if (signs.Y < 0)
                {
                    closest.Y = node.Bounds.Max.Y;
                    furthest.Y = node.Bounds.Min.Y;
                }
                else { 
                    closest.Y = node.Bounds.Min.Y;
                    furthest.Y = node.Bounds.Max.Y;
                }

                if (signs.Z < 0)
                {
                    closest.Z = node.Bounds.Max.Z;
                    furthest.Z = node.Bounds.Min.Z;
                }
                else { 
                    closest.Z = node.Bounds.Min.Z;
                    furthest.Z = node.Bounds.Max.Z;
                }
                float d_closest = planes[i].DistanceToPoint(closest);
                float d_furthest = planes[i].DistanceToPoint(furthest);
                if (d_furthest < 0) { return; }
                if (d_closest > 0) { newMask[i] = 0; }
            }

            // if node passed all tests
            //      - if node is leaf -> add objects to visible list
            //      - if its inner node -> recursion to test children
            // node passed frustum test
            if (node.IsLeaf)
            {
                foreach (var obj in node.Objects)
                {
                    visibleList.Add(obj);
                }
            }
            else {
                if (node.Left != null) {
                    TraverseAndCull(node.Left, planes, newMask, visibleList);
                }
                if (node.Right != null) {
                    TraverseAndCull(node.Right, planes, newMask, visibleList);
                }
            }

        }
    }
}
