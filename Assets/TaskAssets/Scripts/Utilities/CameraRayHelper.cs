using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public class CameraRayHelper
	{
		private static Camera _cachedCamera;
		private static Vector3 _center;

		public static Ray GetCenterCameraRay()
		{
			if (_cachedCamera == null)
			{
				_cachedCamera = Camera.main;
				_center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
			}

			return _cachedCamera.ScreenPointToRay(_center);
		}
	}
}
