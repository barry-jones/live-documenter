
namespace SignitureReader
{
    using System;
    using System.Text;
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
                byte[] contents;

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
                            /*
                            Signitures type = (Signitures)Enum.Parse(typeof(Signitures), command[2]);
                            Signiture sig = blobStream.GetSigniture(1, type);
                            Console.WriteLine(sig.ToString());
                            */
                            SignatureBuilder builder1 = new SignatureBuilder(blobStream);
                            Signature sig1 = builder1.Read(offset);

                            Console.WriteLine(sig1.ToString());
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

                    case "all":
                        StringBuilder output = new StringBuilder();
                        blobStream.GetRange(0, (uint)blobStream.GetLength());

                        for(int i = 0; i < blobStream.GetLength(); i++)
                        {
                            if(i == 0)
                            {
                                output.Append($"{i.ToString()}: ");
                            }
                            if(i != 0 && i % 16 == 0)
                            {
                                Console.WriteLine(output.ToString());
                                output.Clear();
                                output.Append($"{i.ToString()}: ");
                            }

                            output.Append($"{blobStream.GetByte(i).ToString("X2")} ");
                        }

                        break;

                    case "report":
                        offset = int.Parse(command[1]);

                        Console.WriteLine("Original code signiture retrieval: ");
                        contents = blobStream.GetSignitureContents(offset);
                        Console.WriteLine(FieldReader.ToHexString(contents, 0, contents.Length));

                        Console.WriteLine($"  byte at {offset}: 0x{blobStream.GetByte(offset).ToString("X2")}");

                        SignatureBuilder builder = new SignatureBuilder(blobStream);
                        Console.WriteLine($"  length: {builder.GetLength(offset)}");

                        contents = builder.GetSignitureBytes(offset);
                        Console.WriteLine("  contents:");
                        Console.WriteLine(FieldReader.ToHexString(contents, 0, contents.Length));

                        break;
                }
                Console.WriteLine();
                input = Console.ReadLine();
            }
        }
    }
}
