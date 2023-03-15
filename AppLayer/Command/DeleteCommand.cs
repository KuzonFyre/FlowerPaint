using AppLayer.DrawingComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Command
{
    public class DeleteCommand: Command
    {
        private Element _deletedElement;
        public override bool Execute()
        {
            _deletedElement = SelectionManager.SelectedElement; // retrieve selected element from SelectionManager
            if (_deletedElement == null) return false;

            TargetDrawing.DeleteElement(_deletedElement);
            SelectionManager.SelectedElement = null; // clear selected element in SelectionManager

            TargetDrawing.IsDirty = true;

            return true;
        }

        internal override void Undo()
        {
            TargetDrawing.Add(_deletedElement);
            SelectionManager.SelectedElement = _deletedElement; // restore selected element in SelectionManager

            TargetDrawing.IsDirty = true;
        }

        internal override void Redo()
        {
            TargetDrawing.DeleteElement(_deletedElement);
            SelectionManager.SelectedElement = null; // clear selected element in SelectionManager

            TargetDrawing.IsDirty = true;
        }
    }
}
