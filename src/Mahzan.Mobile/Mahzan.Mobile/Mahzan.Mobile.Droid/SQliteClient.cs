using System.IO;
using Mahzan.Mobile.Droid;
using Mahzan.Mobile.SqLite;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(SQliteClient))]
namespace Mahzan.Mobile.Droid
{
    public class SQliteClient : ISQLite
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var sqliteFileName = "mahzan.db3";

            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), sqliteFileName);

            return new SQLiteAsyncConnection(path);
        }
    }
}