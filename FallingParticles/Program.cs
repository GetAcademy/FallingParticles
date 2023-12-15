using FallingParticles;

class Program
{
    static int paddlePosition;
    private static int paddleMoveDistance = 6;
    static string paddle = "========";
    static List<Particle> particles = new List<Particle>();
    static bool isGameOver = false;
    static int level = 1;
    static int score = 1;
    static int gameRoundsBetweenSpawn;
    private static readonly Random random = new Random();

    static void Main()
    {
        Console.CursorVisible = false;
        Console.WindowWidth = 80;
        InitializeGame();
        var levelCount = 0;
        var roundCount = 40;
        while (!isGameOver)
        {
            DrawGame();
            MovePaddle();
            MoveParticles();
            CheckCollision();
            if (roundCount >= gameRoundsBetweenSpawn)
            {
                SpawnParticles();
                InitGameRoundsBetweenSpawn();
                roundCount = 0;
            }
            roundCount++;
            levelCount++;
            if (levelCount == 50) level++;
            Thread.Sleep(100);
        }

        Console.WriteLine("Game Over!");
        Console.ReadLine();
    }

    static void InitializeGame()
    {
        var centerX = Console.WindowWidth / 2;
        paddlePosition = centerX - centerX % paddleMoveDistance;
        particles.Clear();
        isGameOver = false;
        InitGameRoundsBetweenSpawn();
    }

    static void InitGameRoundsBetweenSpawn()
    {
        gameRoundsBetweenSpawn = 50 / level;
    }

    static void DrawGame()
    {
        Console.Clear();
        Console.SetCursorPosition(60, 0);
        Console.Write($"Score: {score}");
        Console.SetCursorPosition(71, 0);
        Console.Write($"Level: {level}");
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
            var moveLeft = key.Key == ConsoleKey.LeftArrow && paddlePosition >= paddleMoveDistance;
            var moveRight = key.Key == ConsoleKey.RightArrow && paddlePosition < Console.WindowWidth - paddle.Length;
            if (moveLeft || moveRight)
            {
                var direction = moveLeft ? -1 : 1;
                paddlePosition += direction * 3 * paddle.Length / 4;
            }
        }
    }

    static void MoveParticles()
    {
        for (var index = particles.Count - 1; index >= 0; index--)
        {
            var particle = particles[index];
            particle.Y++;
            if (particle.Y >= Console.WindowHeight - 1)
            {
                score++;
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
        var newParticle = new Particle
        {
            X = random.Next(0, Console.WindowWidth),
            Y = 0
        };
        particles.Add(newParticle);
    }
}
