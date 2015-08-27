using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {
        struct Position
        {
            public int row;
            public int col;
            public Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }

        static void Main(string[] args)
        {
            byte right = 0;
            byte left = 1;
            byte up = 2;
            byte down = 3;
            int lastFoodTime = 0;
            int foodDissapearTime = 8000;
            int negativePoints = 0;

            Position[] directions = new Position[]
            {
                new Position(0,1), //right
                new Position(0, -1), //left
                new Position(-1, 0), //up
                new Position(1, 0), //down
            };
            double sleepTime = 100;
            int direction = 0;
            Random randomNumbersGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
            lastFoodTime = Environment.TickCount;

            List<Position> obsticles = new List<Position>()
            {
                new Position(12,12),
                new Position(15,30),
                new Position(7,14),
            };
            foreach (var obstucle in obsticles)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstucle.col, obstucle.row);
                Console.Write("X");
            }

            Queue<Position> snakeEl = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeEl.Enqueue(new Position(0, i));
            }

            Position food;
            do
            {
                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
            }
            while (snakeEl.Contains(food) || obsticles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@");

            foreach (Position position in snakeEl)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("*");
            }

            while (true)
            {
                negativePoints++;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    if (input.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right)
                        {
                            direction = left;
                        }
                    }
                    if (input.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left)
                        {

                            direction = right;
                        }
                    }
                    if (input.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down)
                        {
                            direction = up;
                        }
                    }
                    if (input.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up)
                        {
                            direction = down;
                        }
                    }
                }

                Position snakeHead = snakeEl.Last();
                Position nextDirection = directions[direction];
                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row, snakeHead.col + nextDirection.col);

                if (snakeNewHead.col < 0)
                {
                    snakeNewHead.col = Console.WindowWidth - 1;
                }
                if (snakeNewHead.row < 0)
                {
                    snakeNewHead.row = Console.WindowHeight - 1;
                }
                if (snakeNewHead.row >= Console.WindowHeight)
                {
                    snakeNewHead.row = 0;
                }
                if (snakeNewHead.col >= Console.WindowWidth)
                {
                    snakeNewHead.col = 0;
                }

                /*(snakeNewHead.row < 0 || snakeNewHead.col < 0 || snakeNewHead.row >= Console.WindowHeight || snakeNewHead.col >= Console.WindowWidth || */
                if (snakeEl.Contains(snakeNewHead) || obsticles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game Over!");
                    int userPoints = ((snakeEl.Count - 6) * 100) - negativePoints;
                    userPoints = Math.Max(userPoints, 0);
                    Console.WriteLine($"Your point are: {userPoints}");
                    return;
                }


                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("*");

                snakeEl.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                if (direction == right) Console.Write(">");
                if (direction == left) Console.Write("<");
                if (direction == up) Console.Write("^");
                if (direction == down) Console.Write("v");

                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
                {
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeEl.Contains(food) || obsticles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@");
                    sleepTime--;

                    Position obsticle = new Position();
                    do
                    {
                        obsticle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeEl.Contains(obsticle) || obsticles.Contains(obsticle) || food.row != obsticle.row && food.col != food.col);
                    obsticles.Add(obsticle);
                    Console.SetCursorPosition(obsticle.col, obsticle.row);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("X");
                }
                else
                {
                    Position last = snakeEl.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }

                if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeEl.Contains(food));
                    lastFoodTime = Environment.TickCount;
                }

                Console.SetCursorPosition(food.col, food.row);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@");

                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);

            }
        }
    }
}