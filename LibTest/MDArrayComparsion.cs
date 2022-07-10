namespace LibTest
{
    class MDArrayComparsion<T>
    {
        public static MDArrayComparsion<T> Default { get; } = new();

        public IComparer<T> Comparer { get; } = Comparer<T>.Default;

        public bool TestEquals(T[,] left, T[,] right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left.GetLength(0) != right.GetLength(0)) return false;
            if (left.GetLength(1) != right.GetLength(1)) return false;

            for (int row = 0; row < left.GetLength(0); row++)
            {
                for (int col = 0; col < left.GetLength(1); col++)
                {
                    if (Comparer.Compare(left[row, col], right[row, col]) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public bool TestEquals(T[][] left, T[][] right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left.Length != right.Length) return false;
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i].Length != right[i].Length) return false;
            }

            for (int row = 0; row < left.Length; row++)
            {
                for (int col = 0; col < right.Length; col++)
                {
                    if (Comparer.Compare(left[row][col], right[row][col]) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public bool TestEquals(T[][] left, T[,] right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left.Length != right.GetLength(0)) return false;
            if (left.Any(row => row.Length != right.GetLength(1))) return false;

            for (int row = 0; row < right.GetLength(0); row++)
            {
                for (int col = 0; col < right.GetLength(1); col++)
                {
                    if (Comparer.Compare(left[row][col], right[row, col]) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public bool TestEquals(T[,] left, T[][] right)
        {
            return TestEquals(right, left);
        }
    }
}
