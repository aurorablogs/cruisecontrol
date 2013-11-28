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
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Aurora_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel;
        public MainWindow()
        {
            InitializeComponent();
            ViewModel=new MainWindowViewModel();
            this.DataContext = ViewModel;
            dataGrid1.ItemsSource = ViewModel.Newprice;
            comboBox2.ItemsSource = ViewModel.Subscribers;
            dataGrid1.IsReadOnly = true;
            comboBox1.ItemsSource = ViewModel.OrderTypes;
            comboBox3.ItemsSource = ViewModel.BuySellSelect;
            listBox1.ItemsSource = ViewModel.Ordertypes;
            
        }

       private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

       

        

      

       
    }
}
