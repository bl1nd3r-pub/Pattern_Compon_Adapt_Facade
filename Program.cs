namespace Patterns_Compon_Adapt_Facade
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var root = new Folder("Root");
            var docs = new Folder("Documents");

            docs.Add(new File("doc.txt", 100));
            docs.Add(new File("report.pdf", 500));
            root.Add(docs);
            root.Add(new File("photo.jpg", 1200));

            root.PrintStructure();

            Console.WriteLine($"\nОбщий размер: {root.GetSize()} байт");

            DemoFacade();
        }

        static void DemoFacade()
        {
            Console.WriteLine();
            Console.WriteLine("=== FACADE (Фасад) ===\n");

            var root = new Folder("Root");
            root.Add(new File("doc.txt", 1024));
            root.Add(new File("image.jpg", 2048));

            IFileSystem localFS = new FileAdapter(root);
            IFileSystem backupFS = new FileAdapter(new Folder("Backup"));

            var facade = new SyncFacade(localFS, backupFS);

            Console.WriteLine("1. Синхронизация:");
            facade.SyncFolder("/Root", "/Backup");

            Console.WriteLine("2. Бэкап:");
            facade.Backup("/Root", "/Backup_Archive");
        }
    }
}
