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

namespace GiGaBuGaManager.AdditionClass
{
    public class FileManager
    {
        private static FileManager instance = null;
        private static readonly object padlock = new object();

        private string fileItemsPath = "FileItem.txt";
        private string fileFormatPath = "Fileformat.txt";

        // Private constructor to prevent instantiation
        private FileManager() { }

        // Public static method to provide the single instance
        public static FileManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new FileManager();
                    }
                    return instance;
                }
            }
        }

        // Save a file item and its format
        public void SaveFile(string fileName, string filePath, string tags = "")
        {
            string fileFormat = Path.GetExtension(fileName).TrimStart('.').ToUpper();
            string formatFilePath = $"{fileFormat}.txt";

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
                        lines[i] = entry;
                        fileItemExists = true;
                        break;
                    }
                }

                if (!fileItemExists)
                {
                    lines.Add(entry);
                }

                File.WriteAllLines(path, lines);
            }

            UpdateOrAddEntry(formatFilePath, $"{fileName},{filePath},{tags}");
            UpdateOrAddEntry(fileItemsPath, $"{fileName},{filePath},{tags}");

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

                File.WriteAllLines(path, lines);
            }

            UpdateOrAddFileFormat(fileFormatPath, fileFormat);
        }

        public void RemoveTagFromFileItems(string tagName)
        {
            UpdateTagInFileItems(tagName, null);
        }

        public void UpdateTagInFileItems(string oldTagName, string newTagName)
        {
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
                        List<string> tagList = tags.Split('|').Select(t => t.Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

                        if (tagList.Contains(oldTagName))
                        {
                            if (newTagName != null)
                            {
                                tagList[tagList.IndexOf(oldTagName)] = newTagName;
                            }
                            else
                            {
                                tagList.Remove(oldTagName);
                            }

                            string updatedTags = string.Join(" | ", tagList);
                            lines[i] = $"{parts[0]},{parts[1]},{updatedTags}";
                        }
                    }
                }

                File.WriteAllLines(path, lines);
            }

            UpdateTagsInFile(fileItemsPath);

            foreach (string formatFile in Directory.GetFiles(".", "*.txt"))
            {
                if (formatFile != "FileItem.txt" && formatFile != "Tags.txt" && formatFile != "Fileformat.txt")
                {
                    UpdateTagsInFile(formatFile);
                }
            }
        }

        public void LoadFileFormats(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            if (File.Exists(fileFormatPath))
            {
                string[] formats = File.ReadAllLines(fileFormatPath);

                foreach (string format in formats)
                {
                    if (!string.IsNullOrWhiteSpace(format))
                    {
                        comboBox.Items.Add(format);
                    }
                }
            }
        }

        public void LoadFileItems(ListView listView, ImageList imageList)
        {
            listView.Items.Clear();

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
                                Icon fileIcon = Icon.ExtractAssociatedIcon(filePath);
                                string iconKey = Guid.NewGuid().ToString();
                                imageList.Images.Add(iconKey, fileIcon);

                                ListViewItem item = new ListViewItem
                                {
                                    ImageKey = iconKey,
                                    Text = fileName
                                };
                                item.SubItems.Add(fileName);
                                item.SubItems.Add(filePath);
                                item.SubItems.Add(tags);

                                listView.Items.Add(item);
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

        public void UpdateTags(string fileName, string newTags)
        {
            string fileFormat = Path.GetExtension(fileName).TrimStart('.').ToUpper();
            string formatFilePath = $"{fileFormat}.txt";

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

                File.WriteAllLines(path, lines);
            }

            UpdateTagsInFile(fileItemsPath);
            UpdateTagsInFile(formatFilePath);
        }
    }
}
