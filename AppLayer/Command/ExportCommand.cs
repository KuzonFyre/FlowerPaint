using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppLayer.Command
{
    public class ExportCommand : Command
    {
        public override bool Execute()
        {

            // Get the control that contains the user-created content, e.g. a PictureBox
            Control contentControl = myPictureBox;

            // Create a new bitmap with the same size as the control
            Bitmap bmp = new Bitmap(contentControl.Width, contentControl.Height);

            // Draw the control onto the bitmap
            contentControl.DrawToBitmap(bmp, contentControl.ClientRectangle);

            // Save the bitmap to a PNG or JPG file
            bmp.Save("filename.png", ImageFormat.Png); // or ImageFormat.Jpeg

        }

        internal override void Redo()
        {
            Execute();
        }

        internal override void Undo()
        {
            //do nothing
        }
    }
}
