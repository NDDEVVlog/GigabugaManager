using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using GiGaBuGaManager.AdditionClass;

namespace GiGaBuGaManager
{
    public partial class Gigabuga : Form
    {
        #region Private Variables

        private Panel dragOverPanel;
        private ImageList LargeIconImage;
        private string fileItemsPath = "FileItem.txt"; // Path to the file item storage

        private bool adjust = false;
        #endregion

        private Panel CreateDragOverPanel()
        {
            Panel dragOverPanel = new Panel
            {
                Size = new Size(300, 300),
                BackColor = Color.FromArgb(10, 110, 255, 4),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            Label label = new Label
            {
                Text = "Browse File to upload!",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black
            };

            dragOverPanel.Controls.Add(label);

            // Position the panel in the center of the ListView
            dragOverPanel.Location = new Point(
                (ListItemView.Width - dragOverPanel.Width) / 2,
                (ListItemView.Height - dragOverPanel.Height) / 2
            );

            return dragOverPanel;
        }

        public Gigabuga()
        {
            InitializeComponent();

            // Enable drag and drop for the ListView
            ListItemView.AllowDrop = true;
            ListItemView.DragEnter += ListItemView_DragEnter;
            ListItemView.DragLeave += ListItemView_DragLeave;
            ListItemView.DragDrop += ListItemView_DragDrop;
            ListItemView.MouseDoubleClick += ListItemView_MouseDoubleClick;

            // Initialize ListView columns and LargeIconImageList
            InitializeListViewColumns();
            LargeIconImage = new ImageList { ImageSize = new Size(48, 48) };
            ListItemView.LargeImageList = LargeIconImage;

            // Load tags and file items
            TagManager.Instance.LoadTags(TagFlow,TagButton_Click);
            LoadFileItems();
            LoadFileFormats();
            FileFormatSelection.Items.Add("None");
            // Create and add the drag over panel
            dragOverPanel = CreateDragOverPanel();
            ListItemView.Controls.Add(dragOverPanel);
        }

        #region Save and Load
        // Method to save a file item and its format
        private void SaveFile(string fileName, string filePath, string tags = "")
        {
            // Determine the file format
            string fileFormat = Path.GetExtension(fileName).TrimStart('.').ToUpper();

            // Define paths for the files
            string fileItemsPath = "FileItem.txt";
            string formatFilePath = $"{fileFormat}.txt";
            string fileFormatPath = "Fileformat.txt";

            // Function to update or add an entry in a specified file
            void UpdateOrAddEntry(string path, string entry)
            {
                List<string> lines = new List<string>();
                if (File.Exists(path))
                {
                    lines = File.ReadAllLines(path).ToList();
                }

                bool fileItemExists = false;
                for (int i = 0; i < lines.Count; i++)
                {
                    string[] parts = lines[i].Split(',');
                    if (parts.Length >= 2 && parts[0] == fileName)
                    {
                        // Update the existing entry
                        lines[i] = entry;
                        fileItemExists = true;
                        break;
                    }
                }

                // If the file item doesn't exist, add a new entry
                if (!fileItemExists)
                {
                    lines.Add(entry);
                }

                // Write the updated entries back to the file
                File.WriteAllLines(path, lines);
            }

            // Update or add entry in the format-specific file
            UpdateOrAddEntry(formatFilePath, $"{fileName},{filePath},{tags}");
            // Update or add entry in the main file
            UpdateOrAddEntry(fileItemsPath, $"{fileName},{filePath},{tags}");

            // Function to update or add file format in the Fileformat.txt
            void UpdateOrAddFileFormat(string path, string format)
            {
                List<string> lines = new List<string>();
                if (File.Exists(path))
                {
                    lines = File.ReadAllLines(path).ToList();
                }

                if (!lines.Contains(format))
                {
                    lines.Add(format);
                }

                // Write the updated formats back to the file
                File.WriteAllLines(path, lines);
            }

            // Update or add file format in the Fileformat.txt
            UpdateOrAddFileFormat(fileFormatPath, fileFormat);
        }

        private void LoadFileFormats()
        {
            string fileFormatPath = "Fileformat.txt";

            // Clear existing items
            FileFormatSelection.Items.Clear();

            // Check if the file exists
            if (File.Exists(fileFormatPath))
            {
                // Read all lines from the file
                string[] formats = File.ReadAllLines(fileFormatPath);

                // Add each format to the ComboBox
                foreach (string format in formats)
                {
                    if (!string.IsNullOrWhiteSpace(format))
                    {
                        FileFormatSelection.Items.Add(format);
                    }
                }
            }
        }

        private void LoadFileItems()
        {
            ListItemView.Items.Clear();

            if (File.Exists(fileItemsPath))
            {
                using (StreamReader reader = new StreamReader(fileItemsPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length >= 2)
                        {
                            string fileName = parts[0];
                            string filePath = parts[1];
                            string tags = parts.Length > 2 ? parts[2] : "";

                            try
                            {
                                Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(filePath);

                                // Add the icon to the ImageList and get its key
                                string iconKey = Guid.NewGuid().ToString(); // Generate a unique key
                                //LargeIconImage.Images.Add(iconKey, fileIcon);

                                // Create a new ListViewItem
                                ListViewItem item = new ListViewItem();
                                item.ImageKey = iconKey; // Set the image key to the generated key
                                item.SubItems.Add(fileName); // Add file name as sub-item
                                item.SubItems.Add(filePath); // Add file path as sub-item
                                item.SubItems.Add(tags);
                                // Create a new FileItem
                                FileItem fileItem = new FileItem(fileIcon, fileName, filePath);

                                // Set the ListViewItem's Tag to the FileItem
                                item.Tag = fileItem;

                                // Add the ListViewItem to the ListView
                                ListItemView.Items.Add(item);

                                // Display a message box showing the icon, file name, and file path
                                //MessageBox.Show($"File: {fileName}\nPath: {filePath}", "File Information",
                                //        MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateTags(string fileName, string newTags)
        {
            // Determine the file format
            string fileFormat = Path.GetExtension(fileName).TrimStart('.').ToUpper();

            // Define paths for the files
            string fileItemsPath = "FileItem.txt";
            string formatFilePath = $"{fileFormat}.txt";

            // Function to update tags in a specified file
            void UpdateTagsInFile(string path)
            {
                List<string> lines = new List<string>();
                if (File.Exists(path))
                {
                    lines = File.ReadAllLines(path).ToList();
                }

                for (int i = 0; i < lines.Count; i++)
                {
                    string[] parts = lines[i].Split(',');
                    if (parts.Length >= 2 && parts[0] == fileName)
                    {
                        string filePath = parts[1];
                        lines[i] = $"{fileName},{filePath},{newTags}";
                        break;
                    }
                }

                // Write the updated entries back to the file
                File.WriteAllLines(path, lines);
            }

            // Update tags in the main file
            UpdateTagsInFile(fileItemsPath);

            // Update tags in the format-specific file
            UpdateTagsInFile(formatFilePath);
        }
        #endregion

        // Other methods and event handlers...

        private void TagName_TextChanged(object sender, EventArgs e)
        {
            // Add your implementation here if needed
        }

        private void AddTagButton_Click(object sender, EventArgs e)
        {
            // Get the text from the TagName TextBox
            string tagName = TagName.Text;

            // Add the tag using TagManager
            TagManager.Instance.AddTag(tagName,TagFlow);
        }

        // Method to initialize ListView columns
        private void InitializeListViewColumns()
        {
            // Create the column headers
            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "Icon"; // Column header text for icon
            columnHeader1.Width = 50; // Set the width of the icon column

            columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "File Name"; // Column header text for file name
            columnHeader2.Width = 200; // Set the width of the file name column

            columnHeader3 = new ColumnHeader();
            columnHeader3.Text = "File Path"; // Column header text for file path
            columnHeader3.Width = 300; // Set the width of the file path column

            columnHeader4 = new ColumnHeader();
            columnHeader4.Text = "Tags"; // Column header text for tags
            columnHeader4.Width = 300; // Set the width of the tags column

            // Add the column headers to the ListView
            ListItemView.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
        }

        private void TagButton_Click(object sender, EventArgs e)
        {
            string tagName = ((Button)sender).Text;

            foreach (ListViewItem selectedItem in ListItemView.SelectedItems)
            {
                if (selectedItem.SubItems.Count < 4)
                {
                    selectedItem.SubItems.Add("| " + tagName);
                }
                else
                {
                    string currentTags = selectedItem.SubItems[3].Text;
                    List<string> tagList = currentTags.Split('|').Select(t => t.Trim()).ToList();

                    if (!tagList.Contains(tagName))
                    {
                        tagList.Add(tagName);
                        tagList = tagList.OrderBy(t => t.Split(' ')[0]).ThenBy(t => t.Length).ToList();
                        selectedItem.SubItems[3].Text = string.Join(" | ", tagList);
                    }
                    else
                    {
                        MessageBox.Show("Tag already added to one or more selected files, so it will be removed.", "Duplicate Tag will be removed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Remove the tag if it already exists
                        tagList.Remove(tagName);
                        selectedItem.SubItems[3].Text = tagList.Count > 0 ? string.Join(" | ", tagList) : string.Empty;
                    }

                    // Update the tags in the file
                    string fileName = selectedItem.SubItems[1].Text;
                    string newTags = selectedItem.SubItems[3].Text;
                    UpdateTags(fileName, newTags);
                }
            }

            if (adjust)
            {
                try
                {
                    if (sender is Button tagButton)
                    {
                        AdjustTag adjustTagForm = new AdjustTag(tagButton, TagFlow);
                        adjustTagForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Sender is not a Button.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error changing tag properties: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ListItemView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

            // Show the drag over panel
            dragOverPanel.Visible = true;
        }

        private void ListItemView_DragLeave(object sender, EventArgs e)
        {
            // Hide the drag over panel when the drag operation leaves the ListView
            dragOverPanel.Visible = false;
        }

        private void ListItemView_DragDrop(object sender, DragEventArgs e)
        {
            // Hide the drag over panel when the drag and drop operation completes
            dragOverPanel.Visible = false;

            // Check if the data being dragged is a file
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the file paths from the data
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Loop through the file paths
                foreach (string filePath in filePaths)
                {
                    // Get the file name from the file path
                    string fileName = Path.GetFileName(filePath);

                    // Save the file item
                    SaveFile(fileName, filePath);

                    // Create a new ListViewItem
                    ListViewItem item = new ListViewItem();
                    item.Text = fileName; // Set the file name as the text

                    // Add the file icon to the ImageList
                    Icon fileIcon = Icon.ExtractAssociatedIcon(filePath);
                    LargeIconImage.Images.Add(fileName, fileIcon);

                    // Set the ListViewItem's ImageKey to the file name
                    item.ImageKey = fileName;

                    // Add the file path as a sub-item
                    item.SubItems.Add(filePath);

                    // Create a new FileItem
                    FileItem fileItem = new FileItem(fileIcon, fileName, filePath);

                    // Set the ListViewItem's Tag to the FileItem
                    item.Tag = fileItem;

                    // Add the ListViewItem to the ListView
                    ListItemView.Items.Add(item);

                    // Optionally, display a message box showing the icon, file name, and file path
                    //MessageBox.Show($"File: {fileName}\nPath: {filePath}", "File Information",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ListItemView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem selectedItem = ListItemView.GetItemAt(e.X, e.Y);

            if (selectedItem != null)
            {
                string filePath = selectedItem.SubItems[1].Text;

                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void adjustProperties_CheckedChanged_1(object sender, EventArgs e)
        {
            adjust = !adjust;
        }

        private void ListItemView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle ListView selection changes if needed...
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Additional load logic if needed...
        }
    }
}
