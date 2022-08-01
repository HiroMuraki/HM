namespace HM.Mathematics.Algorithm
{
    internal static class SolutionHelper
    {
        /// <summary>
        /// 使用二分法查找最大可行解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="predicate">测试函数</param>
        /// <param name="center">取中函数</param>
        /// <returns>最大可行解</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NoSolutionFoundException"></exception>
        private static T FindMaximumByBinary<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T, T> center) where T : IComparable<T>
        {
            if (maxValue.CompareTo(minValue) < 0)
            {
                throw new ArgumentException($"min value({minValue}) should be smaller than or equals to max value({maxValue})");
            }

            var leftBorder = minValue;
            var rightBorder = maxValue;
            var centerValue = maxValue;
            var lastCenterValue = centerValue;
            while (true)
            {
                if (predicate(centerValue))
                {
                    leftBorder = centerValue;
                }
                else
                {
                    if (centerValue.CompareTo(minValue) == 0)
                    {
                        throw new NoSolutionFoundException($"no solution found in range [{minValue}, {maxValue}]");
                    }
                    rightBorder = centerValue;
                }
                centerValue = center(leftBorder, rightBorder);
                if (centerValue.CompareTo(lastCenterValue) == 0)
                {
                    break;
                }
                else
                {
                    lastCenterValue = centerValue;
                }
            }

            return leftBorder;
        }
        /// <summary>
        /// 从最大值向最小值搜索最大可行解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="predicate">测试函数</param>
        /// <param name="decrease">递减函数</param>
        /// <returns>最大可行解</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NoSolutionFoundException"></exception>
        private static T FindMaximumFromMaxToMin<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T> decrease) where T : IComparable<T>
        {
            if (maxValue.CompareTo(minValue) < 0)
            {
                throw new ArgumentException($"min value({minValue}) should be smaller than or equals to max value({maxValue})");
            }

            var outputValue = maxValue;
            while (outputValue.CompareTo(minValue) >= 0)
            {
                if (predicate(outputValue))
                {
                    return outputValue;
                }
                outputValue = decrease(outputValue);
            }

            throw new NoSolutionFoundException($"no solution found in range [{minValue}, {maxValue}]");
        }
        /// <summary>
        /// 从最小值向最大值搜索最大可行解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="predicate">测试函数</param>
        /// <param name="increase">递减函数</param>
        /// <returns>最大可行解</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NoSolutionFoundException"></exception>
        private static T FindMaximumFromMinToMax<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T> increase) where T : IComparable<T>
        {
            if (maxValue.CompareTo(minValue) < 0)
            {
                throw new ArgumentException($"min value({minValue}) should be smaller than or equals to max value({maxValue})");
            }

            var outputValue = minValue;
            var nextValue = minValue;
            while (nextValue.CompareTo(maxValue) <= 0)
            {
                if (predicate(nextValue))
                {
                    outputValue = nextValue;
                    nextValue = increase(nextValue);
                }
                else
                {
                    break;
                }
            }

            if (nextValue.CompareTo(minValue) == 0)
            {
                throw new NoSolutionFoundException($"no solution found in range [{minValue}, {maxValue}]");
            }

            return outputValue;
        }

        /// <summary>
        /// 使用二分法查找最小可行解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="predicate">测试函数</param>
        /// <param name="center">取中函数</param>
        /// <returns最小可行解</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NoSolutionFoundException"></exception>
        private static T FindMinimumByBinary<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T, T> center) where T : IComparable<T>
        {
            if (maxValue.CompareTo(minValue) < 0)
            {
                throw new ArgumentException($"min value({minValue}) should be smaller than or equals to max value({maxValue})");
            }

            var leftBorder = minValue;
            var rightBorder = maxValue;
            var centerValue = minValue;
            var lastCenterValue = minValue;
            while (true)
            {
                if (predicate(centerValue))
                {
                    rightBorder = centerValue;
                }
                else
                {
                    leftBorder = centerValue;
                    if (leftBorder.CompareTo(maxValue) == 0)
                    {
                        throw new NoSolutionFoundException($"no solution found in range [{minValue}, {maxValue}]");
                    }
                }
                centerValue = center(leftBorder, rightBorder);
                if (centerValue.CompareTo(lastCenterValue) == 0)
                {
                    break;
                }
                else
                {
                    lastCenterValue = centerValue;
                }
            }

            return rightBorder;
        }
        /// <summary>
        /// 从最大值向最小值搜索最小可行解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="predicate">测试函数</param>
        /// <param name="decrease">递减函数</param>
        /// <returns>最小可行解</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NoSolutionFoundException"></exception>
        private static T FindMinimumFromMaxToMin<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T> decrease) where T : IComparable<T>
        {
            if (maxValue.CompareTo(minValue) < 0)
            {
                throw new ArgumentException($"min value({minValue}) should be smaller than or equals to max value({maxValue})");
            }

            var outputValue = maxValue;
            var nextValue = maxValue;
            while (outputValue.CompareTo(minValue) >= 0)
            {
                if (predicate(nextValue))
                {
                    outputValue = nextValue;
                    nextValue = decrease(nextValue);
                }
                else
                {
                    break;
                }
            }

            if (nextValue.CompareTo(maxValue) == 0)
            {
                throw new NoSolutionFoundException($"no solution found in range [{minValue}, {maxValue}]");
            }

            return outputValue;
        }
        /// <summary>
        /// 从最小值向最大值搜索最小可行解
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="predicate">测试函数</param>
        /// <param name="increase">递减函数</param>
        /// <returns>最小可行解</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NoSolutionFoundException"></exception>
        private static T FindMinimumFromMinToMax<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T> increase) where T : IComparable<T>
        {
            if (maxValue.CompareTo(minValue) < 0)
            {
                throw new ArgumentException($"min value({minValue}) should be smaller than or equals to max value({maxValue})");
            }

            var outputValue = minValue;
            while (outputValue.CompareTo(maxValue) <= 0)
            {
                if (predicate(outputValue))
                {
                    return outputValue;
                }
                outputValue = increase(outputValue);
            }

            throw new NoSolutionFoundException($"no solution found in range [{minValue}, {maxValue}]");
        }

        internal static T FindByBinary<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T, T> center, FeasibleSolution feasibleSolution) where T : IComparable<T>
        {
            switch (feasibleSolution)
            {
                case FeasibleSolution.Maximum:
                    return FindMaximumByBinary(minValue, maxValue, predicate, center);
                case FeasibleSolution.Minimum:
                    return FindMinimumByBinary(minValue, maxValue, predicate, center);
                default:
                    throw new ArgumentException("not supported feasible solution type");
            }
        }
        internal static T FindFromMaxToMin<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T> decrease, FeasibleSolution feasibleSolution) where T : IComparable<T>
        {
            switch (feasibleSolution)
            {
                case FeasibleSolution.Maximum:
                    return FindMaximumFromMaxToMin(minValue, maxValue, predicate, decrease);
                case FeasibleSolution.Minimum:
                    return FindMinimumFromMaxToMin(minValue, maxValue, predicate, decrease);
                default:
                    throw new ArgumentException("not supported feasible solution type");
            }
        }
        internal static T FindFromMinToMax<T>(T minValue, T maxValue, Predicate<T> predicate, Func<T, T> increase, FeasibleSolution feasibleSolution) where T : IComparable<T>
        {
            switch (feasibleSolution)
            {
                case FeasibleSolution.Maximum:
                    return FindMaximumFromMinToMax(minValue, maxValue, predicate, increase);
                case FeasibleSolution.Minimum:
                    return FindMinimumFromMinToMax(minValue, maxValue, predicate, increase);
                default:
                    throw new ArgumentException("not supported feasible solution type");
            }
        }
    }
}
