using FallingParticles;

class Program
{
    static int paddlePosition;
    static string paddle = "========";
    static List<Particle> particles = new List<Particle>();
    static bool isGameOver = false;
    private static readonly Random random = new Random();

    static void Main()
    {
        Console.CursorVisible = false;
        InitializeGame();

        while (!isGameOver)
        {
            DrawGame();
            MovePaddle();
            MoveParticles();
            CheckCollision();
            SpawnParticles();
            Thread.Sleep(100); // Legg til en kort forsinkelse for bedre synlighet
            Console.Clear();
        }

        Console.WriteLine("Game Over!");
        Console.ReadLine();
    }

    static void InitializeGame()
    {
        paddlePosition = Console.WindowWidth / 2 - (Console.WindowWidth % 8);
        particles.Clear();
        isGameOver = false;
    }

    static void DrawGame()
    {
        Console.SetCursorPosition(paddlePosition, Console.WindowHeight - 1);
        Console.Write(paddle);

        foreach (var particle in particles)
        {
            Console.SetCursorPosition(particle.X, particle.Y);
            Console.Write("O");
        }
    }

    static void MovePaddle()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.LeftArrow && paddlePosition >= paddle.Length)
            {
                paddlePosition -= paddle.Length;
            }
            else if (key.Key == ConsoleKey.RightArrow && paddlePosition < Console.WindowWidth - paddle.Length)
            {
                paddlePosition += paddle.Length;
            }
        }
    }

    static void MoveParticles()
    {
        foreach (var particle in particles.ToList())
        {
            particle.Y++;

            // Remove particles that have reached the bottom
            if (particle.Y >= Console.WindowHeight - 1)
            {
                particles.Remove(particle);
            }
        }
    }

    static void CheckCollision()
    {
        foreach (var particle in particles)
        {
            if (particle.X == paddlePosition && particle.Y == Console.WindowHeight - 1)
            {
                isGameOver = true;
                break;
            }
        }
    }

    static void SpawnParticles()
    {
        // Randomly spawn particles at the top
        if (random.Next(0, 100) < 10)
        {
            Particle newParticle = new Particle
            {
                X = random.Next(0, Console.WindowWidth),
                Y = 0
            };

            particles.Add(newParticle);
        }
    }
}
