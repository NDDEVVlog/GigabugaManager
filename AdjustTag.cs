using System;
using System.Drawing;
using System.Windows.Forms;
using GiGaBuGaManager.AdditionClass;
namespace GiGaBuGaManager
{
    public partial class AdjustTag : Form
    {
        private Button myButton;
        private FlowLayoutPanel flow;
        
        public AdjustTag(Button inputbutton,FlowLayoutPanel flow)
        {
            InitializeComponent();
            this.flow = flow;
            myButton = inputbutton;
            TagName.Text = myButton.Text;
        }

        private void AdjustTag_Load(object sender, EventArgs e)
        {
            // Optionally, initialize the color dialog with the current button color
            colorDialog1.Color = myButton.BackColor;
        }

        private void buttonTextColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                myButton.ForeColor = color;
                buttonTextColor.BackColor = color;
                TagName.ForeColor = color;
            }
        }

        private void buttonBackgroundColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                
                buttonBackgroundColor.BackColor = color;
            }
        }

        private void buttonButtonColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
               
                buttonButtonColor.BackColor = color;
            }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            myButton.BackColor = buttonBackgroundColor.BackColor;
            myButton.FlatAppearance.BorderColor = buttonButtonColor.BackColor;
            myButton.ForeColor = TagName.ForeColor;
            // Save the tags
            TagManager.Instance.SaveTags(flow);
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void DeleteTagButton_Click(object sender, EventArgs e)
        {
            // Confirm deletion
            var confirmResult = MessageBox.Show(
                "Are you sure to delete this tag?",
                "Confirm Delete",
                MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                TagManager.Instance.DeleteTag(myButton.Text, flow);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var oldName = myButton.Text;
            

            TagManager.Instance.RenameTag(oldName, TagName.Text, flow);
            TagManager.Instance.SaveTags(flow);
        }
    }
}
