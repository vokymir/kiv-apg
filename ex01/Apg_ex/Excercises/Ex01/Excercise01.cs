using KPGBase.Excercises;
using OpenTK.Mathematics;
using OpenTK_tutorial.Objects;
using OpenTK_tutorial.Shaders;

namespace OpenTK_tutorial.Excercises.Ex01
{
    /// <summary>
    /// OpenGL introduction
    /// </summary>
    internal class Excercise01 : BasicExcercise
    {
        // square objects
        Square background1;
        Square background2;
        Square background3;
        Square floor;
        Square player;
        Square house;

        float speed = 300f;
        float X_MAX = 300;
        float X_MIN = -300;
        float Y_MAX = -200;
        float Y_MIN = -300;
        float SCALE_MIN = 30;
        float SCALE_MAX = 50;

        /// <summary>
        /// Constructor
        /// </summary>
        public Excercise01(int width, int height) : base(width, height)
        {
            background1 = new Square(new Vector3(-100, -100, 0), new Vector3(), new Vector3(400, 400, 1));
            background1.Initialize();
            background1.SetTexture("./Textures/bg_1.png");

            background2 = new Square(new Vector3(-100, -150, 0), new Vector3(), new Vector3(400, 400, 1));
            background2.Initialize();
            background2.SetTexture("./Textures/bg_2.png");

            background3 = new Square(new Vector3(-100, -200, 0), new Vector3(), new Vector3(400, 400, 1));
            background3.Initialize();
            background3.SetTexture("./Textures/bg_3.png");

            floor = new Square(new Vector3(-400, -300, 0), new Vector3(), new Vector3(800, 120, 1));
            floor.Initialize();
            floor.SetTexture("./Textures/grass.png");

            house = new Square(new Vector3(-100, -250, 0), new Vector3(), new Vector3(100, 100, 1));
            house.Initialize();
            house.SetTexture("./Textures/house.png");

            player = new Square(new Vector3(1, -300, 0), new Vector3(), new Vector3(50, 50, 1));
            player.Initialize();
            player.SetTexture("./Textures/person.png");

            // create shader here
            shader = new TextureShader("Shaders/tex_shader.vert", "Shaders/tex_shader.frag");
        }

        /// <summary>
        /// Draw triangle using the BasicExcercise shader
        /// </summary>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        internal void DrawingSandbox(Matrix4 view, Matrix4 projection)
        {
            ClearImage();
            Console.WriteLine($"{player.Scale.X}");

            ((TextureShader)shader).Use();
            ((TextureShader)shader).SetUniforms(view, projection, new Vector3(0, 1, 0));

            background1.Draw(shader);
            background2.Draw(shader);
            background3.Draw(shader);
            floor.Draw(shader);
            house.Draw(shader);
            player.Draw(shader);
        }

        /// <summary>
        /// Move player from side to side
        /// - check so the object does not escape the visible area
        /// </summary>
        /// <param name="v"> true = move back, false = move forward </param>
        /// <param name="time"> time since last frame </param>
        internal void MovePlayerBack(bool v, double time)
        {
            /// ** TODO
            float dir = v ? -1 : 1;
            float translationChange = -1 * dir * speed * (float)time;


            float newTransY = player.Translation.Y + translationChange;
            if (newTransY>Y_MAX) newTransY = Y_MAX;
            if (newTransY < Y_MIN) newTransY = Y_MIN;
            player.Translation = new Vector3(player.Translation.X, newTransY, player.Translation.Z);


            float newScale = (Y_MAX - player.Translation.Y) / (Y_MAX - Y_MIN) * (SCALE_MAX - SCALE_MIN) + SCALE_MIN;

            player.Scale = new Vector3(newScale, newScale, 1);
        }

        /// <summary>
        /// Move player to the back or to the front
        /// - change y coordinate
        /// - change player's scale
        /// - check so the object does not escape the allowed area
        /// </summary>
        /// <param name="v"> true = move left, false = move right</param>
        /// <param name="time"> time since last frame </param>
        internal void MovePlayerSides(bool v, double time)
        {
            /// ** TODO
            float dir = v ? -1 : 1;
            float translationChange = 1 * dir * speed * (float)time;

            float newTransX = player.Translation.X + translationChange;
            if (newTransX>X_MAX) newTransX = X_MAX;
            if (newTransX < X_MIN) newTransX = X_MIN;

            player.Translation = new Vector3(newTransX, player.Translation.Y, player.Translation.Z);
        }

        /// <summary>
        /// Draw whole scenery
        /// - draw backgroud1-3 - 3 times, it tiles when repeated in the x axis
        ///     - draw the image in the center, change its coordinates, draw it again...
        ///     - do not forget to set the coordinates back
        /// - decide wheter to draw player in front or behinf the house
        /// </summary>
        /// <param name="view"> View matrix </param>
        /// <param name="projection"> Projection matrix </param>
        internal void DrawScenery(Matrix4 view, Matrix4 projection)
        {
            ClearImage();

            ((TextureShader)shader).Use();
            ((TextureShader)shader).SetUniforms(view, projection, new Vector3(0, 1, 0));

            background1.Draw(shader);
            background1.Translation = new Vector3(background1.Translation.X - 400, background1.Translation.Y, background1.Translation.Z);
            background1.Draw(shader);
            background1.Translation = new Vector3(background1.Translation.X + 800, background1.Translation.Y, background1.Translation.Z);
            background1.Draw(shader);
            background1.Translation = new Vector3(background1.Translation.X - 400, background1.Translation.Y, background1.Translation.Z);

            // TODO
        }
    }
}
