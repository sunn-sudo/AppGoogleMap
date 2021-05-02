using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Collections.ObjectModel;

namespace AppGoogleMap
{
    public class SearchWord
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public String Word { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    // リスト1件のデータを表すクラス
    public class SearchWordList
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string TimeStamp { get; set; }

        public SearchWordList(int Id, string Word, string TimeStamp)
        {
            this.Id = Id;
            this.Word = Word;
            this.TimeStamp = TimeStamp;
        }
    }

    public partial class MainPage : ContentPage
    {
        public static string DbPath { get; }
            = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                , "SQLiteDataBase.db");

        public static ObservableCollection<SearchWordList> WordListData { get; set; }
            = new ObservableCollection<SearchWordList>();
        public MainPage()
        {
            InitializeComponent();
            // ListViewの初期化
            WordListData = new ObservableCollection<SearchWordList>();
            // データベース初期設定
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {   // テーブル作成
                db.CreateTable<SearchWord>();
                // データ取得
                foreach (var row in db.Table<SearchWord>())
                {
                    WordListData.Add(new SearchWordList(row.Id, row.Word, row.TimeStamp.ToString()));
                }
            }

            // ListViewにデータソースをセット
            wordList.ItemsSource = WordListData;
        }

        async void OnButtonSearch(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                try
                {
                    IGeolocator locator = CrossGeolocator.Current;
                    // 1. 50mの精度に指定
                    // locator.DesiredAccuracy = 50;
                    Position position = await locator.GetPositionAsync();

                    string result = position.Latitude.ToString() + ',' + position.Longitude.ToString();
                    string search_text = ((Button)sender).Text;

                    await Launcher.OpenAsync("geo:" + result + "?q=" + search_text);
                }
                catch
                {
                    await DisplayAlert("確認", "位置情報をオンにしてください。", "OK");
                }

            }
        }

        private void OnButtonDelete(object sender, EventArgs e)
        {
            SearchWordList remove_list = (SearchWordList)((Button)sender).CommandParameter;
            using (var db = new SQLite.SQLiteConnection(MainPage.DbPath))
            {
                db.Delete<SearchWord>(remove_list.Id);
            }
            WordListData.Remove(remove_list);
        }

        private void NextPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SubPage());
        }


    }
}
