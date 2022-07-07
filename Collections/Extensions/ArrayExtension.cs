namespace HM.Collections.Extensions
{
    public static class ArrayExtension
    {
        public static T?[,] ToPhalanx<T>(this T?[] self)
        {
            int sideLen = (int)Math.Sqrt(self.Length);
            if (sideLen * sideLen != self.Length)
            {
                throw new ArgumentException($"Unable to cast array with len = {self.Length} to phalanx");
            }

            var phalanx = new T?[sideLen, sideLen];
            for (int row = 0; row < sideLen; row++)
            {
                for (int col = 0; col < sideLen; col++)
                {
                    phalanx[row, col] = self[row * sideLen + col];
                }
            }

            return phalanx;
        }
        public static T?[,] To2DArray<T>(this T?[] self, int rowSize, int columnSize)
        {
            if (self.Length != rowSize * columnSize)
            {
                throw new ArgumentException("Require row size * column size == size of array");
            }
            var twoDArray = new T?[rowSize, columnSize];
            for (int row = 0; row < rowSize; row++)
            {
                int yOffset = row * columnSize;
                for (int col = 0; col < columnSize; col++)
                {
                    twoDArray[row, col] = self[yOffset + col];
                }
            }
            return twoDArray;
        }
        public static void CopyFrom<T>(this T?[] self, T?[] sourceArray)
        {
            self.CopyFrom(sourceArray, 0);
        }
        public static void CopyFrom<T>(this T?[] self, T?[] sourceArray, int fromPos)
        {
            Array.Copy(sourceArray, 0, self, fromPos, sourceArray.Length);
        }
    }
}
