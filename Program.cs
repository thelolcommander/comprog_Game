using System;

public class Player
{
    public string Name;
    public int Health;
    public int Attack;
    public int Level;
    public int Defense;

    public Player(string name)
    {
        Name = name;
        Health = 100;
        Attack = 10;
        Level = 1;
        Defense = 0;
    }

    public void LevelUp()
    {
        Level++;
        Attack += 5;
        Health += 20;
    }

    public void ApplyBuff(string buffType)
    {
        Random rand = new Random();
        switch (buffType)
        {
            case "Health":
                int healthBuff = rand.Next(10, 30);
                Health += healthBuff;
                Console.WriteLine($"{Name} gained {healthBuff} Health!");
                break;
            case "Attack":
                int attackBuff = rand.Next(5, 15);
                Attack += attackBuff;
                Console.WriteLine($"{Name} gained {attackBuff} Attack!");
                break;
            case "Defense":
                int defenseBuff = rand.Next(3, 10);
                Defense += defenseBuff;
                Console.WriteLine($"{Name} gained {defenseBuff} Defense!");
                break;
            default:
                Console.WriteLine("Invalid buff type.");
                break;
        }
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = damage - Defense;
        actualDamage = actualDamage > 0 ? actualDamage : 0;
        Health -= actualDamage;
    }

    public void Heal()
    {
        int healAmount = 20;
        Health += healAmount;
    }

    public void Defend()
    {
        Defense = 5;
    }

    public void ResetDefense()
    {
        Defense = 0;
    }
}

public class Enemy
{
    public string Name;
    public int Health;
    public int Attack;

    public Enemy(string name, int health, int attack)
    {
        Name = name;
        Health = health;
        Attack = attack;
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}

public class Dungeon
{
    private Random _random;
    private Player _player;
    private int _enemyMultiplier;

    public Dungeon(Player player)
    {
        _random = new Random();
        _player = player;
        _enemyMultiplier = 1;  // Initial multiplier
    }

    public void EnterRoom()
    {
        bool enemyEncountered = _random.Next(0, 2) == 0;
        if (enemyEncountered)
        {
            Enemy enemy = GenerateEnemy();
            Combat(enemy);
        }
        else
        {
            Console.Clear();  // Clear the screen
            DisplayStaticInfo();

            // Buff Room
            string[] buffs = { "Health", "Attack", "Defense" };
            string chosenBuff = buffs[_random.Next(buffs.Length)];

            // Apply the buff and display the message
            ApplyBuffWithMessage(chosenBuff);

            // Increase enemy multiplier
            _enemyMultiplier++;
            ChooseNextAction();
        }
    }

    private void Combat(Enemy enemy)
    {
        Console.Clear();  // Clear the screen
        DisplayStaticInfo();

        Console.WriteLine($"A wild {enemy.Name} appears!");

        while (_player.IsAlive() && enemy.IsAlive())
        {
            // Display combat actions
            Console.WriteLine("\nChoose your action:");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Heal");
            Console.WriteLine("3. Defend");

            string choice = Console.ReadLine();

            // Clear the action choices line
            Console.Clear();
            DisplayStaticInfo();  // Re-display static info after clearing the screen

            int damageDealt = 0;

            switch (choice)
            {
                case "1":
                    // Check for critical hit
                    damageDealt = CriticalHit(_player.Attack);
                    enemy.TakeDamage(damageDealt);
                    Console.WriteLine($"{_player.Name} attacks! {enemy.Name} takes {damageDealt} damage! Health is now {enemy.Health}.");
                    break;

                case "2":
                    _player.Heal();
                    Console.WriteLine($"{_player.Name} heals! Health is now {_player.Health}.");
                    break;

                case "3":
                    _player.Defend();
                    Console.WriteLine($"{_player.Name} defends, increasing defense to {_player.Defense}.");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
                    continue;
            }

            if (enemy.IsAlive())
            {
                _player.TakeDamage(enemy.Attack);
                Console.WriteLine($"{enemy.Name} attacks! {_player.Name} takes {enemy.Attack} damage! Health is now {_player.Health}.");
            }
            else
            {
                Console.Clear();
                DisplayStaticInfo();

                Console.WriteLine($"You defeated {enemy.Name}!");
                _player.LevelUp();
                break;
            }

            _player.ResetDefense();  // Reset defense after the player's turn
        }

        if (!_player.IsAlive())
        {
            Console.Clear();
            DisplayStaticInfo();

            Console.WriteLine("You have been defeated! Game Over!");
        }
    }

    private Enemy GenerateEnemy()
    {
        string[] enemyNames = { "Goblin", "Orc", "Troll", "Skeleton" };
        string enemyName = enemyNames[_random.Next(enemyNames.Length)];
        int enemyHealth = _random.Next(30, 60) * _enemyMultiplier;
        int enemyAttack = _random.Next(5, 15) * _enemyMultiplier;

        return new Enemy(enemyName, enemyHealth, enemyAttack);
    }

    private int CriticalHit(int baseDamage)
    {
        int criticalChance = _random.Next(0, 100);
        if (criticalChance < 20) // 20% chance for critical hit
        {
            Console.WriteLine("Critical hit!");
            return baseDamage * 2; // Double damage on critical hit
        }
        return baseDamage;
    }

    private void ChooseNextAction()
    {
        // Display action choices
        Console.WriteLine("\nWhat do you want to do?");
        Console.WriteLine("1. Enter the next room");
        Console.WriteLine("2. Quit");

        string input = Console.ReadLine();

        if (input == "1")
        {
            EnterRoom();
        }
        else if (input == "2")
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid choice. Please select 1 or 2.");
            ChooseNextAction();
        }
    }

    private void DisplayStaticInfo()
    {
        Console.WriteLine(@"
██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ███████╗ ██████╗ ███╗   ██╗     ██████╗██████╗  █████╗ ██╗    ██╗██╗     ██╗███╗   ██╗
██╔══██╗██║   ██║████╗  ██║██╔════╝ ██╔════╝██╔═══██╗████╗  ██║    ██╔════╝██╔══██╗██╔══██╗██║    ██║██║     ██║████╗  ██║
██║  ██║██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██║   ██║██╔██╗ ██║    ██║     ██████╔╝███████║██║ █╗ ██║██║     ██║██╔██╗ ██║
██║  ██║██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║   ██║██║╚██╗██║    ██║     ██╔══██╗██╔══██║██║███╗██║██║     ██║██║╚██╗██║
██████╔╝╚██████╔╝██║ ╚████║╚██████╔╝███████╗╚██████╔╝██║ ╚████║    ╚██████╗██║  ██║██║  ██║╚███╔███╔╝███████╗██║██║ ╚████║
╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚══╝╚══╝ ╚══════╝╚═╝╚═╝  ╚═══╝                                                                                                                 
Welcome to the Roguelike Dungeon Game!");
        Console.WriteLine($"Enter your character's name: {_player.Name}");
        Console.WriteLine("");
    }

    private void ApplyBuffWithMessage(string buffType)
    {
        Random rand = new Random();
        int buffAmount = 0;

        switch (buffType)
        {
            case "Health":
                buffAmount = rand.Next(10, 30);
                _player.Health += buffAmount;
                Console.WriteLine($"You enter a room filled with mystery and you gain Health +{buffAmount}. You currently have {_player.Health} Health.");
                break;
            case "Attack":
                buffAmount = rand.Next(5, 15);
                _player.Attack += buffAmount;
                Console.WriteLine($"You enter a room filled with mystery and you gain Attack +{buffAmount}. You currently have {_player.Attack} Attack.");
                break;
            case "Defense":
                buffAmount = rand.Next(3, 10);
                _player.Defense += buffAmount;
                Console.WriteLine($"You enter a room filled with mystery and you gain Defense +{buffAmount}. You currently have {_player.Defense} Defense.");
                break;
            default:
                Console.WriteLine("Invalid buff type.");
                break;
        }
    }
}

public class Game
{
    public static void Main()
    {
        Console.WriteLine(@"
██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ███████╗ ██████╗ ███╗   ██╗     ██████╗██████╗  █████╗ ██╗    ██╗██╗     ██╗███╗   ██╗
██╔══██╗██║   ██║████╗  ██║██╔════╝ ██╔════╝██╔═══██╗████╗  ██║    ██╔════╝██╔══██╗██╔══██╗██║    ██║██║     ██║████╗  ██║
██║  ██║██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██║   ██║██╔██╗ ██║    ██║     ██████╔╝███████║██║ █╗ ██║██║     ██║██╔██╗ ██║
██║  ██║██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║   ██║██║╚██╗██║    ██║     ██╔══██╗██╔══██║██║███╗██║██║     ██║██║╚██╗██║
██████╔╝╚██████╔╝██║ ╚████║╚██████╔╝███████╗╚██████╔╝██║ ╚████║    ╚██████╗██║  ██║██║  ██║╚███╔███╔╝███████╗██║██║ ╚████║
╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚══╝╚══╝ ╚══════╝╚═╝╚═╝  ╚═══╝                                                                                                                 
Welcome to the Roguelike Dungeon Game!");
        Console.Write("Enter your character's name: ");

        string playerName = Console.ReadLine();
        Player player = new Player(playerName);

        Dungeon dungeon = new Dungeon(player);
        bool isPlaying = true;

        while (isPlaying && player.IsAlive())
        {
            Console.Clear();
            dungeon.EnterRoom();
        }

        if (!player.IsAlive())
        {
            Console.Clear();
            Console.WriteLine(@"
██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ███████╗ ██████╗ ███╗   ██╗     ██████╗██████╗  █████╗ ██╗    ██╗██╗     ██╗███╗   ██╗
██╔══██╗██║   ██║████╗  ██║██╔════╝ ██╔════╝██╔═══██╗████╗  ██║    ██╔════╝██╔══██╗██╔══██╗██║    ██║██║     ██║████╗  ██║
██║  ██║██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██║   ██║██╔██╗ ██║    ██║     ██████╔╝███████║██║ █╗ ██║██║     ██║██╔██╗ ██║
██║  ██║██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║   ██║██║╚██╗██║    ██║     ██╔══██╗██╔══██║██║███╗██║██║     ██║██║╚██╗██║
██████╔╝╚██████╔╝██║ ╚████║╚██████╔╝███████╗╚██████╔╝██║ ╚████║    ╚██████╗██║  ██║██║  ██║╚███╔███╔╝███████╗██║██║ ╚████║
╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚══╝╚══╝ ╚══════╝╚═╝╚═╝  ╚═══╝                                                                                                                 
Welcome to the Roguelike Dungeon Game!");
            Console.WriteLine($"Enter your character's name: {player.Name}");
            Console.WriteLine("");

            Console.WriteLine("You have died! Game Over!");
        }
    }
}
