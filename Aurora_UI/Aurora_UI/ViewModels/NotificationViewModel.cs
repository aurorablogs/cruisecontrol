using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace Aurora_UI.ViewModels
{
    public class NotificationViewModel:DependencyObject
    {
        public NotificationViewModel(string notification)
        {
            Notification = notification;
        }

        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof (string), typeof (NotificationViewModel), new PropertyMetadata(default(string)));

        public string Notification
        {
            get { return (string) GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }
    }
}
