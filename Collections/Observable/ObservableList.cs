using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HM.Collections.Observable
{
    public class ObservableList<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler? PropertyChanged;
        
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
             * 若不同则说明位置发生变化，因此重新调整值 */
            var list = this.ToList();
            list.Sort(comparer);

            for (int i = 0; i < list.Count; i++)
            {
                if (comparer.Compare(this[i], list[i]) == 0)
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

        protected override void ClearItems()
        {
            base.ClearItems();
            OnPropertyChanged(nameof(Count));
        }
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnPropertyChanged(nameof(Count));
        }
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            OnPropertyChanged(nameof(Count));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
