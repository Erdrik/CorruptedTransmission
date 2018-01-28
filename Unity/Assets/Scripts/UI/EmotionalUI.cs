using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionalUI : MonoBehaviour {

    public enum EmotionStatus
    {
        Happy,
        Sad,
        Cry
    }

    public Texture2D _happyFace;
    public Texture2D _sadFace;
    public Texture2D _cryingFace;

    public Image _fearEmotion;
    public Image _distrustEmotion;
    public Image _angerEmotion;

    private Sprite _happyFaceSprite;
    private Sprite _sadFaceSprite;
    private Sprite _cryFaceSprite;

    [Range(0,1)]
    public float _happyTheshold = 0.30f;
    [Range(0, 1)]
    public float _sadTheshold = 0.60f;

    private void Awake()
    {
        _happyFaceSprite = Sprite.Create(_happyFace, new Rect(0, 0, _happyFace.width, _happyFace.height), Vector2.one * 0.5f);
        _sadFaceSprite = Sprite.Create(_sadFace, new Rect(0, 0, _sadFace.width, _sadFace.height), Vector2.one * 0.5f);
        _cryFaceSprite = Sprite.Create(_cryingFace, new Rect(0, 0, _cryingFace.width, _cryingFace.height), Vector2.one * 0.5f);
    }

    public void Update()
    {
        if (RoomCameraManager._professor)
        {
            _fearEmotion.overrideSprite = EmotionalStatusToImage(EvalutateEmotion(RoomCameraManager._professor._fear));
            _distrustEmotion.overrideSprite = EmotionalStatusToImage(EvalutateEmotion(RoomCameraManager._professor._distrust));
            _angerEmotion.overrideSprite = EmotionalStatusToImage(EvalutateEmotion(RoomCameraManager._professor._anger));
        }
    }

    private EmotionStatus EvalutateEmotion(float emotionLevel)
    {
        if(emotionLevel <= _happyTheshold)
        {
            return EmotionStatus.Happy;
        }else if(emotionLevel <= _sadTheshold)
        {
            return EmotionStatus.Sad;
        }
        else
        {
            return EmotionStatus.Cry;
        }
    }

    private Sprite EmotionalStatusToImage(EmotionStatus status)
    {
        switch (status)
        {
            case EmotionStatus.Happy:
                return _happyFaceSprite;
            case EmotionStatus.Sad:
                return _sadFaceSprite;
            case EmotionStatus.Cry:
                return _cryFaceSprite;
            default:
                return _happyFaceSprite;
        }
    }

}
