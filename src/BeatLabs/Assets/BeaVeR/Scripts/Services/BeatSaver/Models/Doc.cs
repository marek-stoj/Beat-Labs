using System;
using System.Collections.Generic;

namespace BeaVeR.Services.BeatSaver
{
  public class Doc
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Uploader Uploader { get; set; }

    public Metadata Metadata { get; set; }

    public Stats Stats { get; set; }

    public DateTime Uploaded { get; set; }

    public bool Automapper { get; set; }

    public bool Ranked { get; set; }

    public bool Qualified { get; set; }

    public List<Version> Versions { get; set; } = new List<Version>();
  }
}
