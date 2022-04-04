using MddPickUp;
using MddPickUp.Helpers;
using MddPickUp.Service;
using MddPickUp.Shared;
using MddPickUp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MddPickUp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Common.ToastInit();
            ThemeHelper.ApplyAdjustment();
            MainWindow m = new MainWindow();
            this.MainWindow = m;

            //1.登录
            Account.Init();
            var res = Account.ValidateLogin(Account.id, Account.key);
            if (!res)
            {
                bool valid = false;
                string msg = "";
                while(!valid)
                {
                    LoginWindow inputWindow = new LoginWindow();
                    var res1 = inputWindow.ShowDialog();
                    if (res1.HasValue && res1.Value == false)
                    {
                        Application.Current.Shutdown();
                        return;
                    }
                    
                    valid = Account.ValidateLogin(inputWindow.Id, inputWindow.Key);
                    if (!valid)
                        MessageBox.Show("登录失败");
                    else
                        Account.Login(inputWindow.Id, inputWindow.Key);
                }
            }

            //2.初始化
            Task.Init();

            //2.读取数据
            //Storage.ReadData();

            //3.启动
            m.Show();
        }
    }
}
