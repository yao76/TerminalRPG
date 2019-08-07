using System;
using System.Collections.Generic;

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

    class Program
    {
        int resourceModifer { get; set; }
        static void Main(string[] args)
        {
            Fighter Diablo = new Fighter("Diablo", 100, 100, 82, 120, 50, 1);
            Console.WriteLine(Diablo.getHealth);
            Fighter Uriel = new Fighter("Uriel", 80, 70, 60, 140, 100, 160);
            Console.WriteLine(Uriel.getHealth);
            Diablo.punch(Uriel);
            Console.WriteLine("Diablo casts fuck you on Uriel");
            Console.WriteLine(Uriel.getHealth);
            Item longsword = new Item("Flametongue", "A fucking sword", 30, 35);
            Uriel.PickUpItem(longsword);
            Uriel.EquipWeapon(longsword.UseItem(Uriel));
            Uriel.MeleeAttack(Diablo);
            Console.WriteLine(Diablo.getHealth);
            Uriel.Ultimate(Diablo);
            Console.WriteLine(Diablo.getHealth);
        }
    }
}
