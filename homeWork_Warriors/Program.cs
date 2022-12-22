using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace homeWork_Warriors
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Fight fight = new Fight();
            fight.Work();
        }
    }

    class Fight 
    {
        public void Work()
        {
            Console.WriteLine("выберите двух войнов для битвы:");
            Console.ReadKey();
            Warrior warriorOne = SelectWarrior();
            Warrior warriorTwo = SelectWarrior();

            Console.WriteLine("");
            warriorOne.ShowInfo();

            
            while(warriorOne.isAlive && warriorTwo.isAlive)
            {
                warriorOne.DealDamage(warriorTwo);
                warriorTwo.ShowInfo();
                warriorTwo.DealDamage(warriorOne);
                warriorOne.ShowInfo();
            }

            if (warriorOne.isAlive == false && warriorTwo.isAlive == false) 
            {
                Console.WriteLine("Ничья");
            }
            else if (warriorOne.isAlive == false)
            {
                Console.WriteLine($"Победил {warriorTwo.Name}");
            }
            else if (warriorTwo.isAlive == false)
            {
                Console.WriteLine($"Победил {warriorOne.Name}");
            }

            Console.ReadKey();
        }

        private Warrior SelectWarrior() 
        {
            const string CommandSelecrSwordman = "1";
            const string CommandSelectThief = "2";
            const string CommandSelectBarbarion = "3";
            const string CommandSelectKnigh = "4";
            const string CommandSelectMonarch = "5";

            bool isSelect = false;
            Warrior warrior = null;

            while (isSelect == false) {
                Console.WriteLine($"нажмитие\n" +
                    $"{CommandSelecrSwordman} - Мечник \n" +
                    $"{CommandSelectThief} - Вор \n" +
                    $"{CommandSelectBarbarion} - Варвор \n" +
                    $"{CommandSelectKnigh} - Рыдцарь \n" +
                    $"{CommandSelectMonarch} - Монарх \n");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandSelecrSwordman:
                        warrior = new Swordman();
                        isSelect = true;
                        break;

                    case CommandSelectThief:
                        warrior = new Thief();
                        isSelect = true;
                        break;

                    case CommandSelectBarbarion:
                        warrior = new Barbarion();
                        isSelect = true;
                        break;

                    case CommandSelectKnigh:
                        warrior = new Knigh();
                        isSelect = true;
                        break;

                    case CommandSelectMonarch:
                        warrior = new Monarch();
                        isSelect = true;
                        break;

                    default:
                        Console.WriteLine("Комманда не распознана");
                        break;
                }
            }

            return warrior;
        }
    }

    abstract class Warrior
    {
        public bool isAlive => Health > 0;
        protected int Health { get; private set; }
        protected int Damage { get; private set; }
        protected int Armor { get; private set; }
        public string Name { get; private set; }

        public Warrior(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя {Name}\n" +
                $"Здоровье {Health}\n" +
                $"Урон {Damage}\n" +
                $"Броня {Armor}\n");
        }

        public void DecreaseArmor(int value) 
        {
            Armor -= value;
        }

        public void Healing(int value) 
        {
            Health += value;
        }

        public virtual void TakeDamage(int damage) 
        {
            const double ValueProcentArmor = 100;
            double damageReduction = Armor / (ValueProcentArmor + Armor);
            damage = Convert.ToInt32(damage * (1.0 - damageReduction));
            Console.WriteLine($"{Name}у  нанисли {damage} урона");
            Health -= damage;
        }

        public virtual void DealDamage(Warrior Enemy) 
        {
            Enemy.TakeDamage(Damage);
        }     
    }

    class Swordman : Warrior
    {
        private int _moveNumber = 0;
        private int _decreaseDamage = 8;

        public Swordman() : base("Мечник", 100, 15, 30) { }

        public override void DealDamage(Warrior Enemy)
        {
            Enemy.TakeDamage(Damage);
            Enemy.TakeDamage(Damage - _decreaseDamage);
        }      
    }

    class Thief : Warrior 
    {
        private int _chanceDodge = 40;
        public Thief() : base("Вор", 100, 20, 50) { }

        public override void TakeDamage(int damage)
        {
            Random random = new Random();
            int maxRandom = 100;
            bool isDodge = random.Next(0, maxRandom + 1) <= _chanceDodge;

            if (isDodge) 
            {
                Console.WriteLine($"{Name} укланился");
            }
            else
            {
                base.TakeDamage(damage);
            }
        }
    }

    class Barbarion : Warrior 
    {
        private int _powerDecreaseArmor = 5;

        public Barbarion() : base("Варвор", 100, 15, 50){}        
        
        public override void DealDamage(Warrior Enemy)
        { 
            Enemy.TakeDamage(Damage);
            Enemy.DecreaseArmor(_powerDecreaseArmor);           
        }
    }

    class Knigh : Warrior 
    {
        private int _moveNumber = 0;

        public Knigh() : base("Рыцарь", 100, 10, 80) { }

        public override void DealDamage(Warrior Enemy)
        {
            int attackPowerMove = 3;
            int attackBoost = 10;

            if (_moveNumber % attackPowerMove == 0)
            {
                Enemy.TakeDamage(Damage + attackBoost);
            }
            else
            {
                base.DealDamage(Enemy);
            }

            _moveNumber++;
        }
    }

    class Monarch : Warrior 
    {
        private int _powerHealing = 4;

        public Monarch() : base("Монарх", 100, 10, 80) { }

        public override void DealDamage(Warrior Enemy)
        {
            base.DealDamage(Enemy);
            Healing(_powerHealing);
        }
    }
}
