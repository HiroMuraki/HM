using HM.Data;
using HM.Data.Servers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;

namespace LibraryTest;

[TestClass]
public class DataServerTest
{
    record class Vector
    {
        public int X { get; }
        public int Y { get; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    private static readonly string _localRoot = @"C:\Users\11717\Downloads\ServerTest\LocalData";
    private static readonly string _serverRoot = @"C:\Users\11717\Downloads\ServerTest\ServerData";
    private static readonly string _tempFile = @"testFile.txt";
    private static readonly string _serverFilePath = @"serverFile.txt";

    private readonly IDataServer _dataServer = new LocalDataServer(_serverRoot, FileIO.Instance);

    [TestMethod]
    public async Task Uploading_Test()
    {
        var vectors = new Vector[]
        {
            new Vector(Random.Shared.Next(0,10), Random.Shared.Next(0,10)),
            new Vector(Random.Shared.Next(0,10), Random.Shared.Next(0,10)),
            new Vector(Random.Shared.Next(0,10), Random.Shared.Next(0,10)),
            new Vector(Random.Shared.Next(0,10), Random.Shared.Next(0,10)),
        };

        _dataServer.ProgressChanged += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"[Uploading] {e.Progress * 100:F2}%"); // debug output
        };

        var data = await DataJsonSeralizer.Instance.SerializeAsync(vectors);
        await _dataServer.UploadAsync(data, _serverFilePath);

        System.Diagnostics.Debug.WriteLine($"[Write]"); // debug output
        foreach (var vector in vectors)
        {
            System.Diagnostics.Debug.WriteLine($"{vector}"); // debug output
        }
        System.Diagnostics.Debug.WriteLine($""); // debug output
    }

    [TestMethod]
    public async Task Downloading_Test()
    {
        _dataServer.ProgressChanged += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"[Downloading] {e.Progress * 100:F2}%"); // debug output
        };

        byte[] data = await _dataServer.FetchAsync(_serverFilePath);
        var result = await DataJsonSeralizer.Instance.DeserializeAsync<Vector[]>(data);

        System.Diagnostics.Debug.WriteLine($"[Read]"); // debug output
        foreach (var item in result)
        {
            System.Diagnostics.Debug.WriteLine($"{item}"); // debug output
        }
        System.Diagnostics.Debug.WriteLine($""); // debug output
    }
}