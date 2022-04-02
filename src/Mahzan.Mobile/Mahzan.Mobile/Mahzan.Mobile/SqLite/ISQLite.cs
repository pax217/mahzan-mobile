using SQLite;

namespace Mahzan.Mobile.SqLite
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}