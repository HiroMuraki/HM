using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HM.Mathematics;

namespace LibTest.Mathematics
{
    [TestClass]
    public class MathematicsTest
    {
        [TestMethod]
        public void RationalNumberTest()
        {
            RationalNumber number1 = new RationalNumber(1, 5);
            RationalNumber number2 = new RationalNumber(1, 5);
            RationalNumber number3 = new RationalNumber(2, 5);

            Assert.IsTrue(number1 == number2);
            Assert.IsTrue(number1 != number3);
            Assert.IsTrue(number2 != number3);
            Assert.IsTrue(number1 + number2 == number3);
            Assert.IsTrue(number1 * number2 == new RationalNumber(1, 25));
            Assert.IsTrue(number1 + number2 + number3 + new RationalNumber(1, 5) == new RationalNumber(1));
            Assert.IsTrue(-number1 == new RationalNumber(-1, 5) && -number2 == new RationalNumber(1, -5));
        }
    }
}
