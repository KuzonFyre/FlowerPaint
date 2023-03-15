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
        private readonly string _filename;
        private readonly Bitmap _image;
        internal ExportCommand(params object[] commandParameters) {
            if (commandParameters.Length > 0)
            {
                _filename = commandParameters[0] as string;
                _image = commandParameters[1] as Bitmap;
            }
        }

        public override bool Execute()
        {
            // Save the bitmap to a PNG or JPG file
            _image.Save(_filename, ImageFormat.Png); // or ImageFormat.Jpeg
            return true;

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
