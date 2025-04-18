using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TurnBasedRPG
{
    public class MonsterData
    {
        public string _name;
        public int _maxHp;
        public int _currentHp;
        public int _attack;
        public int _rewardExp;
        public bool _isBoss;

        public MonsterData(string name, int maxHp, int attack, int rewardExp, bool isBoss)
        {
            this._name = name;
            this._maxHp = maxHp;
            this._currentHp = maxHp;
            this._attack = attack;
            this._rewardExp = rewardExp;
            this._isBoss = isBoss;
        }
    }

    public static class MonsterDatabase
    {
        public static readonly MonsterData[] DataArray = new MonsterData[]
        {
            new MonsterData("슬라임", 15, 3, 10, false),
            new MonsterData("고블린", 20, 4, 15, false),
            new MonsterData("늑대", 27, 5, 20, false),
            new MonsterData("해골 전사", 35, 6, 25, false),
            new MonsterData("불정령", 42, 7, 40, false),
            new MonsterData("암흑의 기사", 65, 10, 100, true)
        };
    }

    internal class Monster
    {
        private int _id;
        public MonsterData _data => MonsterDatabase.DataArray[_id];

        public bool IsDead => CurrentHp == 0;

        public int Id
        {
            get { return _id; }
            set { _id = value;  }
        }

        public int CurrentHp
        {
            get { return _data._currentHp; }
            set 
            { 
                _data._currentHp = value;
                if( _data._currentHp < 0 )
                {
                    _data._currentHp = 0;
                }
            }
        }

        public Monster()
        {
            _id = 0;
        }

        public void ShowMonsterHp()
        {
            string infoStr = $"{_data._name}의 HP {_data._currentHp} / {_data._maxHp}";
            Console.WriteLine(infoStr);
        }

        public void SpawnMonster(int dungeonLevel)
        {
            int monsterId = Math.Min(dungeonLevel, MonsterDatabase.DataArray.Length - 1);
            _id = monsterId;
            _data._currentHp = _data._maxHp; // 체력 초기화
        }
    }
}
