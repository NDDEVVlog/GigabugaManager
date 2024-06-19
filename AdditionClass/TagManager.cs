using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GiGaBuGaManager.AdditionClass
{
    /// <summary>
    /// Singleton class for managing tags in the application.
    /// </summary>
    public class TagManager
    {
        private static TagManager instance = null;
        private static readonly object padlock = new object();
        private readonly string tagsFilePath = "Tags.txt";

        

        public Gigabuga gigabuga;
        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private TagManager() { }

        /// <summary>
        /// Gets the singleton instance of the TagManager.
        /// </summary>
        public static TagManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TagManager();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Saves the current tags to a file.
        /// </summary>
        /// <param name="tagFlow">The FlowLayoutPanel containing tag buttons.</param>
        public void SaveTags(FlowLayoutPanel tagFlow)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(tagsFilePath))
                {
                    foreach (Button tagButton in tagFlow.Controls.OfType<Button>())
                    {
                        string tagName = tagButton.Text;
                        Color buttonColor = tagButton.FlatAppearance.BorderColor;
                        Color backgroundColor = tagButton.BackColor;
                        Color textColor = tagButton.ForeColor;
                        
                        string tagInfo = $"{tagName},{buttonColor.ToArgb()},{backgroundColor.ToArgb()},{textColor.ToArgb()}";
                        writer.WriteLine(tagInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving tags: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads tags from a file and adds them to the FlowLayoutPanel.
        /// </summary>
        /// <param name="tagFlow">The FlowLayoutPanel to add tag buttons to.</param>
        /// <param name="tagButtonClick">Event handler for tag button click events.</param>
        public void LoadTags(FlowLayoutPanel tagFlow, EventHandler tagButtonClick)
        {
            try
            {
                if (File.Exists(tagsFilePath))
                {
                    using (StreamReader reader = new StreamReader(tagsFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');

                            if (parts.Length == 4)
                            {
                                string tagName = parts[0];
                                Color buttonColor = Color.FromArgb(int.Parse(parts[1]));
                                Color backgroundColor = Color.FromArgb(int.Parse(parts[2]));
                                Color textColor = Color.FromArgb(int.Parse(parts[3]));
                                Color borderColor= Color.FromArgb(int.Parse(parts[3]));
                                // Create and add the button to the panel
                                AddTagButton(tagFlow, tagName, buttonColor, backgroundColor, textColor, tagButtonClick);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tags: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Adds a new tag to the FlowLayoutPanel and saves it to the file.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="tagFlow">The FlowLayoutPanel to add the tag button to.</param>
        /// <param name="tagButtonClick">Event handler for tag button click events.</param>
        public void AddTag(string tagName, FlowLayoutPanel tagFlow, EventHandler tagButtonClick)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                MessageBox.Show("Tag name cannot be empty.", "Invalid Tag Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the tag already exists
            bool tagExists = tagFlow.Controls.OfType<Button>().Any(button => button.Text.Equals(tagName, StringComparison.OrdinalIgnoreCase));

            if (!tagExists)
            {
                // Default colors for a new tag
                Color buttonColor = Color.LightGray;
                Color backgroundColor = Color.White;
                Color textColor = Color.Black;

                // Add the new tag button
                AddTagButton(tagFlow, tagName, buttonColor, backgroundColor, textColor, tagButtonClick);

                // Save the updated tags
                SaveTags(tagFlow);
            }
            else
            {
                MessageBox.Show("Tag already exists.", "Duplicate Tag", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Creates and adds a tag button to the FlowLayoutPanel.
        /// </summary>
        /// <param name="tagFlow">The FlowLayoutPanel to add the tag button to.</param>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="buttonColor">The color of the button.</param>
        /// <param name="backgroundColor">The background color of the button.</param>
        /// <param name="textColor">The text color of the button.</param>
        /// <param name="tagButtonClick">Event handler for tag button click events.</param>
        private void AddTagButton(FlowLayoutPanel tagFlow, string tagName, Color buttonColor, Color backgroundColor, Color textColor, EventHandler tagButtonClick)
        {
            Button tagButton = new Button
            {
                Text = tagName,
                BackColor = backgroundColor,
                ForeColor = textColor,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat
            };

            tagButton.FlatAppearance.BorderColor = buttonColor;
            tagButton.Click += tagButtonClick;

            tagFlow.Controls.Add(tagButton);
        }

        /// <summary>
        /// Updates the color of a tag button.
        /// </summary>
        /// <param name="tagName">The name of the tag to update.</param>
        /// <param name="tagFlow">The FlowLayoutPanel containing tag buttons.</param>
        /// <param name="newButtonColor">The new button color.</param>
        /// <param name="newBackgroundColor">The new background color.</param>
        /// <param name="newTextColor">The new text color.</param>
        public void UpdateTagColor(string tagName, FlowLayoutPanel tagFlow, Color newButtonColor, Color newBackgroundColor, Color newTextColor)
        {
            // Find the button for the specified tag
            Button tagButton = tagFlow.Controls
                .OfType<Button>()
                .FirstOrDefault(button => button.Text.Equals(tagName, StringComparison.OrdinalIgnoreCase));

            if (tagButton != null)
            {
                // Update button colors
                tagButton.BackColor = newBackgroundColor;
                tagButton.ForeColor = newTextColor;
                tagButton.FlatAppearance.BorderColor = newButtonColor;

                // Optionally save changes
                SaveTags(tagFlow);
            }
            else
            {
                MessageBox.Show($"Tag '{tagName}' not found.", "Tag Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Deletes a tag from the FlowLayoutPanel and updates the tags file.
        /// </summary>
        /// <param name="tagName">The name of the tag to delete.</param>
        /// <param name="tagFlow">The FlowLayoutPanel containing tag buttons.</param>
        public void DeleteTag(string tagName, FlowLayoutPanel tagFlow)
        {
            // Find the button for the specified tag
            Button tagButton = tagFlow.Controls
                .OfType<Button>()
                .FirstOrDefault(button => button.Text.Equals(tagName, StringComparison.OrdinalIgnoreCase));

            if (tagButton != null)
            {
                // Remove the button from the FlowLayoutPanel
                tagFlow.Controls.Remove(tagButton);

                // Optionally dispose the button to free resources
                tagButton.Dispose();
                
                FileManager.Instance.RemoveTagFromFileItems(tagName);
                // Save the updated tags
                SaveTags(tagFlow);
            }
            else
            {
                MessageBox.Show($"Tag '{tagName}' not found.", "Tag Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Renames a tag in the FlowLayoutPanel and updates the tags file.
        /// </summary>
        /// <param name="oldTagName">The current name of the tag.</param>
        /// <param name="newTagName">The new name for the tag.</param>
        /// <param name="tagFlow">The FlowLayoutPanel containing tag buttons.</param>
        public void RenameTag(string oldTagName, string newTagName, FlowLayoutPanel tagFlow)
        {
            if (string.IsNullOrWhiteSpace(newTagName))
            {
                MessageBox.Show("New tag name cannot be empty.", "Invalid Tag Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Find the button for the specified tag
            Button tagButton = tagFlow.Controls
                .OfType<Button>()
                .FirstOrDefault(button => button.Text.Equals(oldTagName, StringComparison.OrdinalIgnoreCase));

            if (tagButton != null)
            {
                // Check if the new tag name already exists
                bool tagExists = tagFlow.Controls.OfType<Button>()
                    .Any(button => button.Text.Equals(newTagName, StringComparison.OrdinalIgnoreCase));

                if (!tagExists)
                {
                    // Update the tag name
                    var oldName = tagButton.Text;
                    tagButton.Text = newTagName;
                    FileManager.Instance.UpdateTagInFileItems(oldName, tagButton.Text);
                    // Save the updated tags
                    SaveTags(tagFlow);
                }
                else
                {
                    MessageBox.Show("A tag with the new name already exists.", "Duplicate Tag", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show($"Tag '{oldTagName}' not found.", "Tag Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
