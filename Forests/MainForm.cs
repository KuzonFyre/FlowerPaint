using AppLayer.Command;
using AppLayer.DrawingComponents;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Forests
{
    // NOTE: There some design problems with this class

    public partial class MainForm : Form
    {
        private readonly int MOVEDISTANCE = 10;
        private readonly int SIZEINCREMENT = 10;
        private readonly Drawing _drawing;
        private bool _forceRedraw;
        private readonly Invoker _invoker;
        private string _currentTreeResource;
        private float _currentScale = 1;


        private enum PossibleModes
        {
            None,
            TreeDrawing,
            Selection
        };

        private PossibleModes _mode = PossibleModes.None;

        private Bitmap _imageBuffer;
        private Graphics _imageBufferGraphics;
        private Graphics _panelGraphics;

        public MainForm()
        {
            InitializeComponent();

            TreeFactory.Instance.ResourceNamePattern = @"Forests.Graphics.{0}.png";
            TreeFactory.Instance.ReferenceType = typeof(Program);

            _drawing = new Drawing();
            _invoker = new Invoker();
            CommandFactory.Instance.TargetDrawing = _drawing;
            CommandFactory.Instance.Invoker = _invoker;

            _invoker.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ComputeDrawingPanelSize();
            refreshTimer.Start();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            DisplayDrawing();
        }

        private void DisplayDrawing()
        {
            if (_imageBuffer == null)
            {
                _imageBuffer = new Bitmap(drawingPanel.Width, drawingPanel.Height);
                _imageBufferGraphics = Graphics.FromImage(_imageBuffer);
                _panelGraphics = drawingPanel.CreateGraphics();
            }


            if (_drawing.Draw(_imageBufferGraphics, drawingPanel.BackColor, _forceRedraw))
                _panelGraphics.DrawImageUnscaled(_imageBuffer, 0, 0);

            _forceRedraw = false;
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            CommandFactory.Instance.CreateAndDo("new");
        }

        private void ClearOtherSelectedTools(ToolStripButton ignoreItem)
        {
            foreach (var item in drawingToolStrip.Items)
            {
                var toolButton = item as ToolStripButton;
                if (toolButton != null && item != ignoreItem && toolButton.Checked)
                    toolButton.Checked = false;
            }
        }

        private void pointerButton_Click(object sender, EventArgs e)
        {
            var button = sender as ToolStripButton;
            ClearOtherSelectedTools(button);

            if (button != null && button.Checked)
            {
                _mode = PossibleModes.Selection;
                _currentTreeResource = string.Empty;
            }
            else
            {
                CommandFactory.Instance.CreateAndDo("deselect");
                _mode = PossibleModes.None;
            }
        }

        private void drawingPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) saveButton_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.Z) undoButton_Click(sender, e);
            else if (e.Control && e.Shift && e.KeyCode == Keys.Z) redoButton_Click(sender, e);
            else if (e.KeyCode == Keys.Delete) CommandFactory.Instance.CreateAndDo("delete");
            else if (e.Control && e.KeyCode == Keys.V) CommandFactory.Instance.CreateAndDo("duplicatetree", drawingPanel.PointToClient(Cursor.Position));
            else if (e.Control && e.KeyCode == Keys.Oemplus) increaseSize();
            else if (e.Control && e.KeyCode == Keys.OemMinus) decreaseSize();
            else if (e.Control && e.KeyCode == Keys.Left) moveLeft();
            else if (e.Control && e.KeyCode == Keys.Right) moveRight();
            else if (e.Control && e.KeyCode == Keys.Up) moveUp();
            else if (e.Control && e.KeyCode == Keys.Down) moveDown();
        }
        private void moveLeft()
        {
            CommandFactory.Instance.CreateAndDo("move", -1 * MOVEDISTANCE, 0);
        }
        private void moveRight()
        {
            CommandFactory.Instance.CreateAndDo("move", MOVEDISTANCE, 0);
        }
        private void moveUp() {
            CommandFactory.Instance.CreateAndDo("move", 0, -1 * MOVEDISTANCE);
        }
        private void moveDown()
        {
            CommandFactory.Instance.CreateAndDo("move", 0, MOVEDISTANCE);
        }
        private void decreaseSize()
        {
            CommandFactory.Instance.CreateAndDo("decreasetreesize", SIZEINCREMENT);
        }
        private void increaseSize()
        {
            CommandFactory.Instance.CreateAndDo("increasetreesize", SIZEINCREMENT);
        }
        private void treeButton_Click(object sender, EventArgs e)
        {
            var button = sender as ToolStripButton;
            ClearOtherSelectedTools(button);

            if (button != null && button.Checked)
                _currentTreeResource = button.Text;
            else
                _currentTreeResource = string.Empty;

            CommandFactory.Instance.CreateAndDo("deselect");
            _mode = (_currentTreeResource != string.Empty) ? PossibleModes.TreeDrawing : PossibleModes.None;
        }

        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_mode)
            {
                case PossibleModes.TreeDrawing:
                    if (!string.IsNullOrWhiteSpace(_currentTreeResource))
                        CommandFactory.Instance.CreateAndDo("addtree", _currentTreeResource, e.Location, _currentScale);
                    break;
                case PossibleModes.Selection:
                    CommandFactory.Instance.CreateAndDo("select", e.Location);
                    break;
            }
        }

        private void scale_Leave(object sender, EventArgs e)
        {
            _currentScale = ConvertToFloat(scale.Text, 0.01F, 99.0F, 1);
            scale.Text = _currentScale.ToString(CultureInfo.InvariantCulture);
        }

        private float ConvertToFloat(string text, float min, float max, float defaultValue)
        {
            var result = defaultValue;
            if (!string.IsNullOrWhiteSpace(text))
            {
                result = !float.TryParse(text, out result) ? defaultValue : Math.Max(min, Math.Min(max, result));
            }
            return result;
        }

        private void scale_TextChanged(object sender, EventArgs e)
        {
            _currentScale = ConvertToFloat(scale.Text, 0.01F, 99.0F, 1);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = "json",
                Multiselect = false,
                RestoreDirectory = true,
                Filter = @"JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CommandFactory.Instance.CreateAndDo("load", dialog.FileName);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "json",
                RestoreDirectory = true,
                Filter = @"JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CommandFactory.Instance.CreateAndDo("save", dialog.FileName);
            }
        }

        private void exportButton_Click(Object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "png",
                RestoreDirectory = true,
                Filter = @"PNG files (*.png)|*.png|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //_imageBuffer.Save(dialog.FileName);
                CommandFactory.Instance.CreateAndDo("export", new object[] { dialog.FileName, _imageBuffer });
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ComputeDrawingPanelSize();
        }

        private void ComputeDrawingPanelSize()
        {
            var width = Width - drawingToolStrip.Width;
            var height = Height - fileToolStrip.Height;

            drawingPanel.Size = new Size(width, height);
            drawingPanel.Location = new Point(drawingToolStrip.Width, fileToolStrip.Height);

            _imageBuffer = null;

            _forceRedraw = true;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            CommandFactory.Instance.CreateAndDo("remove");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _invoker?.Stop();
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            _invoker.Undo();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            _invoker.Redo();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = drawingPanel.BackColor;

            // Show the dialog box and check if the user clicks OK
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Set the background color of the form to the selected color
                drawingPanel.BackColor = colorDialog.Color;
            }
        }

        private void decreasesize_Click(object sender, EventArgs e)
        {
            decreaseSize();
        }

        private void increasesize_Click(object sender, EventArgs e)
        {
            increaseSize();
        }

        private void Up_Click(object sender, EventArgs e)
        {
            moveUp();
        }

        private void Down_Click(object sender, EventArgs e)
        {
            moveDown();
        }

        private void Left_Click(object sender, EventArgs e)
        {
            moveLeft();
        }

        private void Right_Click(object sender, EventArgs e)
        {
            moveRight();
        }
    }
}
