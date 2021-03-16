using System;
using System.Threading.Tasks;

namespace SceneTest
{
    interface ILevelManager
    {

        event Action<LevelManager> LevelChanged;

        bool ChangingLevels { get; }
        Level CurrentLevel { get; }

        Task GoNextLevel();
        Task GoPreviousLevel();
        Task Initialize();
        Task WaitForCurrentLevel();
        Task WaitForNextLevel();
        Task WaitForPreviousLevel();
    }
}