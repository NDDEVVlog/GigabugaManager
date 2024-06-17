using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GiGaBuGaManager.AdditionClass
{
    public class TagManager
    {
        private static TagManager instance = null;
        private static readonly object padlock = new object();
        private string tagsFilePath = "Tags.txt";

        private TagManager() { }

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

        public void SaveTags(FlowLayoutPanel tagFlow)
        {
            using (StreamWriter writer = new StreamWriter(tagsFilePath))
            {
                foreach (Button tagButton in tagFlow.Controls.OfType<Button>())
                {
                    string tagName = tagButton.Text;
                    Color buttonColor = tagButton.BackColor;
                    Color backgroundColor = tagButton.BackColor;
                    Color textColor = tagButton.ForeColor;

                    string tagInfo = $"{tagName},{buttonColor.ToArgb()},{backgroundColor.ToArgb()},{textColor.ToArgb()}";
                    writer.WriteLine(tagInfo);
                }
            }
        }

        public void LoadTags(FlowLayoutPanel tagFlow, EventHandler tagButtonClick)
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

                            // Create a new Tag instance
                            Tag tag = new Tag(tagName, buttonColor, backgroundColor, textColor);

                            // Create a new Button
                            Button tagButton = new Button();
                            tagButton.Text = tag.TagName;
                            tagButton.BackColor = tag.TagBackgroundColor;
                            tagButton.ForeColor = tag.TagTextColor;
                            tagButton.Click += tagButtonClick;

                            // Add the button to the TagFlow (FlowLayoutPanel)
                            tagFlow.Controls.Add(tagButton);
                        }
                    }
                }
            }
        }

        public void AddTag(string tagName, FlowLayoutPanel tagFlow)
        {
            // Check if the tag already exists in the TagFlow
            bool tagExists = tagFlow.Controls.OfType<Button>().Any(button => button.Text == tagName);

            // If the tag doesn't already exist, add it
            if (!tagExists)
            {
                // Create a new Tag instance
                Tag newTag = new Tag(tagName, Color.LightGray, Color.White, Color.Black);

                // Create a new Button
                Button newTagButton = new Button
                {
                    Text = newTag.GetTagName(),
                    BackColor = newTag.TagBackgroundColor,
                    ForeColor = newTag.TagTextColor,
                    FlatAppearance = { BorderColor = newTag.TagButtonColor }
                };

                // Add the button to the TagFlow (FlowLayoutPanel)
                tagFlow.Controls.Add(newTagButton);

                // Save the updated tags
                SaveTags(tagFlow);
            }
            else
            {
                // Optionally, provide feedback that the tag already exists
                MessageBox.Show("Tag already exists.", "Duplicate Tag", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
