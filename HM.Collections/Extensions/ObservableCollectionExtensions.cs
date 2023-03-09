using System.Collections.ObjectModel;

namespace HM.Collections.Extensions;

public static class ObservableCollectionExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> self, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            self.Add(item);
        }
    }

    public static int RemoveIf<T>(this ObservableCollection<T> self, Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        int removed = 0;

        for (int i = 0; i < self.Count; i++)
        {
            if (predicate(self[i]))
            {
                self.RemoveAt(i);
                --i;
                removed++;
            }
        }

        return removed;
    }

    public static void ForEach<T>(this ObservableCollection<T> self, Action<T>? action)
    {
        if (action is null) return;

        foreach (var item in self)
        {
            action(item);
        }
    }

    public static void Sort<T>(this ObservableCollection<T> self, IComparer<T> comparer)
    {
        /* 转为为一个普通列表后，将该列表使用comparer排序
         * 将排序后的普通列表同自身的对应下标元素一一比较，
         * 若不同则说明位置发生变化，因此重新调整值 */
        var list = self.ToList();
        list.Sort(comparer);

        for (int i = 0; i < list.Count; i++)
        {
            if (comparer.Compare(self[i], list[i]) == 0)
            {
                self[i] = list[i];
            }
        }
    }

    public static void Sort<T>(this ObservableCollection<T> self) => self.Sort(Comparer<T>.Default);
}
