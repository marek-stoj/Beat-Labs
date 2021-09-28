using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

namespace BeatLabs.XR
{
  [RequireComponent(typeof(XRDeviceSimulatorEx))]
  public class InitializeXRDeviceSimulator : MonoBehaviour
  {
    public XRDeviceSimulatorEx.TransformationMode initialMouseTransformationMode = XRDeviceSimulatorEx.TransformationMode.Rotate;
    public Vector3 HMDCenterEyePosition;
    public Vector3 leftControllerDevicePosition;
    public Vector3 rightControllerDevicePosition;

    private void Start()
    {
      XRDeviceSimulatorEx xrDeviceSimulator = GetComponent<XRDeviceSimulatorEx>();

      xrDeviceSimulator.mouseTransformationMode = initialMouseTransformationMode;

      xrDeviceSimulator.HmdCenterEyePosition = HMDCenterEyePosition;
      xrDeviceSimulator.LeftControllerDevicePosition = leftControllerDevicePosition;
      xrDeviceSimulator.RightControllerDevicePosition = rightControllerDevicePosition;
    }
  }
}
