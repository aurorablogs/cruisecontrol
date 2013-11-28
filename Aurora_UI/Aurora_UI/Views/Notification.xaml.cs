using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aurora_UI.ViewModels;

namespace Aurora_UI.Views
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public NotificationViewModel NotificationViewModel;
        public Notification(string notify)
        {
            InitializeComponent();
            NotificationViewModel = new NotificationViewModel(notify);
            this.DataContext = NotificationViewModel;

        }
    }
}
