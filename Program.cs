using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyingJavabo
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, Application.ProductName);
            if(!mutex.WaitOne(0, false))
            {
                return;
            }
            GC.KeepAlive(mutex);
            mutex.Close();

            if(args.Length < 1)
            {
                //ShowScreenSaver();
                //return;
            }
            //String trim = args[0].ToLower().Trim().Substring(0, 2);
            String trim = "/s";
            switch(trim)
            {
            // 表示
            case "/s":
                // スクリーンセーバーを実行
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ShowScreenSaver();
                Application.Run();
                break;
            // プレビュー
            case "/p":
                // プレビュー画面を表示
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //args[1] はプレビューウィンドウのハンドル
                Application.Run(new MainForm(new IntPtr(long.Parse(args[1]))));
                break;
            // 設定
            case "/c":
                // スクリーンセーバーのオプション表示
                MessageBox.Show("オプションなし\nこのスクリーンセーバーには、設定できるオプションはありません。",
                    "My Screen Saver",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                break;
            default:
                // 常にスクリーンセーバーを表示
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ShowScreenSaver();
                Application.Run();
                break;
            }
        }

        // スクリーンセーバーを表示
        static void ShowScreenSaver()
        {
            // コンピューター上のすべてのスクリーン(モニター)をループ
            foreach (Screen screen in Screen.AllScreens)
            {
                MainForm screensaver = new MainForm(screen.Bounds);
                screensaver.Show();
            }
        }
    }
}