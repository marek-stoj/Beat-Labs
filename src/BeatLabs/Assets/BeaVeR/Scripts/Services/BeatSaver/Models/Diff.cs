namespace BeaVeR.Services.BeatSaver
{
  public class Diff
  {
    public double Njs { get; set; }

    public double Offset { get; set; }

    public int Notes { get; set; }

    public int Bombs { get; set; }

    public int Obstacles { get; set; }

    public double Nps { get; set; }

    public double Length { get; set; }

    public string Characteristic { get; set; }

    public string Difficulty { get; set; }

    public int Events { get; set; }

    public bool Chroma { get; set; }

    public bool Me { get; set; }

    public bool Ne { get; set; }

    public bool Cinema { get; set; }

    public double Seconds { get; set; }

    public ParitySummary ParitySummary { get; set; }
  }
}
