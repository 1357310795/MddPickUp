using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MddPickUp.Views
{
    /// <summary>
    /// BaseUriInputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged("Id");
            }
        }
        private string key;

        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                this.RaisePropertyChanged("Key");
            }
        }


        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
