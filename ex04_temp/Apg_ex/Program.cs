namespace OpenTK_tutorial
{

    public class Program
    {
        public static void Main(String[] args)
        {
            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.

            using (Game game = new Game(800, 600, "Game window"))
            {
                game.Run();
            }

        }
    }
}