using Collections;

namespace HM.Collections.Observable
{
    /// <summary>
    /// Represent a collection that can raise event when item in the collection was added, removed or updated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObservableCollection<T>
    {
        event EventHandler<CollectionModifiedEventArgs<T>>? PreviewCollectionModified;
        event EventHandler<CollectionModifiedEventArgs<T>>? CollectionModified;

        NotificationMode NotificationMode { get; set; }
    }
}
