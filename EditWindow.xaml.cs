using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace LabN4_
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        const uint SC_CLOSE = 0xF060;
        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;
        const uint MF_ENABLED = 0x00000000;

        public EditWindow()
        {
            InitializeComponent();
            btnSubmit.IsEnabled = false;
            tbInfo.TextChanged += Edited;
            tbName.TextChanged += Edited;
            tbPrice.TextChanged += Edited;
            tbSeasons.TextChanged += Edited;
        }
        public EditWindow(DataRowView tm)
        {
            InitializeComponent();
            btnSubmit.IsEnabled = false;
            tbInfo.TextChanged += Edited;
            tbName.TextChanged += Edited;
            tbPrice.TextChanged += Edited;
            tbSeasons.TextChanged += Edited;

            tbInfo.Text = tm.Row.ItemArray[2].ToString();
            tbName.Text = tm.Row.ItemArray[1].ToString();
            tbPrice.Text = tm.Row.ItemArray[3].ToString();
            tbSeasons.Text = tm.Row.ItemArray[4].ToString();
        }

        private void Edited(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(tbSeasons.Text) && !string.IsNullOrEmpty(tbPrice.Text) && !string.IsNullOrEmpty(tbName.Text) && !string.IsNullOrEmpty(tbInfo.Text))
            {
                btnSubmit.IsEnabled = true;
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                IntPtr hMenu = GetSystemMenu(hwnd, false);

                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_ENABLED);
            }
            else
            {
                btnSubmit.IsEnabled = false;
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                IntPtr hMenu = GetSystemMenu(hwnd, false);

                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }

            try
            {
                decimal i = Convert.ToDecimal(tbPrice.Text);
            }
            catch
            {
                btnSubmit.IsEnabled = false;
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                IntPtr hMenu = GetSystemMenu(hwnd, false);

                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);

            }
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Disable close button
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero)
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
