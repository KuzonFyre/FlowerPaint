using AppLayer.DrawingComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Command
{
    public class ResizeTreeCommand : Command
    {
        private List<Element> _selectedElements;
        public ResizeTreeCommand(params object[] commandParameters)
        {
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

