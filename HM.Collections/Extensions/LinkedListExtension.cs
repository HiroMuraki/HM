namespace HM.Collections.Extensions
{
    public static class LinkedListExtension
    {
        public static LinkedList<T> RemoveIf<T>(this LinkedList<T> self, Predicate<T> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            var node = self.First;

            while (node is not null)
            {
                var next = node.Next;

                if (predicate(node.Value))
                {
                    self.Remove(node);
                }

                node = next;
            }

            return self;
        }
    }
}
