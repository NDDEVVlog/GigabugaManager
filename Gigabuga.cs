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
        private bool isFindingMode = false;
        private List<string> currentSearchTags = new List<string>();
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
            TagManager.Instance.gigabuga = this;
            LoadFileItems();
            LoadFileFormats();
            FileFormatSelection.Items.Add("None");
            // Create and add the drag over panel
            dragOverPanel = CreateDragOverPanel();
            ListItemView.Controls.Add(dragOverPanel);
            FindingMode.Click += FindingMode_Click;
            adjustProperties.Click += AdjustProperties_Click;
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
                GigabugaFinding.Instance.AddSong(filePath, tags);
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

        public void RemoveTagFromFileItems(string tagName)
        {
            UpdateTagInFileItems(tagName, null); // Passing null to indicate tag removal
        }

        public void UpdateTagInFileItems(string oldTagName, string newTagName)
        {
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
                    if (parts.Length >= 3)
                    {
                        string tags = parts[2];
                        List<string> tagList = tags.Split('|')
                            .Select(t => t.Trim())
                            .Where(t => !string.IsNullOrWhiteSpace(t))
                            .ToList();

                        if (tagList.Contains(oldTagName))
                        {
                            if (newTagName != null)
                            {
                                // Rename the tag
                                tagList[tagList.IndexOf(oldTagName)] = newTagName;
                            }
                            else
                            {
                                // Remove the tag
                                tagList.Remove(oldTagName);
                            }

                            string updatedTags = string.Join(" | ", tagList);
                            lines[i] = $"{parts[0]},{parts[1]},{updatedTags}";
                        }
                    }
                }

                // Write the updated entries back to the file
                File.WriteAllLines(path, lines);
            }

            // Update tags in the main file
            UpdateTagsInFile(fileItemsPath);

            // Update tags in the format-specific files
            foreach (string formatFile in Directory.GetFiles(".", "*.txt"))
            {
                if (formatFile != "FileItem.txt" && formatFile != "Tags.txt" && formatFile != "Fileformat.txt")
                {
                    UpdateTagsInFile(formatFile);
                }
            }

            // Update tags in the ListView
            LoadFileItems();
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
                               

                                // Add the icon to the ImageList and get its key
                                string iconKey = Guid.NewGuid().ToString(); // Generate a unique key
                                

                                // Create a new ListViewItem
                                ListViewItem item = new ListViewItem
                                {
                                    ImageKey = iconKey, // Set the image key to the generated key
                                    Text = fileName // This will set the text for the file name column
                                };
                                item.ImageKey = iconKey;
                                // Add file path and tags as sub-items
                                item.SubItems.Add(fileName);
                                item.SubItems.Add(filePath); // File path in the second column
                                item.SubItems.Add(tags); // Tags in the third column

                                // Add the ListViewItem to the ListView
                                ListItemView.Items.Add(item);
                                GigabugaFinding.Instance.AddSong(filePath, tags);
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
            TagManager.Instance.AddTag(tagName,TagFlow,TagButton_Click);
        }

        // Method to initialize ListView columns
        private void InitializeListViewColumns()
        {
            ListItemView.Columns.Clear();

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "Icon";
            columnHeader1.Width = 50;

            columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "File Name";
            columnHeader2.Width = 200;

            columnHeader3 = new ColumnHeader();
            columnHeader3.Text = "File Path";
            columnHeader3.Width = 300;

            columnHeader4 = new ColumnHeader();
            columnHeader4.Text = "Tags";
            columnHeader4.Width = 300;

            ListItemView.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
        }

        private void TagButton_Click(object sender, EventArgs e)
        {
            Button tagButton = sender as Button;
            string tagName = tagButton.Text;

            if (isFindingMode)
            {
                if (currentSearchTags.Contains(tagName))
                {
                    currentSearchTags.Remove(tagName);
                    tagButton.BackColor = Color.LightGray;
                }
                else
                {
                    currentSearchTags.Add(tagName);
                    tagButton.BackColor = Color.LightGreen;
                }

                ListItemView.Items.Clear();

                // Fetch files matching the current search tags
                string combinedTags = string.Join("|", currentSearchTags);
                var matchingFiles = GigabugaFinding.Instance.FindSongs(combinedTags);

                // Display matching files in the ListView
                foreach (string filePath in matchingFiles)
                {
                    string fileName = Path.GetFileName(filePath);
                    try
                    {
                        Icon fileIcon = Icon.ExtractAssociatedIcon(filePath);
                        string iconKey = Guid.NewGuid().ToString();
                        LargeIconImage.Images.Add(iconKey, fileIcon);

                        ListViewItem item = new ListViewItem
                        {
                            ImageKey = iconKey,
                            Text = fileName
                        };
                        item.ImageKey = iconKey;
                        item.SubItems.Add(fileName);
                        item.SubItems.Add(filePath);
                        item.SubItems.Add(""); // Placeholder for tags

                        ListItemView.Items.Add(item);
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
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
                        string filePath = selectedItem.SubItems[2].Text;
                        UpdateTags(fileName, newTags);
                        GigabugaFinding.Instance.AddSong(filePath, newTags);
                    }
                }
            }
            if (adjust)
            {
                try
                {
                    if (sender is Button )
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
            dragOverPanel.Visible = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    SaveFile(fileName, filePath);

                    Icon fileIcon = Icon.ExtractAssociatedIcon(filePath);
                    string iconKey = Guid.NewGuid().ToString();
                    LargeIconImage.Images.Add(iconKey, fileIcon);

                    ListViewItem item = new ListViewItem { ImageKey = iconKey };
                    item.SubItems.Add(fileName);
                    item.SubItems.Add(filePath);
                    item.SubItems.Add(""); // Placeholder for tags

                    ListItemView.Items.Add(item);
                    FileItem fileItem = new FileItem(fileIcon, fileName, filePath);
                    item.Tag = fileItem;

                    GigabugaFinding.Instance.AddSong(filePath, null);

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

        private void AdjustProperties_Click(object sender, EventArgs e)
        {
            adjust = !adjust;
            adjustProperties.Checked = adjust;
        }

        private void ListItemView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle ListView selection changes if needed...
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Additional load logic if needed...
        }

        private void FindingMode_Click(object sender, EventArgs e)
        {
            isFindingMode = !isFindingMode;
            FindingMode.Checked = isFindingMode;

            // Update the appearance of tag buttons based on finding mode
            foreach (Button tagButton in TagFlow.Controls.OfType<Button>())
            {
                if (isFindingMode)
                {
                    tagButton.BackColor = currentSearchTags.Contains(tagButton.Text) ? Color.LightGreen : Color.LightGray;
                }
                else
                {
                    tagButton.BackColor = Color.FromKnownColor(KnownColor.Control);
                }
            }

            // Clear the current search tags if exiting finding mode
            if (!isFindingMode)
            {
                currentSearchTags.Clear();
                ListItemView.Items.Clear();  // Clear the list view when exiting finding mode
                LoadFileItems();  // Reload all file items
            }
        }
    }
}
