using AppLayer.DrawingComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer.Command
{
    public class MoveCommand : Command
    {
        private List<Element> _movedElements;
        private int _xChange;
        private int _yChange;
        public MoveCommand(params object[] commandParameters) {

            if (commandParameters.Length > 0)
            {
                _xChange = (int)commandParameters[0];
                _yChange = (int)commandParameters[1];
            }

        }

        public override bool Execute()
        {
            _movedElements = new List<Element>();
            List<Element> elements = TargetDrawing.GetSelected();
            if (elements == null) return false;
            foreach (Element element in elements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Location = new Point(newElement.ExtrinsicState.Location.X + _xChange, newElement.ExtrinsicState.Location.Y + _yChange);
                _movedElements.Add(newElement);
                TargetDrawing.IsDirty = true;
            }
            return true;
        }

        internal override void Redo()
        {
            foreach (Element element in _movedElements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Location = new Point(newElement.ExtrinsicState.Location.X - _xChange, newElement.ExtrinsicState.Location.Y - _yChange);
                TargetDrawing.IsDirty = true;
            }
        }

        internal override void Undo()
        {
            foreach (Element element in _movedElements)
            {
                var newElement = element as TreeWithAllState;
                newElement.ExtrinsicState.Location = new Point(newElement.ExtrinsicState.Location.X + _xChange, newElement.ExtrinsicState.Location.Y + _yChange);
                TargetDrawing.IsDirty = true;
            }
        }
    }
}
