using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ccTalk_rcp_demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private coin_channel[] coins = new coin_channel[2] {
                new coin_channel(new ccTalkNet.ccTalk_Coin() { channel = 1, coin_id="coin1",sorter_path=1 },false),
               new coin_channel(new ccTalkNet.ccTalk_Coin() { channel = 2, coin_id="coin2",sorter_path=1 },false)
            };

        public MainWindow()
        {
            InitializeComponent();
            dataGrid_coins.ItemsSource = coins;
        }
    }
}
