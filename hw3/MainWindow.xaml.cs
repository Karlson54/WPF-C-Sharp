using System.Windows;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace hw5
{
    public partial class MainWindow : Window
    {
        List<User> users = new();
        private readonly HttpClient _client;
        public MainWindow()
        {
            InitializeComponent();
            _client = new HttpClient();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var responseText = await _client.GetStringAsync($"https://random-data-api.com/api/v2/users?size={Count.Text}");
            JToken json = JToken.Parse(responseText);

            users.Clear();

            if (json is JArray JArrays)
            {
                foreach (JToken item in JArrays)
                {
                    User user = new(
                        item["id"].Value<int>(),
                        $"{item["first_name"].Value<string>()}",
                        $"{item["last_name"].Value<string>()}",
                        $"{item["phone_number"].Value<string>()}",
                        $"{item["email"].Value<string>()}",
                        $"{item["address"]["city"].Value<string>()}"
                    );
                    users.Add(user);
                }
                PackageList.ItemsSource = users;
            }

            else if (json is JObject obj)
            {
                User user = new(
                    obj["id"].Value<int>(),
                    $"{obj["first_name"].Value<string>()}",
                    $"{obj["last_name"].Value<string>()}",
                    $"{obj["phone_number"].Value<string>()}",
                    $"{obj["email"].Value<string>()}",
                    $"{obj["address"]["city"].Value<string>()}"
                );
                users.Add(user);
                PackageList.ItemsSource = users;
            }

            PackageList.Items.Refresh();
        }

        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (PackageList.SelectedItem is User selectedUser)
            {
                var moreInfo = new MoreInfo(selectedUser);
                Content = moreInfo;
            }
        }

    }

}
