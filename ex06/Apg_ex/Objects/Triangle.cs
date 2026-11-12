using OpenTK.Mathematics;

namespace OpenTK_tutorial.Objects
{
    internal class Triangle : GameObject
    {
        
        public Triangle(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            vertices = [
                -0.5f, -0.5f, 0.0f, 0, 0, 1, //Bottom-left vertex
                0.5f, -0.5f, 0.0f, 0, 0, 1, //Bottom-right vertex
                0.0f,  0.5f, 0.0f, 0, 0, 1  //Top vertex
            ];

            indices = [
                0, 1, 2
            ];

            this.Translation = translation;
            this.Rotation = rotation;
            this.Scale = scale;
            CreateModelMatrix();
        }

    }
}
