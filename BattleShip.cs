using System;
using System.Threading;

namespace BattleShip
{
    class Program   
    {
        static char[,] board;
        static bool[,] hiddenBoard;
        static int fragsLeft = 4;
        static int shipsLeft = 3;
        static int carriersLeft = 1;
        static int attempts = 0;
        static int hits = 0;
        static string playerName = "";
        static int boardSize = 10; // Tamaño predeterminado del tablero
        static bool beginnerMode = false; // true: Principiante (10x10), false: Avanzado (20x20)
        static int maxAttempts = 30; // Número máximo de intentos predeterminado

        static void Main(string[] args)
        {
            ShowWelcomeMessage();
            MostrarInstrucciones();
            ConfigurarJuego();
            InitializeBoard();
            PlaceShips();

            while (fragsLeft + shipsLeft + carriersLeft > 0 && attempts < maxAttempts)
            {
                DrawBoard();
                Attack();
            }

            MostrarEstadisticas(playerName);
        }

        static void ShowWelcomeMessage()//Pantalla1, BIENVENIDA
        {
            // Cambiar el color de la consola a rojo
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("********************************************************************************************");
            Console.WriteLine("*                                                                                          *");
            Console.WriteLine("*                                     BATTLESHIP                                           *");
            Console.WriteLine("*                                                                                          *");
            Console.WriteLine("********************************************************************************************");

            Console.ResetColor(); // Restaurar su valor predeterminado

            Console.WriteLine("¡Bienvenido!");
            Console.WriteLine("SUERTE MARINERO! AL ATAQUE....");

            Thread.Sleep(2000); // Espera 2 segundos antes de continuar
            Console.Clear(); // Limpia la pantalla después del mensaje
        }

        static void MostrarInstrucciones()//Pantalla2, REGLAS DEL JUEGO
        {
            Console.WriteLine("                                                            -INTRO-");
            Console.WriteLine("BattleShip es un juego de estrategia en el que dos jugadores compiten para hundir los barcos del oponente en un tablero. " +
"Es un juego de estrategia, lógica y un poco de suerte." +
"¡Espero que te diviertas jugando!");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");


            Console.WriteLine("                                                           -MODO DE JUEGO-");
            Console.WriteLine("Los barcos disponibles son Fragatas, Navíos y Portaaviones. " +
                "Al comenzar la partida se intentará adivinar las posiciones de los barcos del contrincante, " +
                "si alguno logra derribar los barcos del otro jugador, entonces este será el ganador.");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");



            Console.WriteLine("                                                            -DETALLES-");
            Console.WriteLine("En esta ocasión se jugara con barcos unitarios y tu contrincante será la computadora.");
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void ConfigurarJuego()//Pantalla3, MODO DE JUEGO
        {
            Console.WriteLine("                    -HOME-");
            Console.WriteLine("\n");
            Console.Write("¿Cuál es tu nombre Marinero? ");
            playerName = Console.ReadLine();
            Console.WriteLine("\n");
            Console.WriteLine("\n");

            Console.WriteLine("********** NIVEL *********");
            Console.WriteLine("\n");
            Console.WriteLine("1. Principiante");//Matriz de 10x10
            Console.WriteLine("2. Avanzado");//Matriz de 20x20
            Console.WriteLine("\n");
            Console.Write("¿Cuál eres tú?: ");
            int selectedDifficulty;
            while (!int.TryParse(Console.ReadLine(), out selectedDifficulty) || selectedDifficulty < 1 || selectedDifficulty > 2)
            {
                Console.WriteLine("Entrada inválida. Por favor, seleccione 1 o 2.");
                Console.Write("Seleccione la dificultad (1-2): ");
            }
            beginnerMode = (selectedDifficulty == 1);
            boardSize = beginnerMode ? 10 : 20;

            Console.WriteLine("********************+ MODO DE JUEGO*****************");
            Console.WriteLine("\n");
            Console.WriteLine("¿Con cuantos intentos planeas destruir a tu enemigo?");
            Console.WriteLine("1. 5 intentos");
            Console.WriteLine("2. 10 intentos");
            Console.WriteLine("3. 20 intentos");
            Console.WriteLine("4. 30 intentos");
            Console.Write("¿Cuantos?: ");
            int selectedAttempts;

            while (!int.TryParse(Console.ReadLine(), out selectedAttempts) || selectedAttempts < 1 || selectedAttempts > 4)
            {
                Console.WriteLine("Entrada inválida. Por favor, seleccione una opción válida.");
                Console.Write("Seleccione el número máximo de intentos (1-4): ");
            }

            switch (selectedAttempts)
            {
                case 1:
                    maxAttempts = 5;
                    break;
                case 2:
                    maxAttempts = 10;
                    break;
                case 3:
                    maxAttempts = 20;
                    break;
                case 4:
                    maxAttempts = 30;
                    break;
            }

            Console.Clear();
        }

        static void InitializeBoard()
        {
            board = new char[boardSize, boardSize];
            hiddenBoard = new bool[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = '-';
                    hiddenBoard[i, j] = false;
                }
            }
        }

        static void PlaceShips()
        {
            Random random = new Random();
            PlaceShip(random, 'F', 4);
            PlaceShip(random, 'N', 3, 2);
            PlaceShip(random, 'P', 1, 3);
        }

        static void PlaceShip(Random random, char shipType, int count, int size = 1)
        {
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(boardSize);
                    y = random.Next(boardSize);
                } while (!CanPlaceShip(x, y, size));

                for (int j = 0; j < size; j++)
                {
                    if (shipType == 'P')
                    {
                        board[x + j, y] = '-';
                        hiddenBoard[x + j, y] = true;
                    }
                    else
                    {
                        board[x, y + j] = '-';
                        hiddenBoard[x, y + j] = true;
                    }
                }
            }
        }

        static bool CanPlaceShip(int x, int y, int size)
        {
            if (x + size > boardSize || y + size > boardSize)
                return false;

            for (int i = x; i < x + size; i++)
            {
                for (int j = y; j < y + size; j++)
                {
                    if (hiddenBoard[i, j])
                        return false;
                }
            }
            return true;
        }

        static void DrawBoard()
        {
            Console.Clear();
            Console.Write("   ");
            for (int i = 0; i < boardSize; i++)
            {
                Console.Write(((char)('A' + i)) + " ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
            for (int i = 0; i < boardSize; i++)
            {
                Console.Write((i + 1).ToString().PadLeft(2) + "| ");
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j] == '-' || board[i, j] == 'O' || board[i, j] == 'X' || hiddenBoard[i, j]) // Agregamos hiddenBoard[i, j]
                        Console.Write(board[i, j] + " ");
                    else
                        Console.Write("- "); // Muestra '-' en lugar de los barcos
                }
                Console.WriteLine();
            }
        }

        static void Attack()
        {
            Console.Write("Ingrese fila (1-" + boardSize + "): ");
            int row;
            if (!int.TryParse(Console.ReadLine(), out row) || row < 1 || row > boardSize)
            {
                Console.WriteLine("Entrada inválida para la fila.");
                return;
            }

            Console.Write("Ingrese columna (A-" + (char)('A' + boardSize - 1) + "): ");
            char col = char.Parse(Console.ReadLine().ToUpper());
            int colIndex = col - 'A';

            if (colIndex < 0 || colIndex >= boardSize)
            {
                Console.WriteLine("Entrada inválida para la columna.");
                return;
            }

            attempts++;

            if (hiddenBoard[row - 1, colIndex])
            {
                if (board[row - 1, colIndex] == 'F')
                {
                    fragsLeft--;
                    board[row - 1, colIndex] = 'X';
                    Console.WriteLine("¡Fragata destruida!");
                    hits++;
                }
                else if (board[row - 1, colIndex] == 'N')
                {
                    shipsLeft--;
                    board[row - 1, colIndex] = 'X';
                    Console.WriteLine("¡Navío destruido!");
                    hits++;
                }
                else if (board[row - 1, colIndex] == 'P')
                {
                    carriersLeft--;
                    board[row - 1, colIndex] = 'X';
                    Console.WriteLine("¡Portaavión destruido!");
                    hits++;
                }
            }
            else if (board[row - 1, colIndex] == '-')
            {
                board[row - 1, colIndex] = 'O';
                Console.WriteLine("¡Agua!");
            }
            else
            {
                Console.WriteLine("Ya has atacado esa posición.");
            }

            Console.WriteLine("Presiona Enter para continuar...");
            Console.ReadLine();
        }

        static void MostrarEstadisticas(string playerName)
        {
            Console.Clear();
            Console.WriteLine("¡Juego terminado!");
            Console.WriteLine("Jugador: " + playerName);
            Console.WriteLine("Intentos realizados: " + attempts);
            Console.WriteLine("Aciertos: " + hits);
            Console.WriteLine("Presiona Enter para salir...");
            Console.ReadLine();
        }
    }
}
