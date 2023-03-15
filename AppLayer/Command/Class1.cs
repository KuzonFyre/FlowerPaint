using AppLayer.DrawingComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Command
{
    public class ResizeCommand : Command
    {
        private readonly Element _element;
        internal ResizeCommand(params object[] commandParameters)
        {
            if (commandParameters.Length > 0)
                _element = (Element)commandParameters[0];
        }
        public override bool Execute()
        {
            return false;
        }

        internal override void Redo()
        {
            throw new NotImplementedException();
        }

        internal override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
