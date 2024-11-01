using System.IO;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;

namespace CynVee_Text_Processor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private string _filePath = string.Empty;

    // Functions for menu
    // "File"
    private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            if (File.Exists(openFileDialog.FileName))
            {
                var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                var range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
                _filePath = openFileDialog.FileName;
                fileStream.Close();
            }
        }
    }
    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_filePath != string.Empty)
        {
            FileStream fileStream = new FileStream(_filePath, FileMode.Create);
            TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();
        }
        else
        {
            Console.WriteLine("No file selected");
        }
    }
    private void SaveAsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
        if(saveFileDialog.ShowDialog() == true)
        {
            FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
            TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
            _filePath = saveFileDialog.FileName;
            fileStream.Close();
        }
    }
    private void ExitButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}