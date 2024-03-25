using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePanel : Widget
{
    private List<AdvancedButtonBase> _buttons = new List<AdvancedButtonBase>();

    public void AddButton(AdvancedButtonBase button) 
    {
        _buttons.Add(button);
        button.transform.SetParent(transform);
        button.transform.localScale = Vector3.one;
        button.onClick.AddListener(DisableAllButton);
        button.OnConfirm += (_) => { Close(); };
    }

    private void DisableAllButton() 
    {
        foreach (var item in _buttons)
        {
            item.enabled = false;
        }
    }

    public void Open(int defaultSelectIndex, float duration = 0.2f) 
    {
        RenderOpacity = 0.0f;
        Fade(1f, duration, () => 
        {
            if (defaultSelectIndex < _buttons.Count)
            {
                _buttons[defaultSelectIndex].Select();
            }
            else
            {
                _buttons[0].Select();
            }
        });
    }


    public void Close(float duration = 0.2f) 
    {
        DialogueManager.SetCurrentSelectable(null);
        Fade(0f, duration, () =>
        {
            foreach (var button in _buttons)
            {
                Destroy(button.gameObject);
            }

            _buttons.Clear();
        });
    }
}
