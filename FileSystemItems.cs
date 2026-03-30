using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns_Compon_Adapt_Facade
{
    public abstract class FileSystemItem
    {
        protected string name;
        public FileSystemItem(string name) { this.name = name; }

        public abstract string Name { get; set; }
        public abstract long GetSize();
        public abstract void Add(FileSystemItem item);
        public abstract void Remove(FileSystemItem item);
        public abstract FileSystemItem? GetChild(int index);

        public abstract void PrintStructure(string indent = "", bool isLast = true);
    }

    class File : FileSystemItem {
        private readonly long size;
        public File(string name, long size) : base(name) { Name = name; this.size = size; }
        public override string Name { get; set; }
        public override long GetSize() => size;
        public override void Add(FileSystemItem item) { Console.WriteLine(":3 Это не папка, дурашка <3"); }
        public override void Remove(FileSystemItem item) { Console.WriteLine("^_^ Это не папка, дурашка <3"); }
        public override FileSystemItem? GetChild(int index) { Console.WriteLine(":D Это не папка, дурашка <3"); return null; }
        public override void PrintStructure(string indent = "", bool isLast = true)
        {
            var connector = isLast ? "└── " : "├── ";
            Console.WriteLine($"{indent}{connector}[FILE] {Name} ({size} байт)");
        }
    }

    class Folder : FileSystemItem { 
        List<FileSystemItem> children = [];
        public Folder(string name) : base(name) { Name = name; }
        public override string Name { get; set; }
        public override long GetSize() => children.Sum(child => child.GetSize());
        public override void Add(FileSystemItem item) { children.Add(item); }
        public override void Remove(FileSystemItem item) { children.Remove(item); }
        public override FileSystemItem? GetChild(int index)
        {
            return children[index];
        }
        public override void PrintStructure(string indent = "", bool isLast = true)
        {
            var connector = isLast ? "└── " : "├── ";
            Console.WriteLine($"{indent}{connector}[FOLDER] {Name}/ ({GetSize()} байт)");

            for (int i = 0; i < children.Count; i++)
            {
                var newIndent = indent + (isLast ? "    " : "│   ");
                bool childIsLast = (i == children.Count - 1);
                children[i].PrintStructure(newIndent, childIsLast);
            }
        }
        public int GetChildrenCount() => children.Count;
    }
}
