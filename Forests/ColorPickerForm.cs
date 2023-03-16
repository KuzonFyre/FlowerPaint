using System;
using System.Windows.Forms;

namespace Forests
{
    public partial class ColorPickerForm : Form
    {
        private ColorDialog _colorDialog;
        public ColorPickerForm()
        {
            InitializeComponent();
        }

        private void button1_Load(object sender, EventArgs e)
        {
            _colorDialog = new ColorDialog();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = _colorDialog.Color;
            }
        }
    }
}
