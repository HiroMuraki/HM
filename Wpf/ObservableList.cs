using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HM.Wpf
{
    public class ObservableList<T> : ObservableCollection<T> where T : class
    {
        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
        public void ForEach(Action<T>? action)
        {
            if (action is null) return;

            foreach (var item in this)
            {
                action(item);
            }
        }
        public void Sort(IComparer<T> comparer)
        {
            /* 转为为一个普通列表后，将该列表使用comparer排序
             * 将排序后的普通列表同自身的对应下标元素一一比较，
             * 若引用不同则说明位置发生变化，因此重新调整值
             * 因此这一原因，泛型参数T必须是引用类型 */
            var list = this.ToList();
            list.Sort(comparer);

            for (int i = 0; i < list.Count; i++)
            {
                if (!ReferenceEquals(this[i], list[i]))
                {
                    this[i] = list[i];
                }
            }
        }
        public void Sort()
        {
            Sort(Comparer<T>.Default);
        }
        public void RemoveIf(Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            for (int i = 0; i < Count; i++)
            {
                if (predicate(this[i]))
                {
                    RemoveAt(i);
                    --i;
                }
            }
        }

        public ObservableList() { }
        public ObservableList(IEnumerable<T> items) : base(items) { }
    }
}
