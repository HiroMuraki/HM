using System.Collections.Immutable;

namespace HM.Serialization
{
    public class EntriesRequirements
    {
        enum EntryType
        {
            Directory,
            File
        }

        public ISet<string> RequiredDirectories { get; init; } = new HashSet<string>();
        public ISet<string> RequiredFiles { get; init; } = new HashSet<string>();

        public void CreateRequiredEntries()
        {
            // 优先创建文件夹
            foreach (var requiredDirectory in RequiredDirectories)
            {
                if (!Directory.Exists(requiredDirectory))
                {
                    Directory.CreateDirectory(requiredDirectory);
                }
            }
            foreach (var requiredFile in RequiredFiles)
            {
                if (!File.Exists(requiredFile))
                {
                    File.Create(requiredFile).Dispose();
                }
            }
        }
        public void GetDirectoriesStatus(out string[] existed, out string[] lacked)
        {
            GetStatusCore(out existed, out lacked, EntryType.Directory);
        }
        public bool CheckIfAllDirectoriesExisted(out string[] lacked)
        {
            GetStatusCore(out _, out lacked, EntryType.Directory);
            return !lacked.Any();
        }
        public bool CheckIfAnyDirectoriesExisted(out string[] existed)
        {
            GetStatusCore(out existed, out _, EntryType.Directory);
            return existed.Any();
        }
        public void GetFilesStatus(out string[] existed, out string[] lacked)
        {
            GetStatusCore(out existed, out lacked, EntryType.File);
        }
        public bool CheckIfAllFilesExisted(out string[] lacked)
        {
            GetStatusCore(out _, out lacked, EntryType.File);
            return !lacked.Any();
        }
        public bool CheckIfAnyFilesExsited(out string[] existed)
        {
            GetStatusCore(out existed, out _, EntryType.File);
            return existed.Any();
        }

        public EntriesRequirements()
        {

        }
        public EntriesRequirements(IEnumerable<string> requiredFiles, IEnumerable<string> requiredDirectories)
        {
            RequiredFiles = requiredFiles.ToImmutableHashSet();
            RequiredDirectories = requiredDirectories.ToImmutableHashSet();
        }

        private void GetStatusCore(out string[] existed, out string[] lacked, EntryType entryType)
        {
            var existedEntries = new List<string>();
            var lackedEntries = new List<string>();
            switch (entryType)
            {
                case EntryType.Directory:
                    foreach (var requiredEntry in RequiredDirectories)
                    {
                        if (Directory.Exists(requiredEntry))
                        {
                            existedEntries.Add(requiredEntry);
                        }
                        else
                        {
                            lackedEntries.Add(requiredEntry);
                        }
                    }
                    break;
                case EntryType.File:
                    foreach (var requiredEntry in RequiredFiles)
                    {
                        if (File.Exists(requiredEntry))
                        {
                            existedEntries.Add(requiredEntry);
                        }
                        else
                        {
                            lackedEntries.Add(requiredEntry);
                        }
                    }
                    break;
            }

            existed = existedEntries.ToArray();
            lacked = lackedEntries.ToArray();
        }
    }
}
