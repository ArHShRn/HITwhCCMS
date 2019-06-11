using MahApps.Metro.Controls;
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
using HITwhCMS.Frontend.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using HITwhCMS.Backend.Database;
using HITwhCMS.Backend.Utils;
using System.Windows.Threading;

namespace HITwhCMS.Frontend.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : MetroWindow
    {
        private int dActRefreshSec = 2;

        private DatabaseHelper db_helper = null;
        /// <summary>
        /// A single exception that occurs in SQL stage. This will be set to NULL by a function which excuted well without any exception.
        /// </summary>
        private Exception exSQL = null;
        private int ErrorCount = 0;
        private bool bIdentified = false;
        public string sStudentID = "";
        private readonly RegisterWindowViewModel registerWindowViewModel;

        private List<char> listDigit = null;
        private List<char> listDigitX = null;

        public RegisterWindow()
        {
            registerWindowViewModel = new RegisterWindowViewModel();
            this.DataContext = registerWindowViewModel;

            InitializeComponent();

            db_helper = new DatabaseHelper();


            this.KeyDown += Event_KeyDown;
            this.Loaded += SetInitalFocus;
            this.Loaded += InitialControl;

            //Transitioning timer
            var transTimer = new DispatcherTimer(TimeSpan.FromSeconds(dActRefreshSec), DispatcherPriority.Normal, Tick, this.Dispatcher);

            Tab0.IsEnabled = false;
            Tab1.IsEnabled = false;
            Tab2.IsEnabled = false;

            listDigit = new List<char>();
            listDigitX = new List<char>();

            for (int i = 0; i < 10; ++i) listDigit.Add((char)('0' + i));
            listDigitX.AddRange(listDigit);
            listDigitX.Add('X');
        }

        /// <summary>
        /// Tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Tick(object sender, EventArgs e)
        {
            transitioning.Content = new TextBlock
            {
                Text = tbNicknameInput.Text,
                SnapsToDevicePixels = true,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI Black"),
                TextAlignment = TextAlignment.Center,
                FontSize = 20
            };
        }

        /// <summary>
        /// Initialize controls here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitialControl(object sender, RoutedEventArgs e)
        {
            transitioning.Content = new TextBlock
            {
                Text = "墨色黛海的缅因猫",
                SnapsToDevicePixels = true,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI Black"),
                TextAlignment = TextAlignment.Center,
                FontSize = 20
            };
        }

        /// <summary>
        /// Common method to launch a new window
        /// </summary>
        public void Launch()
        {
            Owner = Application.Current.MainWindow;
            // only for this window, because we allow minimizing
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            Show();
        }

        /// <summary>
        /// Action happens on key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Event_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private async void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            string IDNumber = tbIDNumber.Text;
            string Name = tbName.Text;

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "好的！",
                NegativeButtonText = "谢谢，不用",
                ColorScheme = MetroDialogOptions.ColorScheme,
                AnimateShow = true,
                AnimateHide = true
            };

            var myProcessSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "好的！",
                ColorScheme = MetroDialogOptions.ColorScheme,
                AnimateShow = true,
                AnimateHide = true
            };

            if (exSQL != null)
            {
                await this.ShowMessageAsync("运维提醒（可忽略）",
                    "这条消息会出现是因为代码内部检测到了上一个操作发生了错误。" +
                    "\n如果您是一位用户并恰巧看到了这个消息，您可以忽略该消息，但是仍然建议您将此问题报告给系统管理员。",
                    settings: mySettings);
            }

            //Last unhandled exception
            if (db_helper.bConnectionAlive)
            {
                await this.ShowMessageAsync("运维提醒（可忽略）",
                    "数据库连接仍然处于活跃状态，这可能会导致程序接下来的运行不稳定。" +
                    "这条消息会出现是因为代码内部检测到了上一个操作的连接没有被成功关闭。" +
                    "\n如果您是一位用户并恰巧看到了这个消息，您可以忽略该消息，但是仍然建议您将此问题报告给系统管理员。",
                    settings: mySettings);
            }

            //Check
            //TO-DOs: Password should not contain any special character.
            var prev17 = tbIDNumber.Text.Substring(0, 17);
            var last = tbIDNumber.Text.Substring(17);

            bool bPrev17CtChar = false;
            foreach(var c in prev17)
            {
                if(!listDigit.Contains(c))
                {
                    bPrev17CtChar = true;
                    break;
                }
            }

            bool lastValid = listDigitX.Contains(last.First());

            var beValid = tbIDNumber.Text.Length == 18 && lastValid && !bPrev17CtChar;

            //----------Validate input
            if (!beValid)
            {
                MessageDialogResult result = await this.ShowMessageAsync("格式有误", "检查一下你输入的身份证信息哦~d=====(￣▽￣*)b",
                    MessageDialogStyle.Affirmative, mySettings);
                return;
            }

            var controller = await this.ShowProgressAsync("请稍等......", "我们正在连接到数据库以获取您的账号信息", settings: myProcessSettings);

            controller.SetIndeterminate();
            controller.SetCancelable(true);

            //Stage 1 - Connect
            exSQL = await Task.Run(() => db_helper.Connect());
            controller.SetCancelable(false);

            if (exSQL != null)
            {
                ErrorCount++;
                controller.SetProgress(1.0d);
                controller.SetTitle("很抱歉~");
                controller.SetMessage("与数据库的连接出现了问题...建议您尝试重新连接...\n\nMySQL说道：\n\"" + exSQL.Message + "\"");

                if (ErrorCount < 2)
                {
                    await Task.Delay(3000);
                }
                else
                {
                    await this.ShowMessageAsync("检测到连续的错误",
                        "程序内部检测到数据库连接阶段发生了多次错误。\n您可以忽略该消息，但是仍然建议您将此问题报告给系统管理员。",
                        settings: mySettings);
                }

                await controller.CloseAsync();
                exSQL = null;

                return;
            }
            ErrorCount = 0;

            //Cancel the action
            if (controller.IsCanceled)
            {
                controller.SetProgress(1.0d);

                controller.SetTitle("结束");
                controller.SetMessage("用户取消了下一步操作。");

                await Task.Run(() => db_helper.Disconnect());
                await Task.Delay(1000);
                await controller.CloseAsync();

                return;
            }

            //Stage 2 - Retrieve info
            controller.SetMessage("正在从服务端获取加密信息...");
            var query_result = await Task.Run(
                ()
                =>
                bIdentified = db_helper.CheckIdentity(IDNumber, Name)
            );

            //We wiped the SQL connection because there's no need to keep it up.
            exSQL = await Task.Run(() => db_helper.Disconnect());

            controller.SetMessage("正在验证您输入的账户信息...");
            if (!bIdentified)
            {
                controller.SetProgress(1.0d);
                controller.SetTitle("很抱歉~");
                controller.SetMessage("您输入了错误的身份信息呢(+_+)?请检查以下输入哦~");
                await Task.Delay(3000);
                await controller.CloseAsync();
            }
            else
            {
                controller.SetProgress(1.0d);
                //controller.SetTitle("身份验证成功！");
                //controller.SetMessage(tbName.Text + "，下面我们开始下一步操作！");
                //await Task.Delay(1000);
                await controller.CloseAsync();
                RegisterTabControl.SelectedIndex = 1;
            }
            return;
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Click to open Github page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtOpenGithub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ArHShRn/DevPrac3-HITwhCCMS/");
        }

        /// <summary>
        /// Set initial focus to username textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetInitalFocus(object sender, RoutedEventArgs e)
        {
            tbIDNumber.Focus();
        }

        private void BtPrevious_Tab1_Click(object sender, RoutedEventArgs e)
        {
            RegisterTabControl.SelectedIndex -= 1;
        }

        private async void BtNext_Tab1_Click(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowMessageAsync(
                "你确定信息正确吗(#`O′)",
                "点击OK以进行下一步，请注意您随时可以点击\"上一步\"以返回此页面！",
                MessageDialogStyle.AffirmativeAndNegative);

            if(result == MessageDialogResult.Affirmative)
            {
                string IDNumber = tbIDNumber.Text;
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "好的！",
                    NegativeButtonText = "谢谢，不用",
                    ColorScheme = MetroDialogOptions.ColorScheme,
                    AnimateShow = true,
                    AnimateHide = true
                };

                var myProcessSettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "好的！",
                    ColorScheme = MetroDialogOptions.ColorScheme,
                    AnimateShow = true,
                    AnimateHide = true
                };

                if (exSQL != null)
                {
                    await this.ShowMessageAsync("运维提醒（可忽略）",
                        "这条消息会出现是因为代码内部检测到了上一个操作发生了错误。" +
                        "\n如果您是一位用户并恰巧看到了这个消息，您可以忽略该消息，但是仍然建议您将此问题报告给系统管理员。",
                        settings: mySettings);
                }

                //Last unhandled exception
                if (db_helper.bConnectionAlive)
                {
                    await this.ShowMessageAsync("运维提醒（可忽略）",
                        "数据库连接仍然处于活跃状态，这可能会导致程序接下来的运行不稳定。" +
                        "这条消息会出现是因为代码内部检测到了上一个操作的连接没有被成功关闭。" +
                        "\n如果您是一位用户并恰巧看到了这个消息，您可以忽略该消息，但是仍然建议您将此问题报告给系统管理员。",
                        settings: mySettings);
                }

                var controller = await this.ShowProgressAsync("请稍等......", "我们正在连接到数据库以获取您的账号信息", settings: myProcessSettings);

                controller.SetIndeterminate();
                controller.SetCancelable(true);

                //Stage 1 - Connect
                exSQL = await Task.Run(() => db_helper.Connect());
                controller.SetCancelable(false);

                if (exSQL != null)
                {
                    ErrorCount++;
                    controller.SetProgress(1.0d);
                    controller.SetTitle("很抱歉~");
                    controller.SetMessage("与数据库的连接出现了问题...建议您尝试重新连接...\n\nMySQL说道：\n\"" + exSQL.Message + "\"");

                    if (ErrorCount < 2)
                    {
                        await Task.Delay(3000);
                    }
                    else
                    {
                        await this.ShowMessageAsync("检测到连续的错误",
                            "程序内部检测到数据库连接阶段发生了多次错误。\n您可以忽略该消息，但是仍然建议您将此问题报告给系统管理员。",
                            settings: mySettings);
                    }

                    await controller.CloseAsync();
                    exSQL = null;

                    return;
                }
                ErrorCount = 0;

                //Cancel the action
                if (controller.IsCanceled)
                {
                    controller.SetProgress(1.0d);

                    controller.SetTitle("结束");
                    controller.SetMessage("用户取消了下一步操作。");

                    await Task.Run(() => db_helper.Disconnect());
                    await Task.Delay(1000);
                    await controller.CloseAsync();

                    return;
                }

                //Stage 2 - Retrieve info
                controller.SetMessage("正在从服务端获取加密信息...");
                var query_result = await Task.Run(
                    ()
                    =>
                    sStudentID = db_helper.GetStudentNumFromID(IDNumber)
                );

                //We wiped the SQL connection because there's no need to keep it up.
                exSQL = await Task.Run(() => db_helper.Disconnect());

                controller.SetProgress(1.0d);
                await controller.CloseAsync();

                RegisterTabControl.SelectedIndex += 1;
                Console.WriteLine(sStudentID);
            }
            else
            {
                return;
            }

        }

        private void BtPrevious_Tab2_Click(object sender, RoutedEventArgs e)
        {
            RegisterTabControl.SelectedIndex -= 1;
        }

        private async void BtNext_Tab2_Click(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowMessageAsync(
                "你确定信息正确吗(#`O′)",
                "点击OK以完成账户的激活！",
                MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void TbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbPassword.Password.Length > 16) return;

            pbPasswordStrength.Value = 0 + tbPassword.Password.Length * 4;
        }
    }
}
