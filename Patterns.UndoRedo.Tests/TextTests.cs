using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Patterns.UndoRedo.Tests
{
    public class TextDocument
    {
        private readonly List<char> _text = new List<char>();

        public void RemoveLast()
        {
            _text.Remove(_text.Last());
        }

        public void Append(char c)
        {
            _text.Add(c);
        }

        public override string ToString()
        {
            return new string(_text.ToArray());
        }
    }

    public class AddCharCommand : IReversibleCommand<TextDocument>
    {
        private readonly char _char;

        public AddCharCommand(char c)
        {
            _char = c;
        }

        public void Execute(TextDocument doc)
        {
            doc.Append(_char);
        }

        public void Undo(TextDocument doc)
        {
            doc.RemoveLast();
        }
    }

    public static class ReversibleTextDocumentExtensions
    {
        public static void AddChar(this ReversibleDocument<TextDocument> doc, char c)
        {
            doc.Do(new AddCharCommand(c)).Apply();
        }
    }

    [TestFixture]
    public class TextTests
    {
        [Test]
        public void Should_Support_Undo()
        {
            var sut = new ReversibleDocument<TextDocument>(new TextDocument());

            sut.AddChar('a');
            sut.AddChar('z');
            sut.AddChar('E');
            sut.AddChar('f');
            sut.AddChar('é');

            sut.Undo();

            Assert.That(sut.Document.ToString(), Is.EqualTo("azEf"));
        }
    }
}
