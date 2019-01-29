using System;
using FileBackup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace FileBackupTests
{
    [TestClass]
    public class WatcherTests
    {
        [TestMethod]
        public void FileCreateLocally()
        {
            // Create a file in all watched directories
        }
        [TestMethod]
        public void FileEditeLocally()
        {
        }
    }

    [TestClass]
    public class BackupTests
    {
        [TestMethod]
        public void ZipFiles()
        {
        }

        [TestMethod]
        public void BackupFiles()
        {
        }

        [TestMethod]
        public void RemoveFiles()
        {
        }

        [TestMethod]
        public void RetrieveFiles()
        {
        }
    }

    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void FindBackupFileInDB()
        {
        }

        [TestMethod]
        public void AddBackupFileToDB()
        {
        }

        [TestMethod]
        public void RemoveBackupFileFromDB()
        {
        }
    }
}
