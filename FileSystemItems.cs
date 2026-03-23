using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns_Compon_Adapt_Facade
{
    abstract class FileSystemItem
    {
        protected string name;
        public FileSystemItem(string name) { this.name = name; }

        public abstract string Name { get; set; }
        public abstract long GetSize();
        public abstract void Add(FileSystemItem item);
        public abstract void Remove(FileSystemItem item);
        public abstract FileSystemItem? GetChild(int index);
    }

    class File : FileSystemItem {
        private readonly long size;
        public File(string name, long size) : base(name) { this.size = size; }
        public override string Name { get; set; }
        public override long GetSize() => size;
        public override void Add(FileSystemItem item) { Console.WriteLine(":3 Это не папка, дурашка <3"); }
        public override void Remove(FileSystemItem item) { Console.WriteLine("^_^ Это не папка, дурашка <3"); }
        public override FileSystemItem? GetChild(int index) { Console.WriteLine(":D Это не папка, дурашка <3"); return null; }
    }

    class Folder : FileSystemItem { 
        List<FileSystemItem> children = [];
        public Folder(string name) : base(name) { }
        public override string Name { get; set; }
        public override long GetSize() {
            return 0; // сюда нахерачить рекурсивное сумирование размеров файлов
        }
        public override void Add(FileSystemItem item) { children.Add(item); }
        public override void Remove(FileSystemItem item) { children.Remove(item); }
        public override FileSystemItem? GetChild(int index)
        {
            return children[index];
        }
    }
}
