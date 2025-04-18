using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TurnBasedRPG
{


    internal class SceneManager
    {
        private readonly Dictionary<SceneType, Action> _sceneMap;

        public SceneManager()
        {
            _sceneMap = new Dictionary<SceneType, Action>
            {
                { SceneType.GameStartScene, LoadGameStartScene },
                { SceneType.GameMainScene, LoadGameMainScene },
                { SceneType.PlayerInfoScene, LoadPlayerInfoScene },
                { SceneType.DunjeonScene, LoadDunjeonScene },
                { SceneType.GameClearScene, LoadGameClearScene },
            };
        }

        public void LoadScene(SceneType type)
        {
            Console.Clear();

            if(_sceneMap.TryGetValue(type, out var action))
            {
                action();
            }
        }


        void LoadGameStartScene()
        {
            Console.WriteLine("[던전 어드벤처 게임을 시작하겠습니다.]\n");
            Console.Write("이름을 입력하세요 >> ");
            string name = Console.ReadLine();
            Manager.Instance.Player.Name = name;
            LoadScene(SceneType.GameMainScene);
        }

        void LoadGameMainScene()
        {
            Console.WriteLine("====== 메인 메뉴 ======\n");
            Console.WriteLine("1. 캐릭터 정보 보기");
            Console.WriteLine("2. 던전 입장");
            Console.WriteLine("3. 게임 종료\n");

            Console.Write("선택 >> ");
            int input = int.TryParse(Console.ReadLine(), out int result) ? result : -1;

            switch(input)
            {
                case 1:
                    LoadScene(SceneType.PlayerInfoScene);
                    break;
                case 2:
                    LoadScene(SceneType.DunjeonScene);
                    break;
                case 3:
                    //게임 종료
                    break;
                default:
                    LoadScene(SceneType.GameMainScene);
                    break;
            }
        }

        void LoadPlayerInfoScene()
        {
            Console.WriteLine("====== 캐릭터 정보 보기 ======\n");
            Manager.Instance.Player.ShowPlayerInfo();
            Console.WriteLine("\n1. 메인 메뉴로 돌아가기\n");

            Console.Write("선택 >> ");
            int input = int.TryParse(Console.ReadLine(), out int result) ? result : -1;

            switch(input)
            {
                case 1:
                    LoadScene(SceneType.GameMainScene);
                    break;
                default:
                    LoadScene(SceneType.PlayerInfoScene);
                    break;
            }
        }

        void LoadDunjeonScene()
        {
            Console.WriteLine("[던전에 입장합니다...]\n");

            Console.WriteLine("[전투 시작!]");
            Manager.Instance.Player.ShowPlayerHp();
            Manager.Instance.Monster.ShowMonsterHp();
            Console.WriteLine();

            //player turn
            //monster turn
            while(true)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine($"[현재 던전 레벨: {Manager.Instance.DungeonLevel + 1}]\n");
                //보스 몬스터 등장
                if(Manager.Instance.Monster._data._isBoss)
                {
                    Console.WriteLine("[보스 몬스터 등장!]");
                    Console.WriteLine($"[{Manager.Instance.Monster._data._name}]: \"네가 여기까지 올 줄은 몰랐군... 하지만 여기서 끝이다!\"\n");
                }

                HandlePlayerTurn();
                Console.WriteLine();

                if (Manager.Instance._isGameClear)
                {
                    return;
                }

                HandleMonsterTurn();
                Console.WriteLine();
            }
        }

        void LoadGameClearScene()
        {
            Console.WriteLine("최종 보스를 처치했습니다!");
            Console.WriteLine("던전 어드벤처 클리어!\n");
            Console.WriteLine($"축하합니다, {Manager.Instance.Player.Name} 님");
        }

        private void HandlePlayerTurn()
        {
            Console.WriteLine($"[{Manager.Instance.Player.Name}의 턴]");
            Console.WriteLine("1. 공격하기");
            Console.WriteLine("2. 도망치기");

            Console.Write("선택 >> ");
            int input = int.TryParse(Console.ReadLine(), out int result) ? result : -1;
            if (input == 2)
            {
                LoadScene(SceneType.GameMainScene);
            }

            Console.WriteLine($"\n> {Manager.Instance.Player.Name}가 {Manager.Instance.Monster._data._name}을 공격했다! ({Manager.Instance.Player.Atk} 데미지)");
            //monster hp 깎는 함수
            Manager.Instance.Monster.CurrentHp -= Manager.Instance.Player.Atk;
            Manager.Instance.Monster.ShowMonsterHp();

            //몬스터가 죽었는지 확인
            CheckMonsterDead();
        }

        private void HandleMonsterTurn()
        {
            Console.WriteLine($"[{Manager.Instance.Monster._data._name}의 턴]");
            Console.WriteLine($"> {Manager.Instance.Monster._data._name}이 공격했다! ({Manager.Instance.Monster._data._attack} 데미지)");
            //player hp 깎는 함수
            Manager.Instance.Player.CurrentHp -= Manager.Instance.Monster._data._attack;
            Manager.Instance.Player.ShowPlayerHp();
        }

        private void CheckMonsterDead()
        {
            if (!Manager.Instance.Monster.IsDead)
                return;


            Console.WriteLine("\n------------------------------------------------------");
            Console.WriteLine("[전투 종료!]");
            Console.WriteLine($"{Manager.Instance.Monster._data._name}을 처치했습니다!");
            Console.WriteLine($"+ 경험치 {Manager.Instance.Monster._data._rewardExp} 획득\n");
            Manager.Instance.Player.AddExp(Manager.Instance.Monster._data._rewardExp);

            if (Manager.Instance.Monster._data._isBoss)
            {
                Manager.Instance._isGameClear = true;
                LoadScene(SceneType.GameClearScene);
                return;
            }

            Console.WriteLine("1. 돌아가기");
            Console.WriteLine("2. 다음 던전으로 넘어가기");

            Manager.Instance.DungeonLevel++;


            Console.Write("선택 >> ");
            int input = int.TryParse(Console.ReadLine(), out int result) ? result : -1;
            if (input == 2)
            {
               LoadScene(SceneType.DunjeonScene);
            }

           LoadScene(SceneType.GameMainScene);
        }
    }
}
