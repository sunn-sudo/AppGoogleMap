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

            DateTime temp_now = DateTime.Now;
            String temp_word = insertText.Text;
            // データ追加
            using (var db = new SQLite.SQLiteConnection(MainPage.DbPath))
            {   
                db.Insert(new SearchWord() {TimeStamp = temp_now, Word = temp_word});
                foreach (var row in db.Query<SearchWord>("select Id, Word, TimeStamp from SearchWord where ROWID = last_insert_rowid();"))
                {
                    MainPage.WordListData.Add(new SearchWordList(row.Id, row.Word, row.TimeStamp.ToString()));
                }
            }
            // 登録成功アラート
            DisplayAlert("登録成功", insertText.Text + "を登録しました。", "OK");
        }

    }
}