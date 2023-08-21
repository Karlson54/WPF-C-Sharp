using System.Windows.Controls;

namespace hw2
{
    public partial class SecondPage : Page
    {
        public SecondPage(string MainInfo, string Address, string JobInfo, string BankCards, string AccountStatus)
        {
            InitializeComponent();
            this.MainInfo.Text = MainInfo;
            this.Address.Text = Address;
            this.JobInfo.Text = JobInfo;
            this.BankCards.Text = BankCards;
            this.AccountStatus.Text = AccountStatus;
        }
    }
}
