using AppLayer.DrawingComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppLayer.Command
{
    public class DuplicateTreeCommand : Command
    {
        private Point _location;
        private List<Element> _duplicatedElements;

        public DuplicateTreeCommand(params object[] commandParameters)
        {
            if (commandParameters.Length > 0)
                _location = (Point)commandParameters[0];
        }
        public override bool Execute()
        {
            _duplicatedElements = new List<Element>();
            List<Element> elements = TargetDrawing.GetSelected();
            if (elements == null) return false;
            foreach (Element element in elements)
            {
                var newElement = element as TreeWithAllState;
                var TreeSize = new Size(newElement.Size.Width,newElement.Size.Height);
                var extrinsicState = new TreeExtrinsicState()
                {
                    TreeType = newElement.ExtrinsicState.TreeType,
                    Location = _location,
                    Size = TreeSize
                };
                var _treeadded = TreeFactory.Instance.GetTree(extrinsicState);
                _duplicatedElements.Add(_treeadded);
                TargetDrawing.Add(_treeadded);
                TargetDrawing.IsDirty = true;
            }
            return true;
            
        }

        internal override void Redo()
        {
            foreach(Element element in _duplicatedElements)
            {
                TargetDrawing.Add(element);
            }
        }

        internal override void Undo()
        {
            foreach (Element element in _duplicatedElements)
            {
                TargetDrawing.DeleteElement(element);
            }
            
        }
    }
}
