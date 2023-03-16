using AppLayer.DrawingComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Command
{
    public class DecreaseTreeSizeCommand : Command
    {
        private List<Element> _resizedElements;
        private int _sizeDecrease;
        public DecreaseTreeSizeCommand(params object[] commandParameters)
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
