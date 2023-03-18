using AppLayer.DrawingComponents;
using System.Collections.Generic;
using Xunit;

namespace AppLayer.Command.Tests
{
    public class DeleteCommandTest
    {
        [Fact]
        public void Execute_DeletesSelectedElements()
        {
            // Arrange
            var drawing = new Drawing();
            var command = new DeleteCommand();
            drawing.Add(new Element());
            drawing.Add(new Element());
            drawing.Select(drawing.GetElementAt(0));
            drawing.Select(drawing.GetElementAt(1));
            command.TargetDrawing = drawing;

            // Act
            var result = command.Execute();

            // Assert
            Assert.True(result);
            Assert.Empty(drawing.GetElements());
        }

        [Fact]
        public void Execute_ReturnsFalseIfNoElementsSelected()
        {
            // Arrange
            var drawing = new Drawing();
            var command = new DeleteCommand();
            command.TargetDrawing = drawing;

            // Act
            var result = command.Execute();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Undo_AddsDeletedElementsBackToDrawing()
        {
            // Arrange
            var drawing = new Drawing();
            var command = new DeleteCommand();
            var element1 = new Element();
            var element2 = new Element();
            drawing.Add(element1);
            drawing.Add(element2);
            drawing.Select(element1);
            drawing.Select(element2);
            command.TargetDrawing = drawing;

            // Act
            command.Execute();
            command.Undo();

            // Assert
            Assert.Equal(2, drawing.GetElements().Count);
            Assert.Contains(element1, drawing.GetElements());
            Assert.Contains(element2, drawing.GetElements());
        }

        [Fact]
        public void Redo_DeletesSelectedElementsAgain()
        {
            // Arrange
            var drawing = new Drawing();
            var command = new DeleteCommand();
            drawing.Add(new Element());
            drawing.Add(new Element());
            drawing.Select(drawing.GetElementAt(0));
            drawing.Select(drawing.GetElementAt(1));
            command.TargetDrawing = drawing;

            // Act
            command.Execute();

            // Assert
            Assert.Empty(drawing.GetElements());
        }
    }
}

