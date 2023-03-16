using AppLayer.DrawingComponents;
using System.Collections.Generic;

namespace AppLayer.Command
{
    public class DeleteCommand : Command
    {
        private List<Element> _deletedElements;
        public override bool Execute()
        {
            _deletedElements = TargetDrawing.GetSelected();
            if (_deletedElements == null) return false;

            TargetDrawing.DeleteAllSelected();

            TargetDrawing.IsDirty = true;

            return true;
        }

        internal override void Undo()
        {
            _deletedElements.ForEach(e => { TargetDrawing.Add(e); });

            TargetDrawing.IsDirty = true;
        }

        internal override void Redo()
        {
            TargetDrawing.DeleteAllSelected();

            TargetDrawing.IsDirty = true;
        }
    }
}
