
#if PREVIEW_FEATURE
namespace HM.Utility.Mathematics {
    public class Matrix<T> : IEquatable<Matrix<T>>, IFormattable where T : INumber<T> {
#region 属性
        /// <summary>
        /// 表示行数
        /// </summary>
        public int RowSize => _elements.GetLength(0);
        /// <summary>
        /// 表示列数
        /// </summary>
        public int ColumnSize => _elements.GetLength(1);
        /// <summary>
        /// 用于获取元素的索引符
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public T this[int row, int col] {
            get {
                return _elements[row, col];
            }
            set {
                _elements[row, col] = value;
            }
        }
#endregion

#region 构造函数
        /// <summary>
        /// 建立一个rowSize x columnSize，元素值均为initializeValue的矩阵
        /// </summary>
        /// <param name="rowSize">行数</param>
        /// <param name="columnSize">列数</param>
        /// <param name="initializeValue">矩阵元素初值</param>
        public Matrix(int rowSize, int columnSize, T initializeValue) {
            _elements = new T[rowSize, columnSize];
            for (int row = 0; row < RowSize; row++) {
                for (int col = 0; col < ColumnSize; col++) {
                    _elements[row, col] = initializeValue;
                }
            }
        }
        /// <summary>
        /// 建立一个rowSize x columnSize的矩阵
        /// </summary>
        /// <param name="rowSize">行数</param>
        /// <param name="columnSize">列数</param>
        public Matrix(int rowSize, int columnSize) : this(rowSize, columnSize, T.Zero) { }
        /// <summary>
        /// 根据二维数组创建矩阵
        /// </summary>
        /// <param name="elements">二维数组</param>
        public Matrix(T[,] elements) {
            int rowSize = elements.GetLength(0);
            int colSize = elements.GetLength(1);
            _elements = new T[rowSize, colSize];
            for (int row = 0; row < RowSize; row++) {
                for (int col = 0; col < ColumnSize; col++) {
                    _elements[row, col] = elements[row, col];
                }
            }
        }
#endregion

#region 公共静态方法
        /// <summary>
        /// 获得一个rowSize x columnSize的单位矩阵
        /// </summary>
        /// <param name="rowSize">行</param>
        /// <param name="columnSize">列</param>
        /// <returns></returns>
        public static Matrix<TElement> Identity<TElement>(int rowSize, int columnSize) where TElement : INumber<TElement> {
            var matrix = new Matrix<TElement>(rowSize, columnSize);
            for (int row = 0; row < rowSize; row++) {
                for (int col = 0; col < columnSize; col++) {
                    if (row == col) {
                        matrix[row, col] = TElement.One;
                    }
                    else {
                        matrix[row, col] = TElement.Zero;
                    }
                }
            }
            return matrix;
        }
#endregion

#region 公共方法
        /// <summary>
        /// 获取指定行的元素
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public T[] TakeRow(int row) {
            var targetRow = new T[ColumnSize];
            for (int column = 0; column < ColumnSize; column++) {
                targetRow[column] = _elements[row, column];
            }
            return targetRow;
        }
        /// <summary>
        /// 获取指定列的元素
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public T[] TakeColmun(int column) {
            var targetColumn = new T[RowSize];
            for (int row = 0; row < RowSize; row++) {
                targetColumn[row] = _elements[row, column];
            }
            return targetColumn;
        }
        /// <summary>
        /// 获取代数余子式
        /// </summary>
        /// <param name="ignoreRow"></param>
        /// <param name="ignoreColumne"></param>
        /// <returns></returns>
        public Matrix<T> GetAijMatrix(int ignoreRow, int ignoreColumne) {
            if (RowSize <= 1 || ColumnSize <= 1) {
                throw new MathematicException($"无法获取行数为{RowSize}, 列数为{ColumnSize}的矩阵的代数余子式");
            }
            int rowSize = RowSize - 1;
            int columnSize = ColumnSize - 1;
            var result = new Matrix<T>(rowSize, columnSize);

            int rRow = 0;
            for (int row = 0; row < RowSize; row++) {
                if (row == ignoreRow) {
                    continue;
                }
                int rColumn = 0;
                for (int col = 0; col < ColumnSize; col++) {
                    if (col == ignoreColumne) {
                        continue;
                    }
                    result[rRow, rColumn] = _elements[row, col];
                    ++rColumn;
                }
                ++rRow;
            }

            return result;
        }
        /// <summary>
        /// 获取行列式的值
        /// </summary>
        /// <returns></returns>
        public T GetDeterminate() {
            if (RowSize != ColumnSize) {
                throw new MathematicException($"行为{RowSize}，列为{ColumnSize}的矩阵无法计算行列式");
            }
            return GetDeterminate(this);
        }
#endregion

#region 算术运算符重载
        public static Matrix<T> operator -(Matrix<T> matrix) {
            var result = new Matrix<T>(matrix.RowSize, matrix.ColumnSize);
            for (int row = 0; row < result.RowSize; row++) {
                for (int col = 0; col < result.ColumnSize; col++) {
                    result[row, col] = -matrix[row, col];
                }
            }
            return result;
        }
        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right) {
            if (left.RowSize != right.RowSize || left.ColumnSize != right.ColumnSize) {
                throw new MathematicException($"相加的矩阵行列数应相等 ({left.RowSize},{right.RowSize})!=({left.ColumnSize},{right.ColumnSize})");
            }
            int rowSize = left.RowSize;
            int columnSize = left.ColumnSize;

            var result = new Matrix<T>(rowSize, columnSize);
            for (int row = 0; row < rowSize; row++) {
                for (int col = 0; col < columnSize; col++) {
                    result[row, col] = left[row, col] + right[row, col];
                }
            }
            return result;
        }
        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right) {
            return left + (-right);
        }
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right) {
            if (left.RowSize != right.ColumnSize) {
                throw new MathematicException($"被乘矩阵的行数应等于乘数矩阵的列数 {left.RowSize} != {right.ColumnSize}");
            }
            int rowSize = left.RowSize;
            int columnSize = right.ColumnSize;
            int calCount = left.ColumnSize;

            var result = new Matrix<T>(rowSize, columnSize);
            for (int row = 0; row < rowSize; row++) {
                for (int col = 0; col < columnSize; col++) {
                    var element = T.Zero;
                    for (int i = 0; i < calCount; i++) {
                        element += left[row, i] * right[i, col];
                    }
                    result[row, col] = element;
                }
            }

            return result;
        }
        public static Matrix<T> operator *(T n, Matrix<T> matrix) {
            var result = new Matrix<T>(matrix.RowSize, matrix.ColumnSize);
            for (int row = 0; row < matrix.RowSize; row++) {
                for (int col = 0; col < matrix.ColumnSize; col++) {
                    result[row, col] = n * matrix[row, col];
                }
            }
            return result;
        }
        public static Matrix<T> operator *(Matrix<T> matrx, T n) {
            return n * matrx;
        }
#endregion

#region 逻辑运算符重载
        public static bool operator ==(Matrix<T> left, Matrix<T> right) {
            if (left.RowSize != right.RowSize || left.ColumnSize != right.ColumnSize) {
                return false;
            }
            int rowSize = left.RowSize;
            int columnSize = right.ColumnSize;
            for (int row = 0; row < rowSize; row++) {
                for (int col = 0; col < columnSize; col++) {
                    if (left[row, col] != right[row, col]) {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool operator !=(Matrix<T> left, Matrix<T> right) {
            return !(left == right);
        }
#endregion

#region 强制转换运算符
        /// <summary>
        /// 二维数组转矩阵
        /// </summary>
        /// <param name="elements"></param>
        public static explicit operator Matrix<T>(T[,] elements) {
            return new Matrix<T>(elements);
        }
        /// <summary>
        /// 矩阵转二维数组
        /// </summary>
        /// <param name="matrix"></param>
        public static explicit operator T[,](Matrix<T> matrix) {
            var result = new T[matrix.RowSize, matrix.ColumnSize];
            for (int row = 0; row < matrix.RowSize; row++) {
                for (int col = 0; col < matrix.ColumnSize; col++) {
                    result[row, col] = matrix._elements[row, col];
                }
            }
            return result;
        }
#endregion

#region 矩阵行变化
        /// <summary>
        /// 转置矩阵
        /// </summary>
        public Matrix<T> Trans() {
            int rowSize = ColumnSize;
            int columnSize = RowSize;
            var result = new Matrix<T>(rowSize, columnSize);
            for (int row = 0; row < rowSize; row++) {
                for (int col = 0; col < columnSize; col++) {
                    result[row, col] = _elements[col, row];
                }
            }
            return result;
        }
        /// <summary>
        /// 行变换
        /// </summary>
        /// <param name="xRow"></param>
        /// <param name="yRow"></param>
        public void RowSwitch(int xRow, int yRow) {
            for (int col = 0; col < ColumnSize; col++) {
                Swap(ref _elements[xRow, col], ref _elements[yRow, col]);
            }
        }
        /// <summary>
        /// 倍法变换
        /// </summary>
        /// <param name="n"></param>
        /// <param name="row"></param>
        public void MRSwitch(T n, int row) {
            if (n == T.Zero) {
                throw new MathematicException("倍法变换中倍数不能为0");
            }
            for (int col = 0; col < ColumnSize; col++) {
                _elements[row, col] *= n;
            }
        }
        /// <summary>
        /// 消法变换
        /// </summary>
        /// <param name="n">消行倍数</param>
        /// <param name="sourceRow">消行</param>
        /// <param name="targetRow">被消行</param>
        public void DRSwitch(T n, int sourceRow, int targetRow) {
            for (int col = 0; col < ColumnSize; col++) {
                _elements[targetRow, col] += n * _elements[sourceRow, col];
            }
        }
#endregion

        /// <summary>
        /// 字符串格式化
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return ToString(" ", null);
        }
        /// <summary>
        /// IFormattable接口格式化
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string? format, IFormatProvider? formatProvider) {
            if (string.IsNullOrEmpty(format)) {
                return ToString();
            }
            var sb = new StringBuilder();
            for (int row = 0; row < RowSize; row++) {
                string currentRow = string.Join(format, TakeRow(row));
                sb.Append($"{currentRow}\n");
            }
            return sb.ToString();
        }
        public override int GetHashCode() {
            return _elements.GetHashCode();
        }
        public override bool Equals(object? obj) {
            if (obj is null) {
                return false;
            }
            if (obj.GetType() != typeof(Matrix<T>)) {
                return false;
            }
            return Equals((Matrix<T>)obj);
        }
        public bool Equals(Matrix<T>? other) {
            if (other is null) {
                return false;
            }
            return this == other;
        }
        public Matrix<T> GetDeepCopy() {
            var copy = new Matrix<T>(RowSize, ColumnSize);
            for (int row = 0; row < RowSize; row++) {
                for (int col = 0; col < ColumnSize; col++) {
                    copy._elements[row, col] = _elements[row, col];
                }
            }
            return copy;
        }

#region 私有辅助方法
        private readonly T[,] _elements;//内部储存的二位数组
        /// <summary>
        /// 交换两个元素
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(ref T x, ref T y) {
            var t = x;
            x = y;
            y = t;
        }
        /// <summary>
        /// 递归计算行列式的值
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static T GetDeterminate(Matrix<T> matrix) {
            if (matrix.RowSize == 2 && matrix.ColumnSize == 2) {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            var value = T.Zero;
            for (int col = 0; col < matrix.ColumnSize; col++) {
                var temp = matrix[0, col] * matrix.GetAijMatrix(0, col).GetDeterminate();
                value += col % 2 == 0 ? temp : -temp;
            }
            return value;
        }
#endregion
    }
}
#endif