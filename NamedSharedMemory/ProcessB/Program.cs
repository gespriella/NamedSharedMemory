using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

class Program
{
    // Process B:
    static void Main(string[] args)
    {
        try
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("namedsharedmemory"))
            {

                Mutex mutex = Mutex.OpenExisting("testmapmutex");
                mutex.WaitOne();

                using (MemoryMappedViewStream stream = mmf.CreateViewStream(1, 0))
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(false);
                }
                mutex.ReleaseMutex();
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Memory-mapped file does not exist. Run Process A first.");
        }
    }
}
