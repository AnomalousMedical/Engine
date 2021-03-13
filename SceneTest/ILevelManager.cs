using System.Threading.Tasks;

namespace SceneTest
{
    interface ILevelManager
    {
        bool ChangingLevels { get; }

        Task GoNextLevel();
        Task GoPreviousLevel();
        Task Initialize();
        Task WaitForCurrentLevel();
        Task WaitForNextLevel();
        Task WaitForPreviousLevel();
    }
}