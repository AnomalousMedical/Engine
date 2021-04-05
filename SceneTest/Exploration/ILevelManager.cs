using System;
using System.Threading.Tasks;

namespace SceneTest
{
    interface ILevelManager
    {

        event Action<ILevelManager> LevelChanged;

        bool ChangingLevels { get; }
        Level CurrentLevel { get; }
        bool IsPlayerMoving { get; }

        Task GoNextLevel();
        Task GoPreviousLevel();
        Task Restart();
        Task WaitForCurrentLevel();
        Task WaitForNextLevel();
        Task WaitForPreviousLevel();
    }
}