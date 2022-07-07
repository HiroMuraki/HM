namespace HM.Mathematics.Algorithm
{
    public interface ISolutionSearcher<T> where T : IComparable<T>
    {
        T FindSolution(T minValue, T maxValue, Predicate<T> predicate, FeasibleSolution feasibleSolution);
    }
}
