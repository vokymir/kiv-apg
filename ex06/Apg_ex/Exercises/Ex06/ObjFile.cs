using OpenTK.Mathematics;
using System.Globalization;

namespace Apg_ex.Exercises.Ex06
{
    internal class ObjFile
    {
        public float[] Vertex;
        public float[] Normal;
        public float[] Color;
        public float[] Uv;

        public ObjFile()
        {

        }

        public void LoadObj(string objPath)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en");

            if (!File.Exists(objPath))
            {
                throw new FileNotFoundException($"OBJ file not found: {objPath}");
            }

            var rawPositions = new List<Vector3>();
            var rawNormals = new List<Vector3>();
            var rawUVs = new List<Vector2>();
            var rawColors = new List<Vector3>();
            var faceTokens = new List<string[]>();

            string[] lines = File.ReadAllLines(objPath);

            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                string[] parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "v":
                        if (parts.Length >= 4)
                        {
                            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                            rawPositions.Add(new Vector3(x, y, z));

                            // Check for per-vertex RGB colors (v x y z r g b)
                            if (parts.Length >= 7)
                            {
                                float r = float.Parse(parts[4], CultureInfo.InvariantCulture);
                                float g = float.Parse(parts[5], CultureInfo.InvariantCulture);
                                float b = float.Parse(parts[6], CultureInfo.InvariantCulture);
                                rawColors.Add(new Vector3(r, g, b));
                            }
                            else
                            {
                                rawColors.Add(new Vector3(1.0f, 1.0f, 1.0f));
                            }
                        }
                        break;

                    case "vn":
                        if (parts.Length >= 4)
                        {
                            float nx = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float ny = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            float nz = float.Parse(parts[3], CultureInfo.InvariantCulture);
                            rawNormals.Add(new Vector3(nx, ny, nz));
                        }
                        break;

                    case "vt":
                        if (parts.Length >= 3)
                        {
                            float u = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float v = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            rawUVs.Add(new Vector2(u, v));
                        }
                        break;

                    case "f":
                        if (parts.Length >= 4)
                        {
                            string[] faceVerts = new string[parts.Length - 1];
                            Array.Copy(parts, 1, faceVerts, 0, parts.Length - 1);
                            faceTokens.Add(faceVerts);
                        }
                        break;
                }
            }

            if (faceTokens.Count > 0)
            {
                // Process mesh OBJ files containing face indices
                var vList = new List<float>();
                var nList = new List<float>();
                var cList = new List<float>();
                var uvList = new List<float>();

                foreach (var face in faceTokens)
                {
                    foreach (var vertToken in face)
                    {
                        string[] idxParts = vertToken.Split('/');

                        int vIdx = ParseObjIndex(idxParts[0], rawPositions.Count);
                        if (vIdx >= 0 && vIdx < rawPositions.Count)
                        {
                            var pos = rawPositions[vIdx];
                            vList.Add(pos.X);
                            vList.Add(pos.Y);
                            vList.Add(pos.Z);

                            var col = vIdx < rawColors.Count ? rawColors[vIdx] : Vector3.One;
                            cList.Add(col.X);
                            cList.Add(col.Y);
                            cList.Add(col.Z);
                        }

                        int uvIdx = (idxParts.Length > 1 && !string.IsNullOrEmpty(idxParts[1]))
                            ? ParseObjIndex(idxParts[1], rawUVs.Count) : -1;
                        if (uvIdx >= 0 && uvIdx < rawUVs.Count)
                        {
                            uvList.Add(rawUVs[uvIdx].X);
                            uvList.Add(rawUVs[uvIdx].Y);
                        }
                        else
                        {
                            uvList.Add(0f);
                            uvList.Add(0f);
                        }

                        int nIdx = (idxParts.Length > 2 && !string.IsNullOrEmpty(idxParts[2]))
                            ? ParseObjIndex(idxParts[2], rawNormals.Count) : -1;
                        if (nIdx >= 0 && nIdx < rawNormals.Count)
                        {
                            nList.Add(rawNormals[nIdx].X);
                            nList.Add(rawNormals[nIdx].Y);
                            nList.Add(rawNormals[nIdx].Z);
                        }
                        else
                        {
                            nList.Add(0f);
                            nList.Add(1f);
                            nList.Add(0f);
                        }
                    }
                }

                Vertex = vList.ToArray();
                Normal = nList.ToArray();
                Color = cList.ToArray();
                Uv = uvList.ToArray();
            }
            else
            {
                // Process point cloud OBJs (unindexed v/vn/vt points)
                int count = rawPositions.Count;
                Vertex = new float[count * 3];
                Normal = new float[count * 3];
                Color = new float[count * 3];
                Uv = new float[count * 2];

                for (int i = 0; i < count; i++)
                {
                    Vertex[3 * i + 0] = rawPositions[i].X;
                    Vertex[3 * i + 1] = rawPositions[i].Y;
                    Vertex[3 * i + 2] = rawPositions[i].Z;

                    if (i < rawNormals.Count)
                    {
                        Normal[3 * i + 0] = rawNormals[i].X;
                        Normal[3 * i + 1] = rawNormals[i].Y;
                        Normal[3 * i + 2] = rawNormals[i].Z;
                    }
                    else
                    {
                        Normal[3 * i + 0] = 0f;
                        Normal[3 * i + 1] = 1f;
                        Normal[3 * i + 2] = 0f;
                    }

                    if (i < rawColors.Count)
                    {
                        Color[3 * i + 0] = rawColors[i].X;
                        Color[3 * i + 1] = rawColors[i].Y;
                        Color[3 * i + 2] = rawColors[i].Z;
                    }
                    else
                    {
                        Color[3 * i + 0] = 1f;
                        Color[3 * i + 1] = 1f;
                        Color[3 * i + 2] = 1f;
                    }

                    if (i < rawUVs.Count)
                    {
                        Uv[2 * i + 0] = rawUVs[i].X;
                        Uv[2 * i + 1] = rawUVs[i].Y;
                    }
                    else
                    {
                        Uv[2 * i + 0] = 0f;
                        Uv[2 * i + 1] = 0f;
                    }
                }
            }
        }

        private int ParseObjIndex(string token, int count)
        {
            if (int.TryParse(token, out int idx))
            {
                return idx > 0 ? idx - 1 : count + idx;
            }
            return -1;
        }
    }
}