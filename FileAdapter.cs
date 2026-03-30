using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns_Compon_Adapt_Facade
{
    // Target
    public interface IFileSystem
    {
        List<string> ListItems(string path);
        byte[] ReadFile(string path);
        void WriteFile(string path, byte[] data);
        void DeleteItem(string path);
    }

    public class FileAdapter : IFileSystem
    {
        private readonly FileSystemItem _root;

        public FileAdapter(FileSystemItem root) { _root = root; }

        public List<string> ListItems(string path)
        {
            var item = FindItemByPath(path);
            var result = new List<string>();

            if (item is Folder folder)
            {
                for (int i = 0; i < folder.GetChildrenCount(); i++)
                {
                    var child = folder.GetChild(i);
                    result.Add(child.Name);
                }
            }

            return result;
        }

        public byte[] ReadFile(string path)
        {
            var item = FindItemByPath(path);

            if (item is File file)
            {
                // Симулируем чтение файла - возвращаем байты с размером
                return BitConverter.GetBytes(file.GetSize());
            }

            throw new FileNotFoundException($"Файл не найден: {path}");
        }

        public void WriteFile(string path, byte[] data)
        {
            // Симулируем запись данных в файл
            Console.WriteLine($"[ADAPTER] Запись файла: {path}");
        }

        public void DeleteItem(string path)
        {
            var item = FindItemByPath(path);
            var parentPath = path.Substring(0, path.LastIndexOf('/'));
            var parent = FindItemByPath(parentPath);

            if (parent is Folder folder)
            {
                folder.Remove(item);
            }
        }

        public FileSystemItem FindItemByPath(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return _root;

            // Проверяем, что путь начинается с корня
            if (parts[0] != _root.Name)
                throw new FileNotFoundException($"Неверный путь: {path}");

            FileSystemItem current = _root;

            // Начинаем поиск со ВТОРОЙ части пути (после корня)
            for (int i = 1; i < parts.Length; i++)
            {
                if (current is Folder folder)
                {
                    bool found = false;
                    for (int j = 0; j < folder.GetChildrenCount(); j++)
                    {
                        var child = folder.GetChild(j);
                        if (child.Name == parts[i])
                        {
                            current = child;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        throw new FileNotFoundException($"Элемент не найден: {path}");
                }
                else
                {
                    throw new InvalidOperationException($"Путь некорректен: {path}");
                }
            }

            return current;
        }
    }
}
