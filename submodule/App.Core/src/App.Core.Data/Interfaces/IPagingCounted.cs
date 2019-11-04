namespace App.Core.Data.Interfaces
{
    public interface IPagingCounted
    {
        int TotalRecordCount { get; set; }
    }
}