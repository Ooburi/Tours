using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace LabN4_
{
   
    public partial class MainWindow : Window
    {

        private string filePath = Environment.CurrentDirectory + "/appData.xml";
        private XDocument doc;
        public MainWindow()
        {
            InitializeComponent();

            btnDelete.IsEnabled = false;
            btnEdit.IsEnabled = false;


            if (!File.Exists(filePath))
            {
                var doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Created: " + DateTime.Now.ToString()),
                    new XElement("Tours")
                    );

                doc.Save(filePath);
            }

            doc = XDocument.Load(filePath);

            dataGrid.CanUserAddRows = false;
            dataGrid.SelectionMode = DataGridSelectionMode.Single;
            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
            ItemsRefresh();

        }

        private void ItemsRefresh()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(doc.CreateReader());
                DataView dataView = new DataView(ds.Tables[0]);
                dataGrid.ItemsSource = dataView;
                dataGrid.Items.Refresh();

                rtbText.Document.Blocks.Clear();
                rtbText.AppendText(doc.ToString());
            }
            catch { dataGrid.ItemsSource = null; }

        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                btnDelete.IsEnabled = true;
                btnEdit.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow();
            ew.ShowDialog();

            try
            {

                doc.Root.Add(new XElement("Tour",
                    new XElement(Name = "Identifier", Guid.NewGuid()),
                    new XElement(Name = "Name", ew.tbName.Text),
                    new XElement(Name = "Info", ew.tbInfo.Text),
                    new XElement(Name = "Price", ew.tbPrice.Text),
                    new XElement(Name = "Seasons", ew.tbSeasons.Text)
                    ));

                doc.Save(filePath);
                ItemsRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var item = (DataRowView)dataGrid.SelectedItem;
            var value = item.Row.ItemArray[0].ToString();

            try
            {
                doc.Elements("Tours").Descendants().Where(x => x.Value == value).First().Parent.Remove();
            }
            catch { }

            doc.Save(filePath);
            ItemsRefresh();
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var item = (DataRowView)dataGrid.SelectedItem;
            var value = item.Row.ItemArray[0].ToString();

            EditWindow ew = new EditWindow((DataRowView)dataGrid.SelectedItem);
            ew.ShowDialog();

            try
            {
                XElement edited = new XElement("Tour",
                    new XElement(Name = "Identifier", value),
                    new XElement(Name = "Name", ew.tbName.Text),
                    new XElement(Name = "Info", ew.tbInfo.Text),
                    new XElement(Name = "Price", ew.tbPrice.Text),
                    new XElement(Name = "Seasons", ew.tbSeasons.Text));

                doc.Elements("Tours").Descendants().Where(x => x.Value == value).First().Parent.ReplaceWith(edited);
            }
            catch { }

            doc.Save(filePath);
            ItemsRefresh();
        }
    }
}
