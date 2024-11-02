using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
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
    private string _folderPath = string.Empty;

    // Functions for menu
    // "File"
    private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            if (File.Exists(openFileDialog.FileName))
            {
                _filePath = openFileDialog.FileName;
                LoadFile();
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }
        else
        {
            Console.WriteLine("No file selected");
        }
    }
    private void OpenFolderButton_OnClick(object sender, RoutedEventArgs e)
    {
        var openFolderDialog = new OpenFolderDialog();
        if (openFolderDialog.ShowDialog() == true)
        {
            if (Directory.Exists(openFolderDialog.FolderName))
            {
                _folderPath = openFolderDialog.FolderName;
                LoadFolder();
            }
            else
            {
                Console.WriteLine("Folder not found");
            }
        }
        else
        {
            Console.WriteLine("No folder selected");
        }
    }
    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_filePath != string.Empty)
        {
            var fileStream = new FileStream(_filePath, FileMode.Create);
            var range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();
            LoadFolder();
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
            LoadFolder();
        }
        else
        {
            Console.WriteLine("No file saved");
        }
    }
    private void ExitButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    // "Edit"
    private void UndoButton_OnClick(object sender, RoutedEventArgs e)
    {
        Editor.Undo();
    }
    private void RedoButton_OnClick(object sender, RoutedEventArgs e)
    {
        Editor.Redo();
    }
    private void CutButton_OnClick(object sender, RoutedEventArgs e)
    {
        Editor.Cut();
    }
    private void CopyButton_OnClick(object sender, RoutedEventArgs e)
    {
        Editor.Copy();
    }
    private void PasteButton_OnClick(object sender, RoutedEventArgs e)
    {
        Editor.Paste();
    }
    private void SelectAllButton_OnClick(object sender, RoutedEventArgs e)
    {
        Editor.SelectAll();
    }
    private void ClearAllButton_OnClick(object sender, RoutedEventArgs e)
    {
        Editor.Document.Blocks.Clear();
    }

    
    // Functions for Editor and FileList
    private void FileList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (File.Exists(_filePath) | FileList.SelectedItem != null)
        {
            _filePath = FileList.SelectedItem.ToString();
            LoadFile();
        }
        else
        {
            Console.WriteLine("File not found");
        }
    }
    
    
    // Left side button functions
    private void ItalicsButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (Editor.FontStyle == FontStyles.Normal)
        {
            Editor.FontStyle = FontStyles.Italic;
        }
        else
        {
            Editor.FontStyle = FontStyles.Normal;
        }
    }
    private void BoldButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (Editor.FontWeight == FontWeights.Normal)
        {
            Editor.FontWeight = FontWeights.Bold;
        }
        else
        {
            Editor.FontWeight = FontWeights.Normal;
        }
    }
    private void UnderlineButton_OnClick(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("Underline Button");
    }
    
    
    // Right side button functions
    private void AlignLeftButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.AlignLeft.Execute(null, Editor);
    }
    private void AlignCenterButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.AlignCenter.Execute(null, Editor);
    }
    private void AlignRightButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.AlignRight.Execute(null, Editor);
    }
    
    // Non-Event functions
    private void LoadFile()
    {
        if (_filePath != string.Empty)
        {
            Editor.Document.Blocks.Clear();
            var fileStream = new FileStream(_filePath, FileMode.Open);
            var range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            if (_filePath.EndsWith(".rtf"))
            {
                range.Load(fileStream, DataFormats.Rtf);
            }
            else if (_filePath.EndsWith(".txt"))
            {
                range.Load(fileStream, DataFormats.Text);
            }
            else
            {
                range.Load(fileStream, DataFormats.Text);
            }
            fileStream.Close();
            CurrentFileTextBlock.Text = "Current File: " + _filePath;
        }
        else
        {
            Console.WriteLine("File not found");
        }
    }
    private void LoadFolder()
    {
        if (_folderPath != string.Empty)
        {
            string[] files = Directory.GetFiles(_folderPath);
            FileList.Items.Clear();
            foreach (string file in files)
            {
                FileList.Items.Add(file);
            }
            CurrentFolderTextBlock.Text = "Current Folder: " + _folderPath;
        }
        else
        {
            Console.WriteLine("Folder not found");
        }
    }
}