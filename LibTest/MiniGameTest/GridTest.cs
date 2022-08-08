using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HM.MiniGames;

namespace LibTest.MiniGameTest
{
    [TestClass]
    public class GridTest
    {
        [TestMethod]
        public void NormalTest()
        {
            Grid<int> grid = Grid<int>.Create(10, 5);
            for (int i = 0; i < 5; i++)
            {
                grid[i, i] = i;
            }
            System.Diagnostics.Debug.WriteLine($"{grid}"); // debug output
        }
    }
}
