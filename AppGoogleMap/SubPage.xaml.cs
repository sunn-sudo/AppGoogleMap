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
            // 登録制限
            if (insertText.Text == null || insertText.Text == "")
            {
                // 登録成功アラート
                DisplayAlert("登録失敗", "入力文字が空です。", "OK");
                return;
            }
            // データ追加
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {   
                db.Insert(new SearchWord() { Word = insertText.Text, TimeStamp = DateTime.Now });
            }
            // 登録成功アラート
            DisplayAlert("登録成功", insertText.Text + "を登録しました。", "OK");
        }

    }
}