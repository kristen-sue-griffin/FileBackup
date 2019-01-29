using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Forms;
using System.Data.Entity;
using System;

namespace FileBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileBackupSQLEntities fileBackupContext = new FileBackupSQLEntities();
        CollectionViewSource filesUpdatedOrAddeedSource;
        CollectionViewSource fileBackupMainViewSource;
        CollectionViewSource fileBackupMainFileBackupHistoriesViewSource;


        FileBackupGeneral fileBackupGeneral = new FileBackupGeneral();


        public MainWindow()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Fills the UI with the database items
        /// </summary>
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filesUpdatedOrAddeedSource = ((CollectionViewSource)(this.FindResource("filesUpdatedOrAddedViewSource")));
            
            fileBackupContext.FilesUpdatedOrAddeds.Load();
            filesUpdatedOrAddeedSource.Source = fileBackupContext.FilesUpdatedOrAddeds.Local;

            fileBackupMainViewSource = ((CollectionViewSource)(this.FindResource("fileBackupMainViewSource")));
            fileBackupMainViewSource.CollectionViewType = typeof(ListCollectionView);

            fileBackupContext.FileBackupMains.Load();
            fileBackupMainViewSource.Source = fileBackupContext.FileBackupMains.Local;            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.MessageBox.Show("CLOSING");
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Takes the updated/added files and backs them up.
        /// </summary>
        private void BackupSelectedCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // Look in backup 2018_07_13 for info about selected instead of all chose to not do so for now though
            fileBackupGeneral.BackupFiles(sender, e);
        }

        /// <summary>
        /// Change handler for the main listing of a backed up file --> Pushes all of its history to the history UI for viewing etc
        /// </summary>
        private void fileBackupMainDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FileBackupMain selectedFiles = fileBackupMainViewSource.View.CurrentItem as FileBackupMain;
            fileBackupMainFileBackupHistoriesViewSource = ((CollectionViewSource)(this.FindResource("fileBackupMainFileBackupHistoriesViewSource")));

        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            
            if (e.Item is FileBackupMain)
            {
                e.Accepted = (e.Item as FileBackupMain).FileName.ToUpper().Contains(filterTextBox.Text.ToUpper());
            }
            else
            { 
                e.Accepted = true;
            }
        }

        private void FilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            
            if (fileBackupMainDataGrid.Items is CollectionView)
            {
                CollectionViewSource fbvs = (CollectionViewSource)FindResource("fileBackupMainViewSource");
                if (fbvs != null)
                    fbvs.View.Refresh();
            }
        }
    }

}
