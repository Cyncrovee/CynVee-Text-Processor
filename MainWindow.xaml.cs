using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        
        int[] fontSize = Enumerable.Range(1, 256).ToArray();
        foreach (int x in fontSize)
        {
            FontSizeBox.Items.Add(x);
        }
    }
    
    private string _filePath = string.Empty;
    private string _folderPath = string.Empty;

    private bool _isTextBoxFocused = false;
    
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
    // "View"
    private void TextBoxFocusButton_OnClick(object sender, RoutedEventArgs e)
    {
        switch (_isTextBoxFocused)
        {
            case true:
                ItalicsButton.Visibility = Visibility.Visible;
                BoldButton.Visibility = Visibility.Visible;
                UnderlineButton.Visibility = Visibility.Visible;
                SubscriptButton.Visibility = Visibility.Visible;
                SuperscriptButton.Visibility = Visibility.Visible;
                AlignLeftButton.Visibility = Visibility.Visible;
                AlignCenterButton.Visibility = Visibility.Visible;
                AlignRightButton.Visibility = Visibility.Visible;
                CurrentFileTextBlock.Visibility = Visibility.Visible;
                CurrentFolderTextBlock.Visibility = Visibility.Visible;
                FileList.Visibility = Visibility.Visible;
                _isTextBoxFocused = false;
                break;
            case false:
                ItalicsButton.Visibility = Visibility.Collapsed;
                BoldButton.Visibility = Visibility.Collapsed;
                UnderlineButton.Visibility = Visibility.Collapsed;
                SubscriptButton.Visibility = Visibility.Collapsed;
                SuperscriptButton.Visibility = Visibility.Collapsed;
                AlignLeftButton.Visibility = Visibility.Collapsed;
                AlignCenterButton.Visibility = Visibility.Collapsed;
                AlignRightButton.Visibility = Visibility.Collapsed;
                CurrentFileTextBlock.Visibility = Visibility.Collapsed;
                CurrentFolderTextBlock.Visibility = Visibility.Collapsed;
                FileList.Visibility = Visibility.Collapsed;
                _isTextBoxFocused = true;
                break;
        }
    }
    
    
    // Left side button functions
    private void ItalicsButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.ToggleItalic.Execute(null, Editor);
    }
    private void BoldButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.ToggleBold.Execute(null, Editor);
    }
    private void UnderlineButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.ToggleUnderline.Execute(null, Editor);
    }
    private void SubscriptButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.ToggleSubscript.Execute(null, Editor);
    }
    private void SuperscriptButton_OnClick(object sender, RoutedEventArgs e)
    {
        EditingCommands.ToggleSuperscript.Execute(null, Editor);
    }
    
    
    // Font size box function
    private void FontSizeBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Editor.FontSize = FontSizeBox.SelectedIndex;
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
    
    // Functions for Editor and FileList
    private void Editor_OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        var selection = Editor.Selection;
        if(selection != null)
        {
            if (selection.GetPropertyValue(TextElement.FontWeightProperty).ToString() == FontWeights.Bold.ToString())
            {
                BoldButton.IsChecked = true;
            }
            else
            {
                BoldButton.IsChecked = false;
            }
            if (selection.GetPropertyValue(TextElement.FontStyleProperty).ToString() == FontStyles.Italic.ToString())
            {
                ItalicsButton.IsChecked = true;
            }
            else
            {
                ItalicsButton.IsChecked = false;
            }
        }
    }
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