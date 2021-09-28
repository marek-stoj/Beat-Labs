namespace BeaVeR.Models.BeatSaber
{
  public enum EventType
  {
    ControlLightsInBackLasersGroup = 0,
    ControlLightsInRingLightsGroup = 1,
    ControlLightsInLeftRotatingLasersGroup = 2,
    ControlLightsInRightRotatingLasersGroup = 3,
    ControlLightsInCenterLightsGroup = 4,
    ControlBoostLightColors = 5,
    Unused6 = 6,
    Unused7 = 7,
    CreateOneRingSpin = 8,
    ControlZoomForRings = 9,
    BpmChange = 10,
    Unused11 = 11,
    ControlRotationSpeedForLightsInLeftRotatingLasersGroup = 12,
    ControlRotationSpeedForLightsInRightRotatingLasersGroup = 13,
    Early_360_90_Rotation = 14,
    Late_360_90_Rotation = 15,
  }
}
