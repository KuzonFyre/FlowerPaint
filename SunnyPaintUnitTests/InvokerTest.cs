
using AppLayer.Command;
using AppLayer.DrawingComponents;
using Forests;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace SunnyPaintUnitTests
{
    public class InvokerTests
    {
        [Fact]
        public void EnqueueCommandForExecution_WithNullCommand_ShouldNotAddToQueue()
        {
            // Arrange
            var invoker = new Invoker();
            var initialCount = invoker.GetTodoQueueCount();

            // Act
            invoker.EnqueueCommandForExecution(null);

            // Assert
            Assert.Equal(initialCount, invoker.GetTodoQueueCount());
        }

        [Fact]
        public void EnqueueCommandForExecution_WithCommand_ShouldAddToQueue()
        {
            // Arrange
            var invoker = new Invoker();
            var initialCount = invoker.GetTodoQueueCount();
            Command command = new DeleteCommand();

            // Act
            invoker.EnqueueCommandForExecution(command);

            // Assert
            Assert.Equal(initialCount + 1, invoker.GetTodoQueueCount());
        }

        [Fact]
        public void Start_ShouldStartWorkerThread()
        {
            // Arrange
            var invoker = new Invoker();

            // Act
            invoker.Start();

            // Assert
            Assert.NotNull(invoker.GetWorkerThread());
            Assert.True(invoker.GetWorkerThread().IsAlive);
        }


        [Fact]
        public void Undo_WhenUndoStackNotEmpty_ShouldExecuteUndo()
        {
            // Arrange
            var invoker = new Invoker();
            Command command = new DeleteCommand();
            invoker.EnqueueCommandForExecution(command);
            command = new DeleteCommand();
            invoker.EnqueueCommandForExecution(command);
            var initialCount = invoker.GetUndoStackCount();

            // Act
            invoker.Undo();

            // Assert
            Assert.Equal(initialCount, invoker.GetUndoStackCount());
            Assert.Equal(0, invoker.GetRedoStackCount());
        }

        [Fact]
        public void Undo_WhenUndoStackEmpty_ShouldNotExecuteUndo()
        {
            // Arrange
            var invoker = new Invoker();
            var initialUndoCount = invoker.GetUndoStackCount();
            var initialRedoCount = invoker.GetRedoStackCount();

            // Act
            invoker.Undo();

            // Assert
            Assert.Equal(initialUndoCount, invoker.GetUndoStackCount());
            Assert.Equal(initialRedoCount, invoker.GetRedoStackCount());
        }

        [Fact]
        public void Redo_WhenRedoStackNotEmpty_ShouldExecuteRedo()
        {
            // Arrange
            var invoker = new Invoker();
            Command command = new DeleteCommand();
            invoker.EnqueueCommandForExecution(command);
            command = new DeleteCommand();
            invoker.EnqueueCommandForExecution(command);
            invoker.Undo();
            var initialCount = invoker.GetRedoStackCount();

            // Act
            invoker.Redo();

            // Assert
            Assert.Equal(initialCount, invoker.GetRedoStackCount());
            Assert.Equal(0, invoker.GetUndoStackCount());
        }

    }
}