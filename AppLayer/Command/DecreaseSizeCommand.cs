using AppLayer.DrawingComponents;
using System.Collections.Generic;
using System.Drawing;

namespace AppLayer.Command
{
    public class DecreaseSizeCommand : Command
    {
        private List<Element> _resizedElements;
        private int _sizeDecrease;
        public DecreaseSizeCommand(params object[] commandParameters)
        {
            if (commandParameters.Length > 0)
                _sizeDecrease = (int)commandParameters[0];
        }
        public override bool Execute()
        {
            _resizedElements = new List<Element>();
            List<Element> elements = TargetDrawing.GetSelected();
            if (elements == null) return false;
            foreach (Element element in elements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Size = new Size(newElement.ExtrinsicState.Size.Width - _sizeDecrease, newElement.ExtrinsicState.Size.Height - _sizeDecrease);
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
                newElement.ExtrinsicState.Size = new Size(newElement.ExtrinsicState.Size.Width - _sizeDecrease, newElement.ExtrinsicState.Size.Height - _sizeDecrease);
                TargetDrawing.IsDirty = true;
            }
        }

        internal override void Undo()
        {
            foreach (Element element in _resizedElements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Size = new Size(newElement.ExtrinsicState.Size.Width + _sizeDecrease, newElement.ExtrinsicState.Size.Height + _sizeDecrease);
                TargetDrawing.IsDirty = true;
            }
        }
    }
}
