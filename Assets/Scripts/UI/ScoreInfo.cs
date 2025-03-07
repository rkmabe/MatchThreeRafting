using MatchThreePrototype.Controllers;
using MatchThreePrototype.MatchReaction;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.UI
{

    public class ScoreInfo : MonoBehaviour
    {

        [SerializeField] private GameObject _display;

        [SerializeField] private TMPro.TextMeshProUGUI _infoText;
        [SerializeField] private Button _infoUpButton;
        [SerializeField] private Button _infoDownButton;
        private int _currInfoTextPageNum = 1;


        int _lastMoveNum = 0;

        string _moveText;

        //internal void UpdateMoveInfo(MoveRecord rec)
        //{
        //    if (rec.PlayerMoveNum != _lastMoveNum)
        //    {
        //        _moveText = string.Empty;
        //    }

        //    string text = "Move=" + rec.PlayerMoveNum + ", Type=" + rec.match.ItemType + ", Num=" + rec.match.NumMatches + ", Bonus=" + rec.match.IsBonusCatch;

        //    if (_moveText == string.Empty)
        //    {
        //        _moveText += Environment.NewLine + text;
        //    }
        //    else
        //    {
        //        _moveText = text;
        //    }

        //    _lastMoveNum = rec.PlayerMoveNum;

        //    if (_display.activeSelf)
        //    {
        //        if (_infoText.text == string.Empty)
        //        {
        //            _infoText.pageToDisplay = 1;
        //        }
        //        _infoText.text = _moveText;

        //        SetTextNavInteractable();
        //    }
        //}


        internal void UpdateMoveText(MoveRecord rec)
        {
            if (rec.PlayerMoveNum != _lastMoveNum)
            {
                _infoText.text = string.Empty;
                _currInfoTextPageNum = 1;
            }

            string text = "Move=" + rec.PlayerMoveNum + ", Type=" + rec.match.ItemType + ", Num=" + rec.match.NumMatches + ", Bonus=" + rec.match.IsBonusCatch;


            if (_infoText.text == string.Empty)
            {
                _infoText.pageToDisplay = 1;
                _infoText.text = text;
            }
            else
            {
                _infoText.text += Environment.NewLine + text;
            }

            SetTextNavInteractable();

            _lastMoveNum = rec.PlayerMoveNum;
        }

        private void SetTextNavInteractable()
        {
            if (_infoText.textInfo.pageCount <= 1)
            {
                _infoDownButton.interactable = false;
                _infoUpButton.interactable = false;
            }
            else
            {
                if (_currInfoTextPageNum < _infoText.textInfo.pageCount &&
                    _currInfoTextPageNum > 1)
                {
                    _infoDownButton.interactable = true;
                    _infoUpButton.interactable = true;
                }
                else if (_currInfoTextPageNum < _infoText.textInfo.pageCount)
                {
                    _infoDownButton.interactable = true;
                    _infoUpButton.interactable = false;
                }
                else if (_currInfoTextPageNum > 1)
                {
                    _infoDownButton.interactable = false;
                    _infoUpButton.interactable = true;
                }
            }
        }

        public void OnInfoUpClick()
        {
            if (_currInfoTextPageNum > 1)
            {
                _currInfoTextPageNum--;
                _infoText.pageToDisplay = _currInfoTextPageNum;
                SetTextNavInteractable();
            }

        }
        public void OnInfoDownClick() 
        {
            if (_infoText.textInfo.pageCount > _currInfoTextPageNum)
            {
                _currInfoTextPageNum++;
                _infoText.pageToDisplay = _currInfoTextPageNum;
                SetTextNavInteractable();
            }
        }

        private void OnChangeShowScoreInfoDelegate(bool isEnabled)
        {
            if (isEnabled) 
            {
                _display.transform.localScale = Statics.Vector3One();
            }
            else
            {
                _display.transform.localScale = Statics.Vector3Zero();
            }
        }


        private void OnDestroy()
        {

            _infoUpButton.onClick.RemoveAllListeners();
            _infoDownButton.onClick.RemoveAllListeners();

            SettingsController.OnChangeShowScoreInfoDelegate -= OnChangeShowScoreInfoDelegate;
        }

        private void Awake()
        {
            _infoUpButton.onClick.AddListener(OnInfoUpClick);
            _infoDownButton.onClick.AddListener(OnInfoDownClick);

            SettingsController.OnChangeShowScoreInfoDelegate += OnChangeShowScoreInfoDelegate;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
