using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Список исключений
/// Можно по ключу сцены указать другой префаб обложки сцены(AbsSceneUI)
/// </summary>
public class ExcepListKeyReferenceSceneAndPrefabSkinSceneLevelUI : AbsExceptionsListInKeyStoragePrefabSkinSceneLevelUI
{
  public override event Action OnInit;
    public override bool IsInit => _isInit;
    private bool _isInit = false;
    
    [SerializeField]
    //Список исключений. Нужен в случ. если опр. сцене, нужно задать опр. номер на сцене
    private List<AbsKeyData<KeyNameSceneInGetDataSO_KeyReferenceScene, AbsSkinSceneLevelUI>> _listExceptions;

    private void Awake()
    {
        List<KeyNameSceneInGetDataSO_KeyReferenceScene> _buffer = new List<KeyNameSceneInGetDataSO_KeyReferenceScene>();
        bool _isStart = false;

        StartLogic();

        void StartLogic()
        {
            _isStart = true;

            foreach (var VARIABLE in _listExceptions)
            {
                VARIABLE.Key.StartInit();
                
                if (VARIABLE.Key.IsInit == false)
                {
                    _buffer.Add(VARIABLE.Key);
                    VARIABLE.Key.OnInit += CheckInit;
                }
            }

            _isStart = false;

            CheckInit();
        }

        void CheckInit()
        {
            if (_isStart == false)
            {
                int targetCount = _buffer.Count;
                for (int i = 0; i < targetCount; i++)
                {
                    if (_buffer[i].IsInit == true)
                    {
                        _buffer[i].OnInit -= CheckInit;
                        _buffer.RemoveAt(i);
                        i--;
                        targetCount--;
                    }
                }

                if (_buffer.Count == 0)
                {
                    Completed();
                }
            }
        }
    }

    private void Completed()
    {
        _isInit = true;
        OnInit?.Invoke();
    }
    public override List<AbsKeyData<KeyNameScene, AbsSkinSceneLevelUI>> GetListExceptions()
    {
        List<AbsKeyData<KeyNameScene, AbsSkinSceneLevelUI>> list = new List<AbsKeyData<KeyNameScene, AbsSkinSceneLevelUI>>();
            
        foreach (var VARIABLE in _listExceptions)
        {
            list.Add(new AbsKeyData<KeyNameScene, AbsSkinSceneLevelUI>(VARIABLE.Key.GetKeySceneName(), VARIABLE.Data));
        }

        return list;
    }
}
