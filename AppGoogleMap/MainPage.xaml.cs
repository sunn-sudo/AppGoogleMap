using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Windows.Input;

namespace AppGoogleMap
{
    public class SearchWord
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public String Word { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        public static string DbPath { get; }
            = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                , "SQLiteDataBase.db");

        public MainPage()
        {
            InitializeComponent();
            var sl = (StackLayout)(this.Content = new StackLayout());
            var Label = new Label() { Text = "現在地周辺のスポットを検索します。" };
            var UpdateWordBtn = new Button() { Text = "更新する" };
            var AddWordBtn = new Button() { Text = "検索ワードを追加する" };
            sl.Margin = 5;
            sl.Children.Add(Label);
            sl.Children.Add(UpdateWordBtn);
            sl.Children.Add(AddWordBtn);
            // ページ更新
            UpdateWordBtn.Clicked += UpdatePage;
            // ページ遷移
            AddWordBtn.Clicked += NextPage;

            // データベース初期設定
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {   // テーブル作成
                db.CreateTable<SearchWord>();
                // データ取得
                foreach (var row in db.Table<SearchWord>())
                {
                    var SearchBtn = new Button() { Text = row.Word };
                    sl.Children.Add(SearchBtn);
                    SearchBtn.Clicked += OnButtonSearch;
                }
            }
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
        private void UpdatePage(object sender, EventArgs e)
        {
            var sl = (StackLayout)(this.Content = new StackLayout());
            var Label = new Label() { Text = "現在地周辺のスポットを検索します。" };
            var UpdateWordBtn = new Button() { Text = "更新する" };
            var AddWordBtn = new Button() { Text = "検索ワードを追加する" };
            sl.Margin = 5;
            sl.Children.Add(Label);
            sl.Children.Add(UpdateWordBtn);
            sl.Children.Add(AddWordBtn);
            // ページ更新
            UpdateWordBtn.Clicked += UpdatePage;
            // ページ遷移
            AddWordBtn.Clicked += NextPage;

            // データベース初期設定
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {   // テーブル作成
                db.CreateTable<SearchWord>();
                // データ取得
                foreach (var row in db.Table<SearchWord>())
                {
                    var SearchBtn = new Button() { Text = row.Word };
                    sl.Children.Add(SearchBtn);
                    SearchBtn.Clicked += OnButtonSearch;
                }
            }
        }

        private void NextPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SubPage());
        }

    }
}
