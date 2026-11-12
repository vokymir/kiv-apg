using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apg_ex.Objects
{
    /// <summary>
    /// Very simple class for loading X-file. It returns the list of vertices, indices, normals and texcoords.
    /// </summary>
    class XFile
    {
        /// <summary>
        /// XFile exception
        /// </summary>
        public class XFileException : ApplicationException
        {
            public XFileException(string message)
                : base(message)
            {
            }

            public XFileException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        /// <summary>
        /// Float3 vector
        /// </summary>
        public struct Vector3f
        {
            public float x;
            public float y;
            public float z;

            public void Parse(string line)
            {
                string[] coordsStr = line.Split(';');
                try
                {
                    x = float.Parse(coordsStr[0]);
                    y = float.Parse(coordsStr[1]);
                    z = float.Parse(coordsStr[2]);
                }
                catch (Exception e)
                {
                    throw new XFileException("Invalid vector3f format in " + Block.block + ":" + Block.name + " (line: " + line + ")", e);
                }
            }
        }

        /// <summary>
        /// Float2 vector
        /// </summary>
        public struct Vector2f
        {
            public float x;
            public float y;

            public void Parse(string line)
            {
                string[] coordsStr = line.Split(';');
                try
                {
                    x = float.Parse(coordsStr[0]);
                    y = float.Parse(coordsStr[1]);
                }
                catch (Exception e)
                {
                    throw new XFileException("Invalid vector2f format in " + Block.block + ":" + Block.name + " (line: " + line + ")", e);
                }
            }
        }

        /// <summary>
        /// Integer3 vector
        /// </summary>
        public struct Vector3i
        {
            public uint v1;
            public uint v2;
            public uint v3;

            public void Parse(string line)
            {
                string[] coordsStr = line.Split(new char[] { ';', ',' });
                try
                {
                    if (int.Parse(coordsStr[0]) != 3) throw new XFileException("Invalid number of indices (not a triangle) in Mesh " + Block.name);
                    v1 = uint.Parse(coordsStr[1]);
                    v2 = uint.Parse(coordsStr[2]);
                    v3 = uint.Parse(coordsStr[3]);
                }
                catch (Exception e)
                {
                    throw new XFileException("Invalid vector3i format in " + Block.block + ":" + Block.name + " (line: " + line + ")", e);
                }
            }

        }

        Vector3f[] vertex;

        /// <summary>
        /// Mesh vertices
        /// </summary>
        public Vector3f[] Vertex
        {
            get
            {
                return vertex;
            }
        }

        Vector3i[] index;

        /// <summary>
        /// Mesh Indices
        /// </summary>
        public Vector3i[] Index
        {
            get
            {
                return index;
            }
        }

        Vector3f[] normal;

        /// <summary>
        /// Mesh normals
        /// </summary>
        public Vector3f[] Normal
        {
            get { return normal; }
        }

        Vector2f[] texCoord;

        /// <summary>
        /// Mesh normals
        /// </summary>
        public Vector2f[] TexCoord
        {
            get { return texCoord; }
        }

        /// <summary>
        /// internal static structure for block name parsing
        /// </summary>
        struct Block
        {
            public static string block;
            public static string name;

            public static void GetBlock(string line)
            {
                string[] tokens = line.Trim().Split(' ');
                block = tokens[0];
                if (tokens[1].Contains("{")) name = "";
                else name = tokens[1];
            }
        }

        string[] file;
        int filePos;

        /// <summary>
        /// Start loading xfile
        /// </summary>
        /// <param name="name"></param>
        public void Load(string name)
        {
            try
            {
                System.Globalization.CultureInfo current = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                file = System.IO.File.ReadAllLines(name);
                filePos = 0;

                Read();
                System.Threading.Thread.CurrentThread.CurrentCulture = current;
            }
            catch (Exception e)
            {
                throw new Exception("XFile loading exception", e);
            }
        }

        /// <summary>
        /// Reads the xfile till the end
        /// Recognized blocks: Frame
        /// May contain: ?
        /// </summary>
        void Read()
        {
            while (!file[filePos].Contains("}"))
            {
                if (file[filePos].Contains("{"))
                {
                    Block.GetBlock(file[filePos]);
                    switch (Block.block)
                    {
                        case "Frame": FrameBlock(); break;
                        default: UnknownBlock(); break;
                    }
                }
                else
                    filePos++;
            }
        }

        /// <summary>
        /// Frame block
        /// Recognized blocks: Frame, Mesh
        /// May contain: FrameTransformMatrix, ?
        /// </summary>
        /// <param name="block"></param>
        void FrameBlock()
        {
            filePos++;
            while (!file[filePos].Contains("}"))
            {
                if (file[filePos].Contains("{"))
                {
                    Block.GetBlock(file[filePos]);
                    switch (Block.block)
                    {
                        case "Frame": FrameBlock(); break;
                        case "Mesh": MeshBlock(); break;
                        case "FrameTransformMatrix":
                        default: UnknownBlock(); break;
                    }
                }
                else
                {
                    filePos++;
                }
            }
            filePos++;

        }

        /// <summary>
        /// Mesh block.
        /// Recognized blocks: MeshNormals, MeshTextureCoords
        /// May contain: MeshMaterialList, SkinMeshHeader, SkinWeights, ?
        /// </summary>
        void MeshBlock()
        {
            filePos++;

            int verticesCount = int.Parse(file[filePos].Split(';')[0]);
            vertex = new Vector3f[verticesCount];
            for (int i = 0; i < verticesCount; i++)
            {
                filePos++;
                vertex[i].Parse(file[filePos]);
            }

            filePos++;
            int indicesCount = int.Parse(file[filePos].Split(';')[0]);
            index = new Vector3i[indicesCount];
            for (int i = 0; i < indicesCount; i++)
            {
                filePos++;
                index[i].Parse(file[filePos]);
            }

            filePos++;
            while (!file[filePos].Contains("}"))
            {
                if (file[filePos].Contains("{"))
                {
                    Block.GetBlock(file[filePos]);
                    switch (Block.block)
                    {
                        case "MeshNormals": MeshNormalsBlock(); break;
                        case "MeshTextureCoords": MeshTextureCoordsBlock(); break;
                        case "MeshMaterialList":
                        case "SkinMeshHeader":
                        case "SkinWeights":
                        default: UnknownBlock(); break;
                    }
                }
                else
                {
                    filePos++;
                }
            }
        }

        /// <summary>
        /// MeshNormals block.
        /// Recognized blocks: None
        /// May contain: ?
        /// </summary>
        void MeshNormalsBlock()
        {
            filePos++;
            normal = new Vector3f[vertex.Length];
            for (int i = 0; i < vertex.Length; i++)
            {
                filePos++;
                normal[i].Parse(file[filePos]);
            }

            while (!file[filePos].Contains("}"))
            {
                filePos++;
            }
            filePos++;
        }

        /// <summary>
        /// MeshTextureCoords block.
        /// Recognized blocks: None
        /// May contain: ?
        /// </summary>
        void MeshTextureCoordsBlock()
        {
            filePos++;
            texCoord = new Vector2f[vertex.Length];
            for (int i = 0; i < vertex.Length; i++)
            {
                filePos++;
                texCoord[i].Parse(file[filePos]);
            }

            while (!file[filePos].Contains("}"))
            {
                filePos++;
            }
            filePos++;
        }



        /// <summary>
        /// Unknown block
        /// </summary>
        /// <param name="block"></param>
        void UnknownBlock()
        {
            while (!file[filePos].Contains("}"))
            {
                filePos++;
                if (file[filePos].Contains("{"))
                {
                    UnknownBlock();
                }
            }
            filePos++;
        }
    }
}
