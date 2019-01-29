using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Windows.Input;

namespace FileBackup
{
    class FileBackupGeneral
    {

        FileBackupSQLEntities fileBackupEntitesContext = new FileBackupSQLEntities(); // Can/should these be more global to the entire project?

        /// <summary>
        /// Takes the given Files, zips them to compress
        /// </summary>
        /// <param name="filesToZipTogether">List of path files to be zipped up together</param>
        /// <returns>DateTime of zip, file name </returns> 
        public string ZipFiles(List<string> filesToZipTogether)
        {
            string backupPath = "C:\\Private\\backup";
            string invalidFiles = " ";

            // Quick check to see if backup directory is available to write to
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(backupPath);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Sorry, the backup directory is not accessible at this time.");
            }

            // Name and zip files to backup directory
            string zipName = backupPath + "\\" + Environment.UserName + Guid.NewGuid().ToString() + ".zip";

            // Zip them up
            ZipArchive zip = ZipFile.Open(zipName, ZipArchiveMode.Create);
            foreach (string file in filesToZipTogether)
            {
                if (File.Exists(file) == true)
                {
                    zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                }
                else
                {
                    invalidFiles = invalidFiles + " " + file;
                }
            }
            zip.Dispose();

            if (invalidFiles != " ")
            {
                // Warn user if things weren't able to be added to the zip file
                MessageBox.Show("Sorry, the following files weren't able to be backed up" + invalidFiles + ".");
                return "";
            }
            else
            {
                return zipName;
            }
        }

        public void BackupFiles(object source, ExecutedRoutedEventArgs e)
        {

            // Get files from the DB that we know are ready to be backed up - zip them for storage 
            List<string> filesToBackup = new List<string>();
            foreach (var file in fileBackupEntitesContext.FilesUpdatedOrAddeds)
            {
                filesToBackup.Add(file.FileFullNamePath);                
            }
            string storageLocation = ZipFiles(filesToBackup);

            // Go through again and push the information (including storageLocation) to the database
            foreach (var file in fileBackupEntitesContext.FilesUpdatedOrAddeds)
            {
                string userLocalFileName = Path.GetFileName(file.FileFullNamePath);

                // See if this file has been backed up before & update the entry in the main section & create a new history log
                //FileBackupMain fileBackupEntry = new FileBackupMain();
                FileBackupMain fileBackupMain = fileBackupEntitesContext.FileBackupMains.FirstOrDefault(x => x.FileName == userLocalFileName);

                if (fileBackupMain == null)
                {
                    fileBackupMain = new FileBackupMain();
                    fileBackupMain.FileName = userLocalFileName;
                    fileBackupMain.FileLastEditedDateTime = file.FileChangeDateTime;

                    fileBackupEntitesContext.FileBackupMains.Add(fileBackupMain);
                }


                // Create new history Log
                FileBackupHistory newBackupHistory = new FileBackupHistory();
                newBackupHistory.FileHistoryBackupDateTime = DateTime.Now;
                newBackupHistory.FileHistoryDescription = "Test String For Now";
                newBackupHistory.FileHistoryEditedDateTime = file.FileChangeDateTime;
                newBackupHistory.FileHistoryOriginPath = file.FileFullNamePath;
                newBackupHistory.FileHistoryUserName = Environment.UserName;
                newBackupHistory.FileHistoryZipPathName = storageLocation;
                fileBackupMain.FileBackupHistories.Add(newBackupHistory);


            }

            fileBackupEntitesContext.SaveChanges();
            // Remove files from the `potential to backup`  temporary database 
            var objCtx = ((IObjectContextAdapter)fileBackupEntitesContext).ObjectContext;
            objCtx.ExecuteStoreCommand("TRUNCATE TABLE [FilesUpdatedOrAdded]");


        }


        public void RemoveFiles()
        {
        }

        public void RetrieveFiles()
        {
        }

        public void FindBackupFileInDB()
        {
        }

        public void AddBackupFileToDB()
        {
        }

        public void RemoveBackupFileFromDB()
        {
        }

        public void Create_New_History()
        {

        }

    }
}
