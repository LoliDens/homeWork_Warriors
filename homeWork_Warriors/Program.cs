using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace homeWork_Warriors
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Knight knight = new Knight(100,10,10,30,3);
            Magican magican = new Magican(100, 10, 10,40,20,20);
            Thief thief = new Thief(100, 10, 10, 40, 7);
            ShieldBearer shieldBearer = new ShieldBearer(100, 10, 10, 30);
            Vampire vampire = new Vampire(100, 10, 10, 20);

            List<Warrior> warriors = new List<Warrior>() { knight, magican, thief, shieldBearer, vampire };
            Arena arena = new Arena(warriors);
            arena.Play();
        }
    }

    class Arena
    {
        private List<Warrior> _warriors;
        private Warrior _firstWarrior;
        private Warrior _secondWarrior;

        public Arena(List<Warrior> warriors)
        {
            _warriors = warriors;
        }

        public void Play() 
        {
            SelectWarriors();
            Fight();
            AnnounceWinner();
        }

        private void SelectWarriors()
        {
            Console.WriteLine("Choose the warriors");
            _firstWarrior = SelectWarrior("first");
            _secondWarrior = SelectWarrior("second");
        }

        private void Fight()
        {
            while (_firstWarrior.IsAlive && _secondWarrior.IsAlive)
            {
                HitWarior(_firstWarrior, _secondWarrior);
                Console.WriteLine();
                HitWarior(_secondWarrior, _firstWarrior);
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void AnnounceWinner()
        {
            string messageWiner = "Winner {0} warrior";

            if (!_firstWarrior.IsAlive && !_secondWarrior.IsAlive)
            {
                Console.WriteLine("Drow");
            }
            else if (_firstWarrior.IsAlive)
            {
                Console.WriteLine(String.Format(messageWiner, "first"));
            }
            else
            {
                Console.WriteLine(String.Format(messageWiner, "second"));
            }

            Console.ReadKey();
        }

        private void HitWarior(Warrior warrior,Warrior enemy)
        {
            warrior.DealDamage(enemy);
            enemy.ShowState();
        }


        private Warrior SelectWarrior(string numberWarior)
        {
            Console.WriteLine(String.Format("select {0} warrior: ", numberWarior));
            
            for(int i = 0; i < _warriors.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {_warriors[i].GetName}");
            }

            int input = ReadNumber(1, _warriors.Count);
            Warrior selectWarrior = _warriors[input - 1].Clone();
            Console.WriteLine($"You select {selectWarrior}");
            Console.ReadKey();
            Console.Clear();

            return selectWarrior;
        }
        private int ReadNumber(int min = int.MinValue, int max = int.MaxValue)
        {
            int result = 0;
            bool isWrok = false;

            while (!isWrok)
            {
                string number = Console.ReadLine();

                while (int.TryParse(number, out result) == false)
                {
                    Console.WriteLine("Input error.Re-enter the number");
                    number = Console.ReadLine();
                }

                if (result <= max && result >= min)
                    isWrok = true;
                else
                    Console.WriteLine("Input error.Number is out for range");
            }

            return result;
        }
    }

    class Vampire : Warrior
    {
        private int _powerVampirism;

        public Vampire(int health, int damage, int armor, int powerVampirism) : base(health, damage, armor)
        {
            _powerVampirism = powerVampirism;
            Name = "Vampire";
            Description = $"restores {_powerVampirism}% of health from damage";
        }

        public override Warrior Clone()
        {
            return new Vampire(Health, Damage, Armor, _powerVampirism);
        }

        public override void DealDamage(Warrior Enemy)
        {
            int maxPercentage = 100;
            int regeneration = (int)(Damage / (float)maxPercentage * _powerVampirism);
            Console.WriteLine($"{Name} regeneration {regeneration} helth");
            Health += regeneration;
            base.DealDamage(Enemy);
        }
    }

    class ShieldBearer : Warrior
    {
        private int _chanseBlock;

        public ShieldBearer(int health, int damage, int armor,int chanseBlock) : base(health, damage, armor)
        {
            _chanseBlock = chanseBlock;
            Name = "Shield Bearer";
            Description = $"has a chance to block damage {chanseBlock}";
        }

        public override Warrior Clone()
        {
            return new ShieldBearer(Health, Damage, Armor, _chanseBlock);
        }

        public override void TakeDamage(int damage)
        {
            int maxRandom = 100;
            int value = random.Next(maxRandom);

            if (_chanseBlock >= value)
                Console.WriteLine("Block");
            else
                base.TakeDamage(damage);
        }
    }

    class Thief : Warrior
    {
        private int _critChance;
        private int _bonusCritDamage;

        public Thief(int health, int damage, int armor,int critChance,int bonusCritDamage) : base(health, damage, armor)
        {
            _critChance = critChance;
            _bonusCritDamage = bonusCritDamage;
            Name = "Thief";
            Description = $"Has a {_critChance} chance to deal an additional {_bonusCritDamage} damage";
        }

        public override Warrior Clone()
        {
            return new Thief(Health, Damage, Armor, _critChance,_bonusCritDamage);
        }

        public override void DealDamage(Warrior enemy)
        {
            int maxCritChance = 100;
            int value = random.Next(maxCritChance);

            if (_critChance >= value)
            {
                int critDamage = Damage + _bonusCritDamage;
                Console.WriteLine($"{Name} dealt {critDamage} crit damage to {enemy.GetName}");
                enemy.TakeDamage(critDamage);
            }
            else
            {
                base.DealDamage(enemy);
            }
        }
    }

    class Magican : Warrior
    {
        private int _amountMans;
        private int _fireballCost;
        private int _fireballDamage;

        public Magican(int health, int damage, int armor,int amountMans,int fireballCost,int fireballDamage) : base(health, damage, armor)
        {
            _amountMans = amountMans;
            _fireballCost = fireballCost;
            _fireballDamage = fireballDamage;
            Name = "Magican";
            Description = $"Has a mana amount of {_amountMans} and a fireball spell that costs {_fireballCost} mana, nanochit {_fireballDamage} damage";
        }

        public override Warrior Clone()
        {
            return new Magican(Health, Damage, Armor, _amountMans, _fireballCost, _fireballDamage);
        }

        public override void DealDamage(Warrior Enemy)
        {
            if (_amountMans >= _fireballCost)
            {
                _amountMans -= _fireballCost;
                Console.WriteLine($"{Name} dealt {_fireballDamage} fireball damage to {Enemy.GetName}");
                Console.WriteLine($"{Name} spent {_fireballCost} mana left {_amountMans}");
                Enemy.TakeDamage(_fireballDamage);
            }
            else 
            {
                Enemy.TakeDamage(Damage);
            }
        }
    }

    class Knight : Warrior 
    {
        private int _criticalHealth;
        private int _bonusDamage;

        public Knight(int health,int damage,int armor,int criticalHealth,int bonusDamage) : base(health,damage,armor)
        {
            _criticalHealth = criticalHealth;
            _bonusDamage = bonusDamage;
            Name = "Knight";
            Description = $"When health is less than {_criticalHealth} increases damage by {_bonusDamage}";
        }

        public override Warrior Clone()
        {
            return new Knight(Health,Damage,Armor,_criticalHealth,_bonusDamage);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if(Health <= _criticalHealth)
                Damage += _bonusDamage;
        }
    }

    abstract class Warrior
    {
        protected static Random random = new Random();

        public string Name { get; private set; }
        protected int Health;
        protected int Damage;
        protected int Armor;
        protected string Description;

        public Warrior(int health, int damage, int armor)
        {
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public abstract Warrior Clone();

        public string GetName => Name;

        public bool IsAlive => Health > 0;

        public virtual void TakeDamage(int damage)
        {
            const double ValueProcentArmor = 100;
            double damageReduction = Armor / (ValueProcentArmor + Armor);
            damage = Convert.ToInt32(damage * (1.0 - damageReduction));
            Console.WriteLine($"{Name} received {damage} damage");
            Health -= damage;
        }

        public virtual void DealDamage(Warrior enemy)
        {
            Console.WriteLine($"{Name} dealt {Damage} damage to {enemy.Name}");
            enemy.TakeDamage(Damage);
        }


        public void ShowState()
        {
            Console.WriteLine($"Name: {Name} Health: {Health}");
        }

        public override string ToString()
        {
            return $"Name: {Name}\nHealth: {Health}\nDamage: {Damage}\nArmor: {Armor}\nDescription: {Description}";
        }
    }
}
