using System;
using System.Collections;
using System.Collections.Generic;
using Demos;
using Sokka06.ModuleController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleToggle : MonoBehaviour
{
    public Toggle Toggle;
    public TextMeshProUGUI Label;
    
    public AbstractCharacterModule Module { get; private set; }

    public void Bind(AbstractCharacterModule module)
    {
        Module = module;
        Label.SetText(Module.GetType().Name);
        Toggle.SetIsOnWithoutNotify(Module.Enabled);
        Toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void Unbind()
    {
        Module = null;
        Label.SetText("");
        Toggle.SetIsOnWithoutNotify(true);
        Toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool value)
    {
        Module.Enabled = value;
    }

    private void OnDestroy()
    {
        
    }
}
