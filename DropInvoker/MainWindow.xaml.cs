using DropInvoker.Models;

using Microsoft.Extensions.DependencyInjection;

using RLauncher.Exceptions;

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

namespace DropInvoker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ((App)Application.Current).ServiceProvider.GetRequiredService<MainViewModel>();
        }

        private async void Slot_Drop(object sender, DragEventArgs e)
        {
            try
            {
                await ((CommandViewModel)((FrameworkElement)sender).DataContext).OnDropAsync(e);
            }
            catch (InvalidRLauncherConfigurationFileException exc)
            {
                var title = $"Failed to parse {exc.FileType}";
                var message = $"Unable parse from {exc.FileFullPath} to {exc.FileType}\nSource:\n{exc.FileContent}";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
