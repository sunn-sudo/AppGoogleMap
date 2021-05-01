using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGoogleMap
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubPage : ContentPage
    {
        public static string DbPath { get; } 
            = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                , "SQLiteDataBase.db");

        public SubPage()
        {
            InitializeComponent();
        }

        private void OnButtonInsert(object sender, EventArgs e)
        {
            // データ追加
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {   // Insert
                db.Insert(new SearchWord() { Word = "登録成功", TimeStamp = DateTime.Now });
            }
        }

    }
}