//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileBackup
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class FileBackupHistory
    {
        public int PK_FileHistoryId { get; set; }
        public string FileHistoryZipPathName { get; set; }
        public string FileHistoryDescription { get; set; }
        public string FileHistoryOriginPath { get; set; }
        public Nullable<System.DateTime> FileHistoryBackupDateTime { get; set; }
        public string FileHistoryUserName { get; set; }
        public Nullable<System.DateTime> FileHistoryEditedDateTime { get; set; }
        public Nullable<int> FK_FileId { get; set; }
    
        public virtual FileBackupMain FileBackupMain { get; set; }
    }
}