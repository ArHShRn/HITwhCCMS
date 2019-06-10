using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using HITwhCMS.Backend.Database;
using HITwhCMS.Backend.DataTemplate;
using HITwhCMS.Backend.Utils;
using HITwhCMS.Frontend.View;
using HITwhCMS.Frontend.ViewModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace HITwhCMS_Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string username = "";
        private string password = "";

        private RegisterWindow registerWindow;

        private bool bExit = false;

        private int dActRefreshSec = 3;
        private int ErrorCount = 0;

        private DatabaseHelper db_helper = null;
        private StudentInfo studentInfo;

        private readonly MainWindowViewModel _viewModel;

        /// <summary>
        /// A single exception that occurs in SQL stage. This will be set to NULL by a function which excuted well without any exception.
        /// </summary>
        private Exception exSQL = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;

            InitializeComponent();
            db_helper = new DatabaseHelper();

            Loaded += SetInitalFocus;
            Loaded += InitialControl;
            this.Closing += this.OnWindowClosing;
            this.KeyDown += this.Event_KeyDown;

            //Transitioning timer
            var transTimer = new DispatcherTimer(TimeSpan.FromSeconds(dActRefreshSec), DispatcherPriority.Normal, Tick, this.Dispatcher);
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
                Text = StandardDataTemplate.activityInfo.Website.ToUpper() + Environment.NewLine +
                    StandardDataTemplate.activityInfo.TitleLevel1 + Environment.NewLine +
                    StandardDataTemplate.activityInfo.TitleLevel2,
                SnapsToDevicePixels = true,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI Black"),
                TextAlignment = TextAlignment.Center,
                FontSize = 18
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
                Text = StandardDataTemplate.activityInfo.Website.ToUpper() + Environment.NewLine + 
                    StandardDataTemplate.activityInfo.TitleLevel1 + Environment.NewLine +
                    StandardDataTemplate.activityInfo.TitleLevel2,
                SnapsToDevicePixels = true,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI Black"),
                TextAlignment = TextAlignment.Center,
                FontSize = 18
            };
        }
        /// <summary>
        /// Set initial focus to username textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetInitalFocus(object sender, RoutedEventArgs e)
        {
            tbUsername.Focus();
        }

        /// <summary>
        /// Popup dialog when user tries to close the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel) return;

            e.Cancel = !this.bExit;
            if (this.bExit) return;

            if(!_viewModel.QuitConfirmationEnabled)
                Application.Current.Shutdown();

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "退出",
                NegativeButtonText = "点错了..",
                AnimateShow = true,
                AnimateHide = true
            };
            var result = await this.ShowMessageAsync(
                "你确定要退出吗(#`O′)",
                "退出还是不退出，这是个问题。",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            this.bExit = result == MessageDialogResult.Affirmative;

            if (this.bExit) Application.Current.Shutdown();

            return;
        }

        /// <summary>
        /// Register an existing student into the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtRegister_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "好的！",
                NegativeButtonText = "取消",
                ColorScheme = MetroDialogOptions.ColorScheme,
                AnimateShow = true,
                AnimateHide = true
            };

            var result = await this.ShowMessageAsync("新用户入驻", 
                "欢迎您使用哈尔滨工业大学（威海）校园卡管理系统！\n请在弹出来的窗口中完成您的入驻手续~",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Negative) return;

            if (registerWindow == null)
            {
                registerWindow = new RegisterWindow();
                registerWindow.Closed += async (o, args) => 
                {
                    registerWindow = null;
                    this.Show();
                    result = await this.ShowMessageAsync("哇哦！",
                        "您是不是已经成功入驻啦呢？赶快体验一下叭！(☆▽☆)",
                        MessageDialogStyle.Affirmative, mySettings);
                    this.tbPassword.Clear();
                    this.tbUsername.Clear();
                };
            }
            this.Hide();
            registerWindow.Launch();
        }

        /// <summary>
        /// Login an admin into the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtAdminLogin_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "好的！",
                ColorScheme = MetroDialogOptions.ColorScheme,
                AnimateShow = true,
                AnimateHide = true
            };
            var result = await this.ShowMessageAsync("开发中",
                "开发中\nStill under development",
                MessageDialogStyle.Affirmative, mySettings);
        }

        /// <summary>
        /// Show "more" window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtMore_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "好的！",
                ColorScheme = MetroDialogOptions.ColorScheme,
                AnimateShow = true,
                AnimateHide = true
            };
            var result = await this.ShowMessageAsync("开发中",
                "开发中\nStill under development",
                MessageDialogStyle.Affirmative, mySettings);
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
        /// Login actions. Retrieve info from SQL server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            username = tbUsername.Text;
            password = tbPassword.Password;

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

            //Check
            //TO-DOs: Password should not contain any special character.
            var beValid = password.Length != 0 && _viewModel.bValidUsernameInput;

            //----------Validate input
            if (!beValid)
            {
                MessageDialogResult result = await this.ShowMessageAsync("格式有误", "检查一下你输入的账号信息哦~d=====(￣▽￣*)b",
                    MessageDialogStyle.Affirmative, mySettings);
            }
            //----------Validate information
            else
            {
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
                    studentInfo = db_helper.RetrieveStudentInfo(username, HITwhCMS.Backend.DataTemplate.DataFormat.Login)
                );
                exSQL = studentInfo.exSQL;

                //Stage 3 - Check account state
                if (studentInfo.CurrentFormat == HITwhCMS.Backend.DataTemplate.DataFormat.NULL)
                {
                    string msg = "";
                    if (exSQL == null)
                        msg = "数据库里面还没有您的学号信息，请检查输入哦~”";
                    else
                        msg = exSQL.Message;

                    ErrorCount++;

                    controller.SetProgress(1.0d);
                    controller.SetTitle("很抱歉~");

                    //No user info
                    if (exSQL == null)
                        controller.SetMessage(msg);
                    //Exception
                    else if(ErrorCount < 2)
                    {
                        controller.SetMessage("数据库在读取您的数据的时候出现了问题...建议您尝试重新连接...\n\nMySQL说道：\n\"" + msg + "\"");
                    }
                    //Repeated exception
                    else
                    {
                        controller.SetMessage("数据库在读取您的数据的时候出现了问题...建议您尝试重新连接...\n\nMySQL说道：\n\"" + msg + "\"");
                        await this.ShowMessageAsync("检测到连续的错误",
                            "程序内部检测到同一个任务阶段发生了多个错误。\n您可以忽略该消息，但是仍然建议您将此问题报告给系统管理员。",
                            settings: mySettings);
                    }

                    exSQL = await Task.Run(() => db_helper.Disconnect());
                    await Task.Delay(3000);
                    await controller.CloseAsync();
                    return;
                }
                ErrorCount = 0;

                if (!studentInfo.bRegistered)
                {
                    controller.SetProgress(1.0d);
                    controller.SetTitle("很抱歉~");
                    controller.SetMessage("您还没有入驻本系统，先在登陆界面点击“新用户入驻哦~”");

                    exSQL = await Task.Run(() => db_helper.Disconnect());
                    await Task.Delay(3000);
                    await controller.CloseAsync();
                    return;
                }

                //We wiped the SQL connection because there's no need to keep it up.
                exSQL = await Task.Run(() => db_helper.Disconnect());

                //Stage 4 - Decrypt data
                controller.SetMessage("正在解密从服务端传输的信息...");
                var correct_pwd = await Task.Run(() => AESHelper.Decrypt(studentInfo.sPasswd, AES_Static.AES256Key));

                //Stage 5 - Validate data
                controller.SetMessage("正在验证您输入的账户信息...");
                if (tbPassword.Password != correct_pwd)
                {
                    controller.SetProgress(1.0d);
                    controller.SetTitle("很抱歉~");
                    controller.SetMessage("您输入了错误的密码呢(+_+)?请检查以下输入哦~如果想找回密码请点击右上角“更多功能”~");
                    await Task.Delay(3000);
                    await controller.CloseAsync();
                }
                else
                {
                    controller.SetProgress(1.0d);
                    controller.SetTitle("欢迎回来！");
                    controller.SetMessage(studentInfo.sName + "，咱们很长时间没有见过了！");
                    await Task.Delay(3000);
                    await controller.CloseAsync();

                    var s = new HomeWindow();
                    s.Closing += (o, args) =>
                    {
                        Application.Current.Shutdown();
                    };
                    s.Launch();
                    this.Hide();
                }
                return;
            }
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
    }
}
