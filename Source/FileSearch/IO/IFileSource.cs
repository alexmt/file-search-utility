using System;

namespace FileSearch.IO
{
    public interface IFileSource
    {
        object SyncRoot { get; }

        event Action<FileSourceUpdatedArgs> FileSourceUpdated;

        string[] GetSourceFiles();
    }
}