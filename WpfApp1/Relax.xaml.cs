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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Relax.xaml
    /// </summary>
    public partial class Relax : UserControl
    {
        public Relax()
        {
            InitializeComponent();
        }

           /* private void BreathingButton_Click(object sender, RoutedEventArgs e)
            {
                RelaxText.Text = "Hướng dẫn hít thở sâu:\nHít vào trong 4 giây - Giữ 4 giây - Thở ra 4 giây.";
                // Bạn có thể mở một View khác để làm animation hướng dẫn thở
            }

            private void GameButton_Click(object sender, RoutedEventArgs e)
            {
                RelaxText.Text = "Đang mở mini game...";
                // Gọi game ở đây, ví dụ:
                var gameWindow = new Game.BubbleGame(); // nếu là Window
                gameWindow.Show();
            }

            private void MusicButton_Click(object sender, RoutedEventArgs e)
            {
                RelaxText.Text = "Đang phát nhạc thư giãn...";
                // Bạn có thể tích hợp MediaPlayer để phát nhạc ở đây
            }*/
    }
}
