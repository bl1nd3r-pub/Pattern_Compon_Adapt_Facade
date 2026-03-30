using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns_Compon_Adapt_Facade
{
    public class SyncFacade
    {
        private IFileSystem _sourceFS;
        private IFileSystem _targetFS;
        public SyncFacade(IFileSystem source, IFileSystem target)
        {
            _sourceFS = source;
            _targetFS = target;
        }
        public void SyncFolder(string sourcePath, string targetPath)
        {
            Console.WriteLine($"Синхронизация: {sourcePath} → {targetPath}");

            var items = _sourceFS.ListItems(sourcePath);
            foreach (var item in items)
            {
                string sourceItemPath = $"{sourcePath}/{item}";
                string targetItemPath = $"{targetPath}/{item}";


                try
                {
                    byte[] data = _sourceFS.ReadFile(sourceItemPath);

                    _targetFS.WriteFile(targetItemPath, data);

                    Console.WriteLine($"  + {item}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  X {item}: {ex.Message}");
                }
            }

            Console.WriteLine("Синхронизация завершена.");
        }
        public void Backup(string sourcePath, string backupPath)
        {
            Console.WriteLine($"[FACADE] Создание бэкапа: {sourcePath} → {backupPath}");

            var items = _sourceFS.ListItems(sourcePath);
            int successCount = 0;
            int failCount = 0;

            foreach (var itemName in items)
            {
                string sourceItemPath = $"{sourcePath}/{itemName}";
                string backupItemPath = $"{backupPath}/{itemName}";

                try
                {
                    byte[] data = _sourceFS.ReadFile(sourceItemPath);
                    long fileSize = 0;

                    if (_sourceFS is FileAdapter adapter)
                    {
                        var fileItem = adapter.FindItemByPath(sourceItemPath);
                        fileSize = fileItem.GetSize();
                    }

                    if (fileSize < 0)
                        throw new InvalidOperationException($"Некорректный размер файла: {fileSize}");

                    _targetFS.WriteFile(backupItemPath, data);
                    successCount++;
                    Console.WriteLine($"  ✓ Бэкап: {itemName} ({fileSize} байт)");  
                }
                catch (Exception ex)
                {
                    failCount++;
                    Console.WriteLine($"  ✗ Ошибка бэкапа {itemName}: {ex.Message}");
                }
            }

            Console.WriteLine($"[FACADE] Бэкап завершён: {successCount} успешно, {failCount} ошибок\n");
        }
    }
}
