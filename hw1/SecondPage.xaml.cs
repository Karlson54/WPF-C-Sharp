using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace hw1
{
    /// <summary>
    /// Логика взаимодействия для SecondPage.xaml
    /// </summary>
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

        public void lox()
        {

        }
    }
}
