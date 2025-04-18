using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedRPG
{
    internal class Player
    {
        private string _name;
        private int _level;
        private int _maxHp;
        private int _currentHp;
        private int _attack;
        private int _exp;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public int Atk
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public int CurrentHp
        {
            get { return _currentHp; }
            set 
            { 
                _currentHp = value;
                if (_currentHp <= 0)
                {
                    Manager.Instance._isGameFail = true;
                }
            }
        }


        //레벨 업 경험치 테이블
        Dictionary<int, int> levelExpTable = new Dictionary<int, int>()
        {
            { 1, 0 },
            { 2, 20 },
            { 3, 30 },
            { 4, 40 },
            { 5, 50 },
            { 6, 60 },
        };

        public Player()
        {
            _level = 1;
            _maxHp = 30;
            _currentHp = _maxHp;
            _attack = 6;
            _exp = 0;
        }

        public void AddExp(int amount)
        {
            _exp += amount;
            if (_exp >= levelExpTable[Level+1])
            {
                _exp -= levelExpTable[Level+1];
                Level++;
                LevelUp();
            }
        }

        public void ShowPlayerInfo()
        {
            string infoStr = $"이름: {_name}\n레벨: {_level}\n체력: {_currentHp}\n공격력: {_attack}\n경험치: {_exp}";
            Console.WriteLine(infoStr);
        }

        public void ShowPlayerHp()
        {
            string infoStr = $"{_name}의 HP {_currentHp} / {_maxHp}";
            Console.WriteLine(infoStr);
        }

        public void LevelUp()
        {
            Console.WriteLine("[레벨 업!]");
            Console.WriteLine($"> 레벨 {_level} 달성!");
            Console.WriteLine($"> 최대 체력 +10, 공격력 +2 증가!\n");

            _maxHp += 10;
            _currentHp = _maxHp;
            _attack += 2;
        }

    }
}
