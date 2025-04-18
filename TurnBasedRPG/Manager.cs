using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace TurnBasedRPG
{
    internal class Manager
    {
        private static Manager _instance;
        public static Manager Instance => _instance ??= new Manager();

        public SceneManager Scene { get; private set; }
        public Player Player { get; private set; }
        public Monster Monster { get; private set; }

        private int _dungeonLevel = 0;
        public int DungeonLevel
        {
            get { return _dungeonLevel; }
            set 
            { 
                _dungeonLevel = value;
                //몬스터 레벨 증가
                Manager.Instance.Monster.SpawnMonster(Manager.Instance.DungeonLevel);
            }
        }

        public bool _isGameClear;
        public bool _isGameFail;

        public void Init()
        {
            Player = new Player();
            Monster = new Monster();
            Scene = new SceneManager();

            _isGameClear = false;
        }
    }
}
