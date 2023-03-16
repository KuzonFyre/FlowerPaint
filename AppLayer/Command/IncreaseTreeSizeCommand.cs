using AppLayer.DrawingComponents;
using System.Collections.Generic;
using System.Drawing;

namespace AppLayer.Command
{
    public class IncreaseTreeSizeCommand : Command
    {
        private List<Element> _resizedElements;
        public override bool Execute()
        {
            _resizedElements = new List<Element>();
            List<Element> elements = TargetDrawing.GetSelected();
            if (elements == null) return false;
            foreach (Element element in elements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Size = new Size(newElement.ExtrinsicState.Size.Width + 10, newElement.ExtrinsicState.Size.Height + 10);
                _resizedElements.Add(newElement);
                TargetDrawing.IsDirty = true;
            }
            return true;
        }

        internal override void Redo()
        {
            foreach (Element element in _resizedElements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Size = new Size(newElement.ExtrinsicState.Size.Width + 10, newElement.ExtrinsicState.Size.Height + 10);
                TargetDrawing.IsDirty = true;
            }
        }

        internal override void Undo()
        {
            foreach (Element element in _resizedElements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Size = new Size(newElement.ExtrinsicState.Size.Width - 10, newElement.ExtrinsicState.Size.Height - 10);
                TargetDrawing.IsDirty = true;
            }
        }
    }
}

