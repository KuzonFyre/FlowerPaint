using AppLayer.DrawingComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Command
{
    public class DuplicateCommand : Command
    {
        private readonly int _xOffset;
        private readonly int _yOffset;
        private Element _duplicatedElement;

        public DuplicateCommand(int xOffset, int yOffset)
        {
            _xOffset = xOffset;
            _yOffset = yOffset;
        }
        public override bool Execute()
        {
            Element selectedElement = SelectionManager.SelectedElement;
            if (selectedElement == null) return false;
            CommandFactory.Instance.CreateAndDo("new");
            throw new NotImplementedException();
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
