using System;
using System.Collections.Generic;
using BeatLabs.Utils;
using BeatLabsPlaygrounds._00_Common.BeatSaber.Models;
using Newtonsoft.Json;
using SonicBloom.Koreo;
using UnityEngine;

namespace BeatLabsPlaygrounds._08_SeatBaber
{
  public class BlockSpawner : MonoBehaviour
  {
    public StageController stageController;

    public GameObject blockTarget;

    [Tooltip("In Units per Second.")]
    public float blockSpeed = 1;

    public GameObject blockArchetype;

    public GameObject blocksParent;

    [EventID]
    public string koreoEventId;

    public float blockSpawnRadius;

    public float blockDespawnRadius;

    private void Start()
    {
      Koreography koreo = stageController.koreo;
      KoreographyTrackBase koreoTrack = koreo.GetTrackByID(koreoEventId);

      if (koreoTrack == null)
      {
        gameObject.SetActive(false);
        return;
      }

      List<KoreographyEvent> koreoEvents = koreoTrack.GetAllEvents();

      // NOTE: this could be optimized so that we don't create all the blocks up front
      // but rather "just in time"
      foreach (KoreographyEvent koreoEvent in koreoEvents)
      {
        GameObject blockGameObject = Instantiate(blockArchetype, blocksParent.transform);
        Block block = blockGameObject.GetComponentInChildrenSafe<Block>();

        string blockPayloadText = koreoEvent.GetTextValue();
        Note note = JsonConvert.DeserializeObject<Note>(blockPayloadText);

        if (note == null)
        {
          throw new Exception($"Couldn't deserialize payload into an object of type '{typeof(Note).FullName}'.");
        }

        int blockMaterialIndex;
        LayerMask blockLayer;

        switch (note._type)
        {
          case NoteType.LeftNote:
            blockLayer = LayerMask.NameToLayer("LeftNotes");
            blockMaterialIndex = 0;
            break;

          case NoteType.RightNote:
            blockLayer = LayerMask.NameToLayer("RightNotes");
            blockMaterialIndex = 1;
            break;

          default:
            throw new NotSupportedException($"Unknown or unsupported note type: '{note._type}'.");
        }

        Material blockMaterial = stageController.blockMaterials[blockMaterialIndex];

        float blockAngle;

        switch (note._cutDirection)
        {
          case CutDirection.Up:
            blockAngle = 180.0f;
            break;
          case CutDirection.Down:
            blockAngle = 0.0f;
            break;
          case CutDirection.Left:
            blockAngle = 90.0f;
            break;
          case CutDirection.Right:
            blockAngle = 270.0f;
            break;
          case CutDirection.UpLeft:
            blockAngle = 135.0f;
            break;
          case CutDirection.UpRight:
            blockAngle = 225.0f;
            break;
          case CutDirection.DownLeft:
            blockAngle = 45.0f;
            break;
          case CutDirection.DownRight:
            blockAngle = 315.0f;
            break;
          case CutDirection.Any:
            blockAngle = 0.0f;
            break;
          default:
            throw new NotSupportedException($"Unknown or unsupported cut direction: '{note._cutDirection}'.");
        }

        // NOTE: couldn't we just use time in samples instead of seconds?;
        float blockTargetTimeInSeconds = koreoEvent.StartSample / (float)koreo.SampleRate;

        block.Initialize(
          stageController,
          blockSpeed,
          blockSpawnRadius,
          blockDespawnRadius,
          blockTarget,
          blockTargetTimeInSeconds,
          blockLayer,
          blockMaterial,
          blockAngle);
      }
    }
  }
}
