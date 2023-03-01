//
//SpingManager.cs for unity-chan!
//
//Original Script is here:
//ricopin / SpingManager.cs
//Rocket Jump : http://rocketjump.skr.jp/unity3d/109/
//https://twitter.com/ricopin416
//
//Revised by N.Kobayashi 2014/06/24
//           Y.Ebata
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityChan
{
	public class SpringManager : MonoBehaviour
	{
		//Kobayashi
		// DynamicRatio is paramater for activated level of dynamic animation 
		public float dynamicRatio = 1.0f;

		//Ebata
		public float			stiffnessForce;
		public AnimationCurve	stiffnessCurve;
		public float			dragForce;
		public AnimationCurve	dragCurve;
		public SpringBone[] springBones;

		void Start ()
		{
			UpdateParameters ();
		}
	
		void Update ()
		{
#if UNITY_EDITOR
		//Kobayashi
		if(dynamicRatio >= 1.0f)
			dynamicRatio = 1.0f;
		else if(dynamicRatio <= 0.0f)
			dynamicRatio = 0.0f;
		//Ebata
		UpdateParameters();
#endif
		}
	
		private void LateUpdate ()
		{
			//Kobayashi
			if (dynamicRatio != 0.0f) {
				for (int i = 0; i < springBones.Length; i++) {
					if (dynamicRatio > springBones [i].threshold) {
						springBones [i].UpdateSpring ();
					}
				}
			}
		}
        [ContextMenu("一键清除动态骨骼")]
        public void ClearAllBone()
        {
            for (int i = 0; i < springBones.Length; i++)
            {
                DestroyImmediate(springBones[i]);
            }
            springBones = null;
        }
        public string[] boneKeywords;
        public SpringCollider[] cols;
        bool isContainBoneKeyName(string transName)
        {
            bool result = false;
            for (int i = 0; i < boneKeywords.Length; i++)
            {
                if(transName.Contains(boneKeywords[i]))
                {
                    result = true;
                }
            }
            return result;
        }
        [ContextMenu("一键设置动态骨骼")]
        public void SetupBone()
        {
            Transform[] allTrans = GetComponentsInChildren<Transform>();
            Transform curTrans = null;
            string curTransName = null;
            SpringBone springBone = null;

            List<SpringBone> bones = new List<SpringBone>();
            for (int i = 0; i < allTrans.Length; i++)
            {
                curTrans = allTrans[i];
                curTransName = curTrans.name;

 
                if(isContainBoneKeyName(curTransName) &&curTrans.childCount>0)
                {
                    springBone=curTrans.gameObject.AddComponent<SpringBone>();
                    springBone.child = curTrans.GetChild(0);
                    springBone.boneAxis = Vector3.up;
                    springBone.colliders = cols;
                    bones.Add(springBone);
                }
            }
            Debug.Log(bones.Count);
            springBones = bones.ToArray();
        }

		private void UpdateParameters ()
		{
			UpdateParameter ("stiffnessForce", stiffnessForce, stiffnessCurve);
			UpdateParameter ("dragForce", dragForce, dragCurve);
		}
	
		private void UpdateParameter (string fieldName, float baseValue, AnimationCurve curve)
		{
			var start = curve.keys [0].time;
			var end = curve.keys [curve.length - 1].time;
			//var step	= (end - start) / (springBones.Length - 1);
		
			var prop = springBones [0].GetType ().GetField (fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		
			for (int i = 0; i < springBones.Length; i++) {
				//Kobayashi
				if (!springBones [i].isUseEachBoneForceSettings) {
					var scale = curve.Evaluate (start + (end - start) * i / (springBones.Length - 1));
					prop.SetValue (springBones [i], baseValue * scale);
				}
			}
		}
	}
}