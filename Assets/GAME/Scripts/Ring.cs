using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ring
{
    [Serializable]
    public class Player_Component
    {
        public Rigidbody2D _rigidbody;
        public SkeletonAnimation _skeletonAnimation;
    }
    [Serializable]
    public class Player_Touch
    {
        public BasePlayerController _girlController;
        public BasePlayerController _boyController;
    }
    [Serializable]
    public class Enemy_Component
    {
        public Rigidbody2D _rigidbody;
        public SkeletonAnimation _skeletonAnimation;
    }
    [Serializable]
    public class Player_Move
    {
        public float Speed_Move;
        public bool isMovingLeft = false;
        public bool isMovingRight = false;
    }
    [Serializable]
    public class Player_CheckGround
    {
        public bool isCheckGround;
    }
    [Serializable]
    public class Player_Spine
    {
        [SpineBone(dataField: "skeletonAnimation")]
        public string _boneName;
        public Bone _bone;
    }
    [Serializable]
    public class Player_Fire
    {
        [HideInInspector]public GameObject _ballFireGameObject;
        public GameObject _prefabs_Blue_BallFire;
        public GameObject _prefabs_Red_BallFire;
        public Transform _firePosition;
        public float _speedBall = 8f;
    }


    //UI
    [Serializable]
    public class UI_List
    {
        public List<GameObject> _listStar;
        public List<GameObject> _listButton;
    }
    [Serializable]
    public class Tutorials
    {
        public List<GameObject> _listTutorialUI_1;
        public List<GameObject> _listTutorialUI_2;
        public List<GameObject> _listTutorialUI_3;
        public EnumTutorials _enumTutorials;
        public string _countLevel;
        public bool ischeckState;
        public enum EnumTutorials
        {
            Step1,
            Step2,
            Step3,
            Step4,
            Step5,
            Step6,
            StepEnd
        }
    }
    
}
