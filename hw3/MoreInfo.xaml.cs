using System.Windows.Controls;

namespace hw3
{
    public partial class MoreInfo : Page
    {
        public MoreInfo(User user)
        {
            InitializeComponent();
            ID.Content = user.id.ToString();
            first_name.Content = user.first_name.ToString();
            last_name.Content = user.last_name.ToString();
            phone_number.Content = user.phone_number.ToString();
            email.Content = user.email.ToString();
            city.Content = user.city.ToString();
        }
    }
}