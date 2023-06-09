﻿using AppLayer.DrawingComponents;
using System.Collections.Generic;
using System.Drawing;

namespace AppLayer.Command
{
    public class DuplicateElementCommand : Command
    {
        private Point _location;
        private List<Element> _duplicatedElements;

        public DuplicateElementCommand(params object[] commandParameters)
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
                var Size = new Size(newElement.Size.Width, newElement.Size.Height);
                var extrinsicState = new ImageElementExtrinsicState()
                {
                    TreeType = newElement.ExtrinsicState.TreeType,
                    Location = _location,
                    Size = Size
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
            foreach (Element element in _duplicatedElements)
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
