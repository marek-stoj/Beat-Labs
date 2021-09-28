using System.IO;
using System.Threading.Tasks;
using Unity.SharpZipLib.Utils;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace BeaVeR
{
  public class BeatmapDownloader : MonoBehaviour
  {
    public static BeatmapDownloader Instance { get; private set; }

    private void Awake()
    {
      Instance = this;
    }

    public void GetBeatmapPaths(string beatmapHash, out string beatmapFilePath, out string beatmapDirPath)
    {
      string beatmapFileName = $"{beatmapHash}.zip";
      
      beatmapFilePath = Path.Combine(Application.persistentDataPath, beatmapFileName);

      string beatmapFileDirName = Path.GetDirectoryName(beatmapFilePath) ?? "";
      string beatmapDirName = Path.GetFileNameWithoutExtension(beatmapFilePath);
      
      beatmapDirPath = Path.Combine(beatmapFileDirName, beatmapDirName);

      Debug.Log($"beatmapFilePath: {beatmapFilePath}");
      Debug.Log($"beatmapDirPath: {beatmapFilePath}");
    }

    public async Task DownloadAndDecompressBeatmap(string beatmapUrl, string beatmapHash)
    {
      GetBeatmapPaths(beatmapHash, out string beatmapFilePath, out string beatmapDirPath);

      if (!Directory.Exists(beatmapDirPath))
      {
        if (!File.Exists(beatmapFilePath))
        {
          await DownloadBeatmap(beatmapUrl, beatmapFilePath);
        }
        else
        {
          Debug.Log("Beatmap file already exists. No need to download.");
        }

        DecompressBeatmap(beatmapFilePath, beatmapDirPath);
      }
      else
      {
        Debug.Log("Beatmap directory already exists. No need to download nor decompress.");
      }
    }

    private static async Task DownloadBeatmap(string beatmapUrl, string filePath)
    {
      Debug.Log("Downloading");

      UnityWebRequest.Result webRequestResult;
      int? dataLength = null;

      using (UnityWebRequest webRequest = UnityWebRequest.Get(beatmapUrl))
      {
        await webRequest.SendWebRequest();

        webRequestResult = webRequest.result;

        if (webRequestResult == UnityWebRequest.Result.Success)
        {
          byte[] fileData = webRequest.downloadHandler.data;

          dataLength = fileData.Length;

          File.WriteAllBytes(filePath, fileData);
        }
      }

      Debug.Log($"Downloaded. Result: {webRequestResult}. Data length: {dataLength}.");
    }

    private static void DecompressBeatmap(string beatmapFilePath, string beatmapDirPath)
    {
      Debug.Log("Decompressing");

      ZipUtility.UncompressFromZip(beatmapFilePath, null, beatmapDirPath);

      Debug.Log("Decompressed");
    }
  }
}
