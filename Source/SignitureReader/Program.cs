
namespace SignitureReader
{
    using System;
    using TheBoxSoftware;
    using TheBoxSoftware.Reflection.Core;
    using TheBoxSoftware.Reflection.Core.COFF;
    using TheBoxSoftware.Reflection.Signitures;

    class Program
    {
        static void Main(string[] args)
        {
            // the argument is going to be a library to load
            PeCoffFile peCoffFile = new PeCoffFile(args[0], new FileSystem());

            peCoffFile.Initialise();
            BlobStream blobStream = peCoffFile.GetMetadataDirectory().Streams[Streams.BlobStream] as BlobStream;

            Console.WriteLine("Library Loaded");

            string input = Console.ReadLine();

            while(input != "q")
            {
                int offset = 0;
                string[] command = input.Split(' ');
                switch(command[0])
                {
                    case "bytes":
                        // print the bytes for the signiture at offset
                        offset = int.Parse(command[1]);
                        byte[] signiture = blobStream.GetSignitureContents(offset);

                        Console.WriteLine(FieldReader.ToHexString(signiture, 0, signiture.Length));
                        break;
                    case "tokens":
                        offset = int.Parse(command[1]);

                        try
                        {
                            Signitures type = (Signitures)Enum.Parse(typeof(Signitures), command[2]);
                            Signiture sig = blobStream.GetSigniture(1, type);
                            Console.WriteLine(sig.ToString());
                        }
                        catch(Exception)
                        {
                            Console.WriteLine($"Could not read signiture as a {command[2]}");
                        }
                        finally { }

                        break;
                    case "length":
                        Console.WriteLine(blobStream.GetLength(int.Parse(command[1])));
                        break;

                    case "first":
                        offset = int.Parse(command[1]);
                        byte[] a = blobStream.GetSignitureContents(offset);
                        CallingConventions conventions = (CallingConventions)(a[0] & 0x0F);

                        Console.WriteLine(conventions);
                        break;
                }
                Console.WriteLine();
                input = Console.ReadLine();
            }
        }
    }
}
