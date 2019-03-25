using System.Windows;
using System.Windows.Controls;

namespace BattleshipUIRework.Views
{
    /// <summary>
    /// Interaction logic for BeforeQueueView.xaml
    /// </summary>
    public partial class BeforeQueueView : UserControl
    {
        public BeforeQueueView()
        {
            InitializeComponent();
        }

        private void StartQueueButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DataContext = new InQueueView();
        }
    }
}
