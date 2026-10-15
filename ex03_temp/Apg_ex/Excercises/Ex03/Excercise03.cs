using Apg_ex.Objects;
using KPGBase.Excercises;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace Apg_ex.Excercises.Ex03
{
    internal class Excercise03 : BasicExcercise
    {
        /// <summary>
        /// All cubes in scene
        /// </summary>
        private List<GameObject> allCubes = new List<GameObject>();
        /// <summary>
        /// BVH tree
        /// </summary>
        private BVH bvhTree;

        /// <summary>
        /// "Fake" camera - rotates in scene
        /// </summary>
        private float cameraAngle = 0f;
        private FrustumWireframe frustumWireframe;

        public Excercise03(int width, int height) : base(width, height)
        {
            shader = new FlatShader("Shaders/flat_shader.vert", "Shaders/flat_shader.frag");
            frustumWireframe = new FrustumWireframe();
            frustumWireframe.Initialize();

            // create a 3D grid of cubes
            Random rand = new Random(42);
            for (int x = -4; x <= 4; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -4; z <= 4; z++)
                    {
                        Vector3 pos = new Vector3(x * 4.5f, y * 4.5f, z * 4.5f);
                        Vector3 rot = new Vector3(0, rand.Next(0, 360), 0);
                        Vector3 scale = Vector3.One * 1.5f;

                        Cube cube = new Cube(pos, rot, scale);
                        cube.Initialize();
                        allCubes.Add(cube);
                    }

            // build BVH tree from the cubes
            bvhTree = new BVH(allCubes);
        }

        /// <summary>
        /// Updates the fake camera position
        /// - rotate its position around 0, 0, 0
        /// </summary>
        public void RotateFakeCamera(float deltaTime)
        {
            cameraAngle += deltaTime * 0.5f;
            cameraAngle %= (float)(2 * Math.PI);
        }

        /// <summary>
        /// Main render call
        /// </summary>
        internal void Draw(Matrix4 view, Matrix4 projection)
        {
            ClearImage();
            shader.Use();

            // == PASS 1 FAKE CAMERA & HIERARCHICAL FRUSTUM CULLING ==
            
            // fake camera
            Vector3 fakeCamPos = new Vector3((float)Math.Cos(cameraAngle) * 20f, 5f, (float)Math.Sin(cameraAngle) * 20f);
            Vector3 fakeCamTarget = Vector3.Zero;
            Matrix4 fakeView = Matrix4.LookAt(fakeCamPos, fakeCamTarget, Vector3.UnitY);
            Matrix4 fakeProj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(50f), (float)width / height, 2.0f, 22.0f);
            Matrix4 fakeVP = fakeView * fakeProj;

            // extract frustrum planes
            Plane[] frustumPlanes = ExtractFrustumPlanes(fakeVP);

            // hierarchical BVH frustum culling
            List<GameObject> visibleCubes = bvhTree.GetVisibleObjects(frustumPlanes);

            // == PASS 2 TOP-DOWN RENDERING ==

            ((FlatShader)shader).SetUniforms(view, projection, new Vector3(0, 1, 0));

            // render VISIBLE cubes as solid
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            foreach (GameObject cube in visibleCubes)
                cube.Draw(shader);

            // render CULLED cubes as wireframe
            ((FlatShader)shader).SetColor(new Vector3(1, 0, 0));
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            foreach (GameObject cube in allCubes)
                if (!visibleCubes.Contains(cube))
                    cube.Draw(shader);

            // render Frustum Lines
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            ((FlatShader)shader).SetColor(new Vector3(1, 1, 1));
            frustumWireframe.Draw(fakeVP, shader);
        }

        /// <summary>
        /// Extracts the 6 frustum planes from a VP matrix
        /// - extracted planes are in world space
        /// - using the Gribb-Hartmann algorithm
        /// </summary>
        private Plane[] ExtractFrustumPlanes(Matrix4 vp)
        {
            Plane[] planes = new Plane[6];

            // left plane
            planes[0] = new Plane(vp.M14 + vp.M11, vp.M24 + vp.M21, vp.M34 + vp.M31, vp.M44 + vp.M41);
            // right plane
            planes[1] = new Plane(vp.M14 - vp.M11, vp.M24 - vp.M21, vp.M34 - vp.M31, vp.M44 - vp.M41);
            // bottom plane
            planes[2] = new Plane(vp.M14 + vp.M12, vp.M24 + vp.M22, vp.M34 + vp.M32, vp.M44 + vp.M42);
            // top plane
            planes[3] = new Plane(vp.M14 - vp.M12, vp.M24 - vp.M22, vp.M34 - vp.M32, vp.M44 - vp.M42);
            // near plane
            planes[4] = new Plane(vp.M14 + vp.M13, vp.M24 + vp.M23, vp.M34 + vp.M33, vp.M44 + vp.M43);
            // far plane
            planes[5] = new Plane(vp.M14 - vp.M13, vp.M24 - vp.M23, vp.M34 - vp.M33, vp.M44 - vp.M43);

            return planes;
        }
    }
}
