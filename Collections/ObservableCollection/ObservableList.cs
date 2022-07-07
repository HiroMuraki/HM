using Collections;
using System.Collections.ObjectModel;

namespace HM.Collections.ObservableCollection
{
    [Serializable]
    public class ObservableList<T> : Collection<T>, IObservableCollection<T>
    {
        #region Events
        public event EventHandler<CollectionModifiedEventArgs<T>>? CollectionModified;
        public event EventHandler<CollectionModifiedEventArgs<T>>? PreviewCollectionModified;
        #endregion

        #region Properties
        public NotificationMode NotificationMode { get; set; } = NotificationMode.All;
        #endregion

        #region Overrided Methods
        protected override void InsertItem(int index, T item)
        {
            OnPreviewListChanged(CollectionModfiyAction.Add, item);
            base.InsertItem(index, item);
            OnListChanged(CollectionModfiyAction.Add, item);
        }
        protected override void ClearItems()
        {
            var clearedItems = Items.ToArray();
            OnPreviewListChanged(CollectionModfiyAction.Remove, clearedItems);
            base.ClearItems();
            OnListChanged(CollectionModfiyAction.Remove, clearedItems);
        }
        protected override void RemoveItem(int index)
        {
            var removedItem = Items[index];
            OnPreviewListChanged(CollectionModfiyAction.Remove, removedItem);
            base.RemoveItem(index);
            OnListChanged(CollectionModfiyAction.Remove, removedItem);
        }
        protected override void SetItem(int index, T item)
        {
            OnPreviewListChanged(CollectionModfiyAction.Update, item);
            base.SetItem(index, item);
            OnListChanged(CollectionModfiyAction.Update, Items[index]);
        }
        #endregion

        #region Helpers
        private void OnListChanged(CollectionModfiyAction action, T[] affactedItems)
        {
            if ((NotificationMode & NotificationMode.Normal) == NotificationMode.Normal)
            {
                CollectionModified?.Invoke(this, new(action, affactedItems));
            }
        }
        private void OnListChanged(CollectionModfiyAction action, T affactedItem)
        {
            OnListChanged(action, new T[] { affactedItem });
        }
        private void OnPreviewListChanged(CollectionModfiyAction action, T[] affactedItems)
        {
            if ((NotificationMode & NotificationMode.Preview) == NotificationMode.Preview)
            {
                PreviewCollectionModified?.Invoke(this, new(action, affactedItems));
            }
        }
        private void OnPreviewListChanged(CollectionModfiyAction action, T affactedItem)
        {
            OnPreviewListChanged(action, new T[] { affactedItem });
        }
        #endregion
    }
}
