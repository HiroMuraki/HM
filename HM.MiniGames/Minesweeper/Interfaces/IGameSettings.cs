﻿namespace HM.MiniGames.Minesweeper.Interfaces
{
    public interface IGameSettings
    {
        /// <summary>
        /// 游戏宽度
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// 游戏高度
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// 生成的雷数
        /// </summary>
        public int MineCount { get; }
    }
}
