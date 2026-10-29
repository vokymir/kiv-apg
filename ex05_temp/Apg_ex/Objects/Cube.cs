using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apg_ex.Excercises.Ex03;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace Apg_ex.Objects
{
    internal class Cube : GameObject
    {
        public Cube(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            // Position (3 floats) + Normal (3 floats) = stride of 6 floats per vertex
            vertices = new float[]
            {
                // Front face
                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                 0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                // Back face
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            };

            indices = new uint[]
            {
                // Front
                0, 1, 2,  2, 3, 0,
                // Right
                1, 5, 6,  6, 2, 1,
                // Back
                7, 6, 5,  5, 4, 7,
                // Left
                4, 0, 3,  3, 7, 4,
                // Top
                3, 2, 6,  6, 7, 3,
                // Bottom
                4, 5, 1,  1, 0, 4
            };

            Translation = translation;
            Rotation = rotation;
            Scale = scale;
            CreateModelMatrix();
        }

        /// <summary>
        /// Calculates world-space Axis-Aligned Bounding Box (AABB) for this cube.
        /// </summary>
        public AABB GetWorldAABB()
        {
            Vector3 localMin = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 localMax = new Vector3(0.5f, 0.5f, 0.5f);

            // Transform all 8 local corners into world space
            Vector3[] corners = new Vector3[8]
            {
                new Vector3(localMin.X, localMin.Y, localMin.Z),
                new Vector3(localMax.X, localMin.Y, localMin.Z),
                new Vector3(localMin.X, localMax.Y, localMin.Z),
                new Vector3(localMax.X, localMax.Y, localMin.Z),
                new Vector3(localMin.X, localMin.Y, localMax.Z),
                new Vector3(localMax.X, localMin.Y, localMax.Z),
                new Vector3(localMin.X, localMax.Y, localMax.Z),
                new Vector3(localMax.X, localMax.Y, localMax.Z)
            };

            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            foreach (var corner in corners)
            {
                Vector4 worldCorner = new Vector4(corner, 1.0f) * model;
                min = Vector3.ComponentMin(min, worldCorner.Xyz);
                max = Vector3.ComponentMax(max, worldCorner.Xyz);
            }

            return new AABB(min, max);
        }
    }
}