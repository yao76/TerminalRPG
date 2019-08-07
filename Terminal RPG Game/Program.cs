using System;
using System.Collections.Generic;
using System.Threading;

// Team HiveMind RPG
// Team Members: Scot 1, Scot 2, Scot 3

namespace Terminal_RPG_Game
{
    public interface IAttackable
    {
        void TakeDamage(int health);
    }
    
    class Item
    {
        string name;
        string type;
        int damageMin;
        int damageMax;

        public Item(string n, string t, int dMin, int dMax)
        {
            name = n;
            type = t;
            damageMax = dMax;
            damageMin = dMin;
        }
        
        public string getName
        {
            get
            {
                return name;
            }
        }

        public int average
        {
            get
            {
                Random rand = new Random();
                return rand.Next(damageMin, damageMax + 1);
            }
        }
        public Item UseItem(Player player)
        {
            player.inventory.Remove(this);
            return this;
        }
    }

    abstract class Character : IAttackable
    {
        protected string _name { get; set; }
        protected int _strength { get; set; }
        protected int _dexterity { get; set; }
        protected int _constitution { get; set; }
        protected int _intelligence { get; set; }
        protected int _wisdom { get; set; }
        protected int _piety { get; set; }
        protected int _level { get; set; }
        protected int currentHealth;

        public Character(string name, int strength, int dexterity, int constitution, int intelligence, int wisdom, int piety)
        {
            _name = name;
            _strength = strength;
            _dexterity = dexterity;
            _constitution = constitution;
            _intelligence = intelligence;
            _wisdom = wisdom;
            _piety = piety;
            _level = 1;
            currentHealth = maxHealth;
        }

        public int maxHealth
        {
            get
            {
                int base_health = (_level * 10);
                int con_modifier = ((_constitution - 10) / 2 ) * 5;
                return (base_health + con_modifier);
            }
        }

        public int getHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                currentHealth = value;    
            }
        }

        public void punch(IAttackable target)
        {
            int damage = _strength;
            target.TakeDamage(damage);
        }

        public void TakeDamage(int damage)
        {
            getHealth -= damage; 
        }
    }

    class Player : Character, IAttackable
    {
        public List<Item> inventory;
        int _phys_armor;
        int _magic_armor;
        protected int baseCrit { get; set; }
        int resource;
        public Item equippedWeapon;

        public Player(string n, int s, int d, int c, int i, int w, int p) : base(n, s, d, c, i, w, p)
        {
            inventory = new List<Item>();
        }

        public void EquipWeapon(Item weapon)
        {
            equippedWeapon = weapon;
        }

        public void PickUpItem(Item item)
        {
            inventory.Add(item);
        }
    }
    
    class Fighter : Player
    {   

        public Fighter(string n, int s, int d, int c, int i, int w, int p) : base(n, s, d, c, i, w, p)
        {
            baseCrit = _dexterity/2;
        }

        public int _damageModifier { 
            get 
            {
                return equippedWeapon.average;
            } 
        }

        public void MeleeAttack(IAttackable target)
        {
            int damage = _strength + _damageModifier;
            target.TakeDamage(damage);
        }

        public void Ultimate(IAttackable target)
        {
            
            int damage = (_level * _strength) + _damageModifier;
            target.TakeDamage(damage);
        }
    }

    class Rogue
    {
        public int ResourceModifier { get; set; }

        public Rogue()
        {
            ResourceModifier = 100;
        }
        public int Ultimate(int level, int dexterity, int crit)
        {
            Random rand = new Random();
            if (rand.Next(1, 101) <= crit)
            {
                return (level * dexterity) * 2;
            }
            return (level * dexterity);
        }
    }

    class Mage
    {
        public int ResourceModifier { get; set; }
        
        public Mage()
        {
            ResourceModifier = 1000;
        }

        public int Ultimate(int level, int intelligence, int remainingResources)
        {
            return level * (remainingResources / 5 * intelligence);
        }
    }

    class Display
    {
        public void drawRect(int x, int y, int w, int h, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            string borderTop = "";
            for (int i = 0; i < w - 2; i++)
                borderTop += "═";
            Console.Write("╔" + borderTop + "╗");
            for (int i = 1; i <= h - 2; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("║");
                Console.SetCursorPosition(x + w - 1, y + i);
                Console.Write("║");
            }
            Console.SetCursorPosition(x, y + h - 1);
            Console.Write("╚" + borderTop + "╝");
        }
        public void clearRect(int x, int y, int w, int h, string character)
        {
            string lineClear = character;
            for (int i = 1; i < w; i++)
                lineClear += character;

            Console.SetCursorPosition(0, 0);
            for (int yc = 0; yc < h; yc++)
            {
                Console.SetCursorPosition(x, y + yc);
                Console.Write(lineClear);
            }
        }
        public void drawText(int x, int y, string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }
    }

    class Program
    {
        int resourceModifer { get; set; }
        static void Main(string[] args)
        {

            /* ```````````````````````````````````````````````````````````````````````````````````*/
            /* ```````````````````````````````````````````````````````````````````````````````````*/
            /* ```````````````````````````````````````````````````````````````````````````````````*/

            Console.CursorVisible = false;
            Display display = new Display();


            display.drawRect(0, 40, 17, 15, ConsoleColor.White);
            display.drawRect(0, 40, 200, 15, ConsoleColor.White);
            display.clearRect(16, 40, 1, 1, "╦");
            display.clearRect(16, 54, 1, 1, "╩");

            display.drawText(4, 42, "Attack", ConsoleColor.White);
            display.drawText(4, 44, "Skills", ConsoleColor.White);
            display.drawText(4, 46, "Inventory", ConsoleColor.White);
            display.drawText(4, 48, "Run", ConsoleColor.White);


            int choice = 0;
            Dictionary<string, object>[] choices = {
                new Dictionary<string, object>() {
                    {"x",4}, {"y",42}, {"text", "Attack"}
                },
                new Dictionary<string, object>() {
                    {"x",4}, {"y", 44}, {"text", "Skills"}
                },
                new Dictionary<string, object>() {
                    {"x",4}, {"y", 46}, {"text", "Inventory"}
                },
                new Dictionary<string, object>() {
                    {"x",4}, {"y", 48}, {"text", "Run"}
                }
            };

            ConsoleKeyInfo cki;
            string gameState = "LeftMenu";
            while (true)
            {
                display.drawText((int)choices[choice]["x"], (int)choices[choice]["y"], choices[choice]["text"].ToString(), ConsoleColor.Green);

                Console.SetCursorPosition(0, 0);
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(16);
                }
                display.clearRect(0, 0, 10, 1, " ");

                cki = System.Console.ReadKey(true);

                if (gameState == "LeftMenu")
                {
                    if (cki.Key.ToString() == "DownArrow")
                    {
                        display.drawText((int)choices[choice]["x"], (int)choices[choice]["y"], choices[choice]["text"].ToString(), ConsoleColor.White);
                        choice = (choice + 1) % choices.Length;
                    }
                    else if (cki.Key.ToString() == "UpArrow")
                    {
                        display.drawText((int)choices[choice]["x"], (int)choices[choice]["y"], choices[choice]["text"].ToString(), ConsoleColor.White);
                        choice--;
                        if (choice == -1) choice = choices.Length - 1;
                    }
                }
                else if (gameState == "RightMenu")
                {
                    Console.WriteLine("wut");
                }

                Console.ForegroundColor = ConsoleColor.White;


            }
        }
    }
}
