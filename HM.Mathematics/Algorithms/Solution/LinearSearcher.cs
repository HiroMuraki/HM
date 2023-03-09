namespace HM.Mathematics.Algorithms.Solution
{
    public class LinearSearcher<T> : ISolutionSearcher<T> where T : IComparable<T>
    {
        public bool ReverseSearch
        {
            get
            {
                return _reverseSearch;
            }
            set
            {
                _reverseSearch = value;
            }
        }
        public LinearSearcher(Func<T, T> iter)
        {
            _iter = iter;
        }

        public T FindSolution(T minValue, T maxValue, Predicate<T> predicate, FeasibleSolution feasibleSolution)
        {
            return FindSolution(minValue, maxValue, predicate, feasibleSolution, _reverseSearch);
        }
        public T FindSolution(T minValue, T maxValue, Predicate<T> predicate, FeasibleSolution feasibleSolution, bool reverseSearch)
        {
            if (reverseSearch)
            {
                return SolutionHelper.FindFromMaxToMin(minValue, maxValue, predicate, _iter, feasibleSolution);
            }
            else
            {

                return SolutionHelper.FindFromMinToMax(minValue, maxValue, predicate, _iter, feasibleSolution);
            }
        }

        private readonly Func<T, T> _iter;
        private bool _reverseSearch;
    }
}
