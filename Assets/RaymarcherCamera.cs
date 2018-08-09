using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarcherCamera : MonoBehaviour {

	int _FrustumCornersID;
	int _FrustumCorners2ID;

	Camera cam;

	void OnEnable () {
		_FrustumCornersID = Shader.PropertyToID("_FrustumCorners");
		_FrustumCorners2ID = Shader.PropertyToID("_FrustumCorners2");
		cam = GetComponent<Camera>();
	}

	static Vector3[] frustumCorners = new Vector3[4];

	void OnPreRender() {
		Shader.SetGlobalMatrix(_FrustumCornersID,  FrustumCornersMatrix(cam, cam.stereoActiveEye));
		if (cam.stereoTargetEye != StereoTargetEyeMask.None && Application.isPlaying && UnityEngine.XR.XRSettings.enabled && UnityEngine.XR.XRDevice.isPresent) 
			Shader.SetGlobalMatrix(_FrustumCorners2ID, FrustumCornersMatrix(cam, Camera.MonoOrStereoscopicEye.Right));
	}
	
	static Matrix4x4 FrustumCornersMatrix(Camera cam, Camera.MonoOrStereoscopicEye eye)
	{
		var camtr = cam.transform;
		cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, eye, frustumCorners);

		Matrix4x4 frustumMatrix = Matrix4x4.identity;
		frustumMatrix.SetRow(0, camtr.TransformVector(frustumCorners[0]));
		frustumMatrix.SetRow(1, camtr.TransformVector(frustumCorners[3]));
		frustumMatrix.SetRow(2, camtr.TransformVector(frustumCorners[1]));
		frustumMatrix.SetRow(3, camtr.TransformVector(frustumCorners[2]));
		return frustumMatrix;
	}
}
