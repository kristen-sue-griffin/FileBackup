using System;
using System.Linq;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FileBackup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        FileSystemWatcher Watcher;
        FileBackupSQLEntities fileBackupEntitesContext = new FileBackupSQLEntities();

        // Create the startup window  & the sys tray icon    
        MainWindow FileBackupWindow = new MainWindow();
        NotifyIcon trayIcon = new NotifyIcon();


        /// <summary>
        /// Startup function for watching and adding files to FilesUpdatedOrAdded database
        /// </summary>
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            // Run the test for files that have changed since being closed

            // Start the filewatcher
            Watcher = new FileSystemWatcher("C:/WildWest");
            Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.CreationTime
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            Watcher.IncludeSubdirectories = true;
            Watcher.Filter = "*.*";
            Watcher.EnableRaisingEvents = true;
            Watcher.Changed += new FileSystemEventHandler(FileChanged);
            //Watcher.Created += new FileSystemEventHandler(FileCreated);

            // System Tray Notification
            trayIcon.Icon = new Icon(@"C:\Users\kritt_000\source\repos\FileBackup\FileBackup.ico");
            trayIcon.Visible = true;
            trayIcon.ShowBalloonTip(5000, "Automatic File Backup", "Backups files in zips for you at regular intervals", System.Windows.Forms.ToolTipIcon.Info);
            trayIcon.Click += trayIcon_Click;

            // show main window            
            FileBackupWindow.Show();
        }

        /// <summary>
        /// FileSystemWatcher file change function
        /// </summary>
        private void FileChanged(object source, FileSystemEventArgs e)
        {
            System.Windows.MessageBox.Show("File Changed: " + e.FullPath + " " + e.ChangeType);

            // Add to the FilesUpdatedOrAdded table. This table will be cleared when the update is zipped and stored.
            FilesUpdatedOrAdded testFileExists = fileBackupEntitesContext.FilesUpdatedOrAddeds.FirstOrDefault(x => x.FileFullNamePath == e.FullPath);
            if (testFileExists == null)
            {
                FilesUpdatedOrAdded file = new FilesUpdatedOrAdded();
                file.FileFullNamePath = e.FullPath;
                file.FileChangeType = 0;
                file.FileChangeDateTime = DateTime.Now;

                fileBackupEntitesContext.FilesUpdatedOrAddeds.Add(file);
                fileBackupEntitesContext.SaveChanges();

                // Update the UI - Do I need a check to make sure window exists for this? Can I decouple this as in theory the main window maybe doesnt need opened - things to ponder for future revisions
                this.Dispatcher.Invoke(() =>
                {
                    MainWindow main = this.MainWindow as MainWindow;
                    main.Window_Loaded(this, null);
                });
            }
        }

        void trayIcon_Click(object sender, EventArgs e)
        {
            // Check if the window is already open 
            if (FileBackupWindow.IsLoaded == true)
            {
                FileBackupWindow.Activate();
                FileBackupWindow.Visibility = Visibility.Visible;
                FileBackupWindow.WindowState = WindowState.Normal;
            }
            else
            {
                //Open the window
                FileBackupWindow = new MainWindow();
                FileBackupWindow.Visibility = Visibility.Visible;
                FileBackupWindow.WindowState = WindowState.Normal;
            }
        }

        /* Don't think I need this really.....
        /// <summary>
        /// FileSystemWatcher file added function
        /// </summary>
        private void FileCreated(object source, FileSystemEventArgs e)
        {
            MessageBox.Show("File Created: " + e.FullPath + " " + e.ChangeType);

            // Add to the FilesUpdatedOrAdded table. This table will be cleared when the update is zipped and stored.
            FilesUpdatedOrAdded file = new FilesUpdatedOrAdded();
            file.FileFullNamePath = e.FullPath;
            file.FileChangeType = 1;
            file.FileChangeDateTime = DateTime.Now;

            fileBackupEntitesContext.FilesUpdatedOrAddeds.Add(file);
            fileBackupEntitesContext.SaveChanges();

            // Update the UI - Do I need a check to make sure window exists for this? Can I decouple this as in theory the main window maybe doesnt need opened - things to ponder for future revisions
            this.Dispatcher.Invoke(() =>
            {
                MainWindow main = this.MainWindow as MainWindow;
                main.Window_Loaded(this, null);
            });
        }
        */
    }
}
