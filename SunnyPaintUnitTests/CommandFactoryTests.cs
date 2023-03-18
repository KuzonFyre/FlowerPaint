using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AppLayer.Command;
using AppLayer.DrawingComponents;
using System.Drawing;

namespace SunnyPaintUnitTests
{
    public class CommandFactoryTests
    {
        [Fact]
        public void Instance_Returns_Same_Instance()
        {
            var factory1 = CommandFactory.Instance;
            var factory2 = CommandFactory.Instance;
            Assert.Same(factory1, factory2);
        }

        [Fact]
        public void CreateAndDo_Does_Not_Create_Command_When_CommandType_Is_Null_Or_WhiteSpace()
        {
            var factory = CommandFactory.Instance;
            var drawing = new Drawing();
            var invoker = new Invoker();
            factory.TargetDrawing = drawing;
            factory.Invoker = invoker;

            factory.CreateAndDo(null);
            factory.CreateAndDo("");
            factory.CreateAndDo("   ");
            Assert.Equal(0, invoker.GetTodoQueueCount());
        }

        [Fact]
        public void CreateAndDo_Does_Not_Create_Command_When_TargetDrawing_Is_Null()
        {
            var factory = CommandFactory.Instance;
            var invoker = new Invoker();
            factory.TargetDrawing = null;
            factory.Invoker = invoker;

            factory.CreateAndDo("New");

            Assert.Equal(0, invoker.GetTodoQueueCount());
        }

        [Fact]
        public void CreateAndDo_Creates_NewCommand_When_CommandType_Is_New()
        {
            var factory = CommandFactory.Instance;
            var drawing = new Drawing();
            var invoker = new Invoker();
            factory.TargetDrawing = drawing;
            factory.Invoker = invoker;

            factory.CreateAndDo("New");
            Assert.Equal(1,invoker.GetTodoQueueCount());
        }

        [Fact]
        public void CreateAndDo_Creates_AddElementCommand_When_CommandType_Is_AddTree()
        {
            var factory = CommandFactory.Instance;
            var drawing = new Drawing();
            var invoker = new Invoker();
            factory.TargetDrawing = drawing;
            factory.Invoker = invoker;

            var assembly = typeof(CommandFactoryTests).Assembly;
            var resourceName = "AppLayer.Tests.Command.SampleTree.png";
            var center = new Point(50, 50);
            var scale = 1.0f;
            factory.CreateAndDo("addtree", resourceName, center, scale);
            Assert.Equal(1, invoker.GetTodoQueueCount());
        }

        [Fact]
        public void CreateAndDo_Creates_RemoveSelectedCommand_When_CommandType_Is_Remove()
        {
            var factory = CommandFactory.Instance;
            var drawing = new Drawing();
            var invoker = new Invoker();
            factory.TargetDrawing = drawing;
            factory.Invoker = invoker;

            factory.CreateAndDo("Remove");
            Assert.Equal(1, invoker.GetTodoQueueCount());
        }

        [Fact]
        public void CreateAndDo_Creates_SelectCommand_When_CommandType_Is_Select()
        {
            var factory = CommandFactory.Instance;
            var drawing = new Drawing();
            var invoker = new Invoker();
            factory.TargetDrawing = drawing;
            factory.Invoker = invoker;

            var location = new Point(50, 50);
            factory.CreateAndDo("Select", location);
            Assert.Equal(1, invoker.GetTodoQueueCount());
        }
    }
}
