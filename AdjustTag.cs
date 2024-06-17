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
            }
        }

        private void buttonBackgroundColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                myButton.BackColor = color;
                buttonBackgroundColor.BackColor = color;
            }
        }

        private void buttonButtonColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                myButton.FlatAppearance.BorderColor = color;
                buttonButtonColor.BackColor = color;
            }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            TagManager.Instance.SaveTags(flow);
        }
    }
}
