using System;
using System.Collections.Generic;
using System.IO;

namespace Twisted_Dice_Rolling_Simulator
{
    class Program
    {
        private static Random random = new Random();
        private static int userBalance;
        private static Player player1;
        private static Player player2;

        class Player
        {
            public string Name { get; set; }
            public int Balance { get; set; }
        }


        static void Main()
        {
            Console.CursorVisible = false;
            Print("Welcome to the Twisted Dice Rolling Simulator!");

            while (true)
            {
                Console.Clear();
                PrintLogo();

                Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 - 5);
                Console.WriteLine("Main Menu:");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 - 4);
                Console.WriteLine("1. Single Player");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 - 3);
                Console.WriteLine("2. Multiplayer");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 - 2);
                Console.WriteLine("3. About");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 - 1);
                Console.WriteLine("4. Leaderboard");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2);
                Console.WriteLine("5. Exit");

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        InitializeSinglePlayer();
                        PlayGame(player1);
                        break;
                    case ConsoleKey.D2:
                        InitializeMultiplayer();
                        PlayMultiplayerGame();
                        break;
                    case ConsoleKey.D3:
                        DisplayAbout();
                        break;
                    case ConsoleKey.D4:
                        DisplayLeaderboard();
                        break;
                    case ConsoleKey.D5:
                        Console.WriteLine("Exiting the Twisted Dice Rolling Simulator. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please press a key between 1 and 5.");
                        Console.ReadLine();
                        break;
                }
            }
        }


        static void InitializeSinglePlayer()
        {
            player1 = new Player { Name = GettingPlayerName(), Balance = SetInitialBalance() };
        }

        static void InitializeMultiplayer()
        {
            player1 = new Player { Name = GettingPlayerName(), Balance = SetInitialBalance() };
            player2 = new Player { Name = GettingPlayerName(), Balance = SetInitialBalance() };
        }

        static void PlayMultiplayerGame()
        {
            while (player1.Balance > 0 && player2.Balance > 0)
            {
                Console.Clear();
                PrintLogoDice();

                Console.WriteLine(player1.Name + "'s turn. Balance: " + player1.Balance);
                PlayTurn(player1, player2);

                if (player2.Balance <= 0) break; // Checking if player 2 is out of balance

                Console.Clear();
                PrintLogoDice();

                Console.WriteLine(player2.Name + "'s turn. Balance: " + player2.Balance);
                PlayTurn(player2, player1);

                if (player1.Balance <= 0) break; // Checking if player 1 is out of balance

                // Check if it's a draw
                if (player1.Balance > 0 && player2.Balance > 0 && player1.Balance == player2.Balance)
                {
                    Console.WriteLine("It's a draw! Both players have the same balance.");
                    Console.ReadLine();
                    return;
                }
            }

            // Displaying "Game over! Final Balances:"
            Console.WriteLine("Game over! Final Balances:");
            Console.WriteLine(player1.Name + ": " + player1.Balance);
            Console.WriteLine(player2.Name + ": " + player2.Balance);
            Console.ReadLine();
        }

        static void PlayTurn(Player currentPlayer, Player opponent)
        {
            Console.Write(currentPlayer.Name + " enter the number of dice to roll (1-10): ");
            int numberOfDice = PositiveIntegerInput(1, 10);

            int userBet;
            int userGuess;
            int rollResult;

            while (true)
            {
                Console.Write("Place your bet (balance: " + currentPlayer.Balance + "): ");
                userBet = PositiveIntegerInput(1, currentPlayer.Balance);

                if (userBet <= currentPlayer.Balance)
                    break;

                Console.WriteLine("You cannot bet more than your current balance. Please enter a valid bet.");
            }

            while (true)
            {
                Console.Write("Guess the total (between " + numberOfDice + " and " + numberOfDice * 6 + "): ");
                userGuess = PositiveIntegerInput();

                if (userGuess >= numberOfDice && userGuess <= numberOfDice * 6)
                    break;

                Console.WriteLine("Invalid guess. Please enter a valid guess within the specified range.");
            }

            rollResult = RollDice(numberOfDice);

            Console.WriteLine("Total rolled: " + rollResult);

            if (userGuess == rollResult)
            {
                int winnings = userBet * 2;
                Console.WriteLine("Congratulations, " + currentPlayer.Name + "! Your guess is correct. You win: " + winnings);
                currentPlayer.Balance += winnings;
                opponent.Balance -= winnings;
            }
            else
            {
                Console.WriteLine("Sorry, " + currentPlayer.Name + ". The correct total was " + rollResult + ". You lose: " + userBet);
                currentPlayer.Balance -= userBet;
                opponent.Balance += userBet;
            }

            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }




        static void PrintLogo()
        {
            // Display the ASCII art logo 
            string[] logoLines = {
                " █████████╗██████╗ ██████╗ ███████╗",
                " ╚══██╔══╝██╔══██╗██╔══██╗██╔════╝",
                "    ██║   ██║  ██║██████╔╝███████╗",
                "    ██║   ██║  ██║██╔══██╗╚════██║",
                "    ██║   ██████╔╝██║  ██║███████║",
                "    ╚═╝   ╚═════╝ ╚═╝  ╚═╝╚══════╝"
            };

            Console.Clear();

            int topPosition = 2;

            foreach (string line in logoLines)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - line.Length / 2, topPosition);
                Console.WriteLine(line);
                topPosition++;
            }
        }


        static void PrintLogoDice()
        {
            // Display the ASCII art logo 
            string[] logoLines = {
                "  ____                  ",
                " /' . \\    _____         ",
                "/: \\___\\  / .  /\\       ",
                "\\' / . / /____/..\\      ",
                " \\/___/  \\'  '\\  /      ",
                "          \\'__'\\/       "
            };

            Console.Clear();

            int topPosition = 2;

            foreach (string line in logoLines)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - line.Length / 2, topPosition);
                Console.WriteLine(line);
                topPosition++;
            }
        }
        static void PlayGame(Player currentPlayer)
        {
            PrintLogoDice();

            Console.WriteLine("Hello " + currentPlayer.Name + ". Let's start the game. ");
            while (currentPlayer.Balance > 0)
            {
                Console.WriteLine("Your current balance: " + currentPlayer.Balance);

                Console.Write(currentPlayer.Name + " enter the number of dice to roll (1-10): ");
                int numberOfDice = PositiveIntegerInput(1, 10);

                int userBet;
                int userGuess;
                int rollResult;

                // Loop to get a valid bet from the player
                while (true)
                {
                    Console.Write("Place your bet (balance: " + currentPlayer.Balance + "): ");
                    userBet = PositiveIntegerInput(1, currentPlayer.Balance);

                    // Check if the player can afford the bet
                    if (userBet <= currentPlayer.Balance)
                        break;

                    Console.WriteLine("You cannot bet more than your current balance. Please enter a valid bet.");
                }

                // Validate the player's guess
                while (true)
                {
                    Console.Write("Guess the total (between " + numberOfDice + " and " + numberOfDice * 6 + "): ");
                    userGuess = PositiveIntegerInput();

                    if (userGuess >= numberOfDice && userGuess <= numberOfDice * 6)
                        break;

                    Console.WriteLine("Invalid guess. Please enter a valid guess within the specified range.");
                }

                rollResult = RollDice(numberOfDice);

                Console.WriteLine("Total rolled: " + rollResult);

                if (userGuess == rollResult)
                {
                    int winnings = userBet * 2;
                    Console.WriteLine("Congratulations! Your guess is correct. You win: " + winnings);
                    currentPlayer.Balance += winnings;  // Update the player's balance
                }
                else
                {
                    Console.WriteLine("Sorry, the correct total was " + rollResult + ". You lose: " + userBet);
                    currentPlayer.Balance -= userBet;  // Update the player's balance
                }

                Console.Write("Roll again? (y/n): ");
                string response = Console.ReadLine().ToLower();

                if (response != "y")
                {
                    break;
                }
                Console.Clear();
                PrintLogoDice();
            }

            // Save the user balance to leaderboard.txt
            using (StreamWriter writer = new StreamWriter("leaderboard.txt", true))
            {
                writer.WriteLine(currentPlayer.Name + ": " + currentPlayer.Balance);
            }


            Console.WriteLine("Thank you for playing Dice Rolling Simulator! Your final balance is: " + currentPlayer.Balance);
            Console.ReadLine();
        }

        static void DisplayAbout()
        {
            Console.Clear(); // Clear the console screen
            PrintLogo();

            Console.WriteLine("\nWelcome to the Twisted Dice Rolling Simulator where you can enhance your guessing skills.\n ");
            Console.WriteLine("Start the game\n");
            Console.WriteLine("About:\nTwisted Dice Rolling Simulator is about Guessing what number is going to roll out. \nWhere you can double your money or lose it.\n ");
            Console.WriteLine("At First, Input your Name\n");
            Console.WriteLine("You have 100 as starting balance or you can set your own starting balance up to 1000.\n ");
            Console.WriteLine("The Banker will ask you how much is your bet then it will ask you how many dice will roll \nand It will give you a hint the range of numbers you'll be guessing. ");
            Console.WriteLine("Lastly, the game will show you the Numbers and what is the final number out.\n ");

            Console.WriteLine("Press Enter to return to the main menu.");
            Console.ReadLine();
        }

        // Get the bar chart for a given value
        static string GetBarChart(int value)
        {
            const int maxValue = 100; // Adjust this based on the maximum balance value

            // Define characters for different levels of the bar
            char[] barCharacters = { ' ', '▁', '▂', '▃', '▄', '▅', '▆', '▇', '█' };

            // Calculate the number of characters to represent the bar
            int barLength = value * barCharacters.Length / maxValue;

            // Create the bar chart using the selected characters
            string barChart = new string(barCharacters[Math.Min(barLength, barCharacters.Length - 1)], barLength);

            return barChart;
        }

        static void DisplayLeaderboard()
        {
            Console.Clear();

            PrintLogoDice();

            Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 - 5);
            Console.WriteLine("Leaderboard");

            List<string> leaderboardEntries = new List<string>();

            // Read the leaderboard entries from leaderboard.txt and add them to the list
            using (StreamReader reader = new StreamReader("leaderboard.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    leaderboardEntries.Add(line);
                }
            }

            // Sort the leaderboard entries in descending order based on the user balance
            leaderboardEntries.Sort((a, b) =>
            {
                int balanceA = int.Parse(a.Split(':')[1].Trim());
                int balanceB = int.Parse(b.Split(':')[1].Trim());
                return balanceB.CompareTo(balanceA);
            });

            // Display the sorted leaderboard entries
            foreach (string entry in leaderboardEntries)
            {
                string[] parts = entry.Split(':');
                string playerName = parts[0].Trim();
                int balance = int.Parse(parts[1].Trim());

                Console.WriteLine(playerName + " - " + balance);
            }

            // Wait for a key press before moving to the next page
            Console.WriteLine("\nPress Enter to view the bar chart...");

            // Read the user's input
            Console.ReadLine();

            // Display the bar chart on a separate page
            DisplayBarChartPage(leaderboardEntries);
        }

        static void DisplayBarChartPage(List<string> leaderboardEntries)
        {
            Console.Clear();

            PrintLogoDice();

            Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 - 5);
            Console.WriteLine("Bar Chart Page\n");

            // Display the bar chart for each player on a separate line
            foreach (string entry in leaderboardEntries)
            {
                string[] parts = entry.Split(':');
                string playerName = parts[0].Trim();
                int balance = int.Parse(parts[1].Trim());

                Console.WriteLine(playerName + " - " + balance);
                Console.WriteLine("Bar Chart: " + GetBarChart(balance) + "\n");
            }

            Console.WriteLine("\nPress 'M' to return to the main menu or 'L' to view the leaderboard again.");

            // Read the user's input
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.M)
            {
                // Return to the main menu
                // Implement the code to go back to the main menu here
            }
            else if (key.Key == ConsoleKey.L)
            {
                // Display the leaderboard again
                DisplayLeaderboard();
            }
        }





        // Group related methods together
        static int RollDice(int numberOfDice)
        {
            int total = 0;

            Console.WriteLine("Rolling " + numberOfDice + " dice: ");

            for (int i = 0; i < numberOfDice; i++)
            {
                int result = random.Next(1, 7);

                Console.Write("Roll " + (i + 1) + ": ");
                RollingAnimation(result);
                Console.WriteLine();

                total += result;

                if (i < numberOfDice - 1)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }

            Console.WriteLine();
            return total;
        }


        static void RollingAnimation(int result)
        {
            for (int j = 1; j <= 5; j++)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(250);
            }

            Console.Write(result);
        }


        static int PositiveIntegerInput(int minValue = 1, int maxValue = 1000)
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input) || input < minValue || input > maxValue)
            {
                Console.Write("Invalid input. Please enter a positive integer between " + minValue + " and " + maxValue + ": ");
            }
            return input;
        }



        public static void Print(string text, int speed = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }


        static int SetInitialBalance()
        {
            Console.Clear();

            Console.Write("Enter your initial balance (1-1000): ");
            userBalance = PositiveIntegerInput();

            // Limit the initial balance to 1000
            userBalance = Math.Min(userBalance, 1000);


            return userBalance;
        }


        static string GettingPlayerName()
        {
            Console.Clear();

            string playerName;
            while (true)
            {
                Console.Write("Enter your name: ");
                playerName = Console.ReadLine();

                // Check if the name is not blank and contains only alphabetic characters
                bool isAlpha = true;
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    foreach (char c in playerName)
                    {
                        if (!char.IsLetter(c))
                        {
                            isAlpha = false;
                            break;
                        }
                    }
                }
                else
                {
                    isAlpha = false;
                }

                if (isAlpha)
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a non-blank name with alphabetic characters only.");
            }

            return playerName;
        }


    }
}



