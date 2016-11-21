using KmlEditorLibrary;
using Microsoft.Win32;
using SharpKml.Dom;
using SharpKml.Engine;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KmlEditorWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KmlFile kmlFile = null;
        String fileName = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenMenu_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog.Filter = "Google Earth (.kml .kmz)|*.kml;*.kmz";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                KmlFile kmlFile = KmlFileHelper.OpenFile(openFileDialog.FileName);
                FerromapasKmlHelper.AddFerromapasSchemaIfNotExists(kmlFile);
                String fileName = ((kmlFile.Root as Kml).Feature as Document).Name;
                this.Title = fileName;
                this.kmlFile = kmlFile;
                kmlTreeView.kmlFile = kmlFile;
                this.fileName = openFileDialog.FileName;
            }
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SaveMenu_Click(object sender, RoutedEventArgs e)
        {
            KmlFileHelper.SaveFile(kmlFile, fileName);
        }

        private void SaveAsMenu_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Set filter options and filter index.
            saveFileDialog.Filter = "Google Earth (.kml .kmz)|*.kml;*.kmz";
            saveFileDialog.FilterIndex = 1;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = saveFileDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                KmlFileHelper.SaveFile(kmlFile, saveFileDialog.FileName);
                fileName = saveFileDialog.FileName;
            }
        }
    }
}
