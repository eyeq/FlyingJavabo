using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FlyingJavabo
{
    public partial class MainForm : Form
    {
        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private const String TITLE = "FlyingJavabo";

        private bool IsPreviewMode = false;
        private Timer timer;

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        // このコンストラクタは、フォームを全画面で表示する
        // ノーマルモードで使用される
        public MainForm(Rectangle Bounds) : this()
        {
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Bounds;
            this.TopMost = true;
            Cursor.Hide();
        }

        // このコンストラクタは、スクリーンセーバーの選択ダイアログボックスの小窓で使用される。
        // プレビューモードで使用される (/p)
        public MainForm(IntPtr PreviewHandle) : this()
        {
            SetParent(this.Handle, PreviewHandle);
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;

            this.Location = new Point(0, 0);
            this.IsPreviewMode = true;
        }

        #endregion

        #region Screen

        private ScreenDrawer drawer;

        private void TimerTick(object sender, System.EventArgs e)
        {
            if(drawer == null)
            {
                drawer = new ScreenDrawer(Width, Height);
            }
            System.Drawing.Graphics graphics = this.CreateGraphics();
            drawer.Draw(graphics);
        }

        #endregion

        #region User Input

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void MainForm_MouseDown(object sender, EventArgs e)
        {
            if(!IsPreviewMode)
            {
                System.Environment.Exit(0);
            }
        }

        // XとY int.MaxValueのとOriginalLoctionを始める
        // カーソルがその位置にすることは不可能なので。
        // この変数がまだ設定されている場合
        Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if(!IsPreviewMode)
            {
                //originallocationが設定されているかどうかを確認
                if (OriginalLocation.X == int.MaxValue & OriginalLocation.Y == int.MaxValue)
                {
                    OriginalLocation = e.Location;
                }
                // マウスが20 pixels 以上動いたかどうかを監視
                // 動いた場合はアプリケーションを終了
                if (Math.Abs(e.X - OriginalLocation.X) > 20 | Math.Abs(e.Y - OriginalLocation.Y) > 20)
                {
                    System.Environment.Exit(0);
                }
            }
        }
        #endregion
    }
}