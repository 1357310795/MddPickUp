using MddPickUp.Models;
using MddPickUp.Service;
using MddPickUp.Views;
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
using ToastHelper;

namespace MddPickUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ToastManager _manager;

        public MainWindow()
        {
            InitializeComponent();
            _manager = new ToastManager();
            _manager.Init<ToastManager>("麦当劳订餐捡漏程序");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _manager.Notify("检测到有余量", "测试套餐ABC");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var res = PickUp.GroupInfoQuery(BasicInfo.groupNo);
            if (!res.success)
            {
                MessageBox.Show(res.message);
                return;
            }

            foreach (FormModel f in res.forms)
            {
                var t = new TaskModel(f);
                var res1 = PickUp.FormInfoQuery(f.formNo);
                if (!res1.success)
                {
                    MessageBox.Show(res1.message);
                    return;
                }
                t.Foods = res1.foods;
                Task.tasks.Add(t);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoginWindow inputWindow = new LoginWindow();
            var res1 = inputWindow.ShowDialog();
            if (res1.HasValue && res1.Value == false)
            {
                return;
            }
            
            var valid = Account.ValidateLogin(inputWindow.Id, inputWindow.Key);
            if (!valid)
                MessageBox.Show("登录失败");
            else
            {
                Account.Login(inputWindow.Id, inputWindow.Key);
                MessageBox.Show("登录成功");
            }
                
        }
    }
}
