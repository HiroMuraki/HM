﻿namespace HM.Mathematics.Algorithm
{
    public class BinarySearcher<T> : ISolutionSearcher<T> where T : IComparable<T>
    {
        public BinarySearcher(Func<T, T, T> center)
        {
            _center = center;
        }

        public T FindSolution(T minValue, T maxValue, Predicate<T> predicate, FeasibleSolution feasibleSolution)
        {
            return SolutionHelper.FindByBinary<T>(minValue, maxValue, predicate, _center, feasibleSolution);
        }

        private readonly Func<T, T, T> _center;
    }
}
