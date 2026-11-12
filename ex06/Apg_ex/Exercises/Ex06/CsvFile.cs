using System.Text;

namespace Apg_ex.Exercises.Ex06
{
    internal class CSVFile
    {
        public float[] Vertex;
        public float[] Normal;
        public float[] Color;

        public void LoadFile(string filename)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en");

            List<float> vertices = new List<float>();
            List<float> normals = new List<float>();
            List<float> colors = new List<float>();

            const Int32 BufferSize = 256;
            using (var fileStream = File.OpenRead(filename))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    // Process line
                    string[] parts = line.Split(" ");
                    float x = float.Parse(parts[0]);
                    float y = float.Parse(parts[1]);
                    float z = float.Parse(parts[2]);

                    vertices.Add(x);
                    vertices.Add(y);
                    vertices.Add(z);

                    float r = float.Parse(parts[3]);
                    float g = float.Parse(parts[4]);
                    float b = float.Parse(parts[5]);

                    colors.Add(r);
                    colors.Add(g);
                    colors.Add(b);

                    if (parts.Length > 6)
                    {
                        float nx = float.Parse(parts[6]);
                        float ny = float.Parse(parts[7]);
                        float nz = float.Parse(parts[8]);

                        normals.Add(nx);
                        normals.Add(ny);
                        normals.Add(nz);
                    } else
                    {
                        normals.Add(0);
                        normals.Add(0);
                        normals.Add(0);
                    }

                }
            }

            this.Vertex = vertices.ToArray();
            this.Normal = normals.ToArray();
            this.Color = colors.ToArray();
        }

    }
}
