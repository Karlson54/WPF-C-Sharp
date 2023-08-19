using System.Windows;

namespace hw1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToSecondPageButton_Click(object sender, RoutedEventArgs e)
        {
            string MainInfo = "";
            string Address = "";
            string JobInfo = "";
            string BankCards = "";
            string AccountStatus = "";
            switch (empName.SelectedIndex)
            {
                case (0):
                    MainInfo = "Name: Shelton Second Name: Cole\nPhoto: Cool photo\nPosition: Corporate Marketing Producer\nEmail:shelton.cole@email.com";
                    Address = "City: Virgiliochester\nStreet name: Alesha Motorway\nStreet address: 8412 Oberbrunner Shore\nZip code: 70973\nState: Louisiana\nCountry: United States";
                    JobInfo = "Title: Corporate Marketing Producer\nKey Skill: Organisation";
                    BankCards = "Nmber: 5135-1644-0127-6135";
                    AccountStatus = "Plan: Gold\nStatus: Idle\nPayment method: Bitcoins\nTerm: Monthly";
                    break;

                case (1):
                    MainInfo = "Name: Ashot Name: Ashotovich\nPhoto: unknown\nPosition: unknown\nEmail: unknown";
                    Address = "City: unknown\nStreet name: unknown\nStreet address: unknown\nZip code: unknown\nState: unknown\nCountry: unknown";
                    JobInfo = "Title: unknown\nKey Skill: unknown";
                    BankCards = "Nmber: unknown";
                    AccountStatus = "Plan: unknown\nStatus: unknown\nPayment method: unknown\nTerm: unknown";
                    break;

                case (2):
                    MainInfo = "Name: ? Name: ?\nPhoto: ?\nPosition: ?\nEmail: ?";
                    Address = "City: ?\nStreet name: ?\nStreet address: ?\nZip code: ?\nState: ?\nCountry: ?";
                    JobInfo = "Title: ?\nKey Skill: ?";
                    BankCards = "Nmber: ?";
                    AccountStatus = "Plan: ?\nStatus: ?\nPayment method: ?\nTerm: ?";
                    break;
            }
            SecondPage secondPage = new(MainInfo, Address, JobInfo, BankCards, AccountStatus);

            Content = secondPage;

        }
    }
}