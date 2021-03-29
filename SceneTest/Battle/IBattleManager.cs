﻿using Engine.Platform;

namespace SceneTest
{
    interface IBattleManager
    {
        bool Active { get; }

        void SetActive(bool active);
        void SetupBattle();
        void UpdateGui(Clock clock);
    }
}