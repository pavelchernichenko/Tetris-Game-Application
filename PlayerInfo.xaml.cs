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
using System.Windows.Shapes;

namespace CSCD371FinalProject
{
    /// <summary>
    /// Interaction logic for PlayerInfo.xaml
    /// </summary>
    public partial class PlayerInfo : Window
    {
        public PlayerInfo()
        {
            InitializeComponent();
        }
        

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
       
        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            
            
            PlayWindow thirdWindow = new PlayWindow(PlayerName.Text);
            
            //scrWind.Show();
            thirdWindow.Show();
            this.Close();
        }
        

        
        public static implicit operator PlayerInfo(PlayWindow v)
        {
            throw new NotImplementedException();
        }
    }
}
