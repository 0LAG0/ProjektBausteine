using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle _allToggle;

    [SerializeField]
    private List<Toggle> _toggleGroup;

    [SerializeField]
    private GameObject GroupContainer;

    void Start()
    {
        _toggleGroup = GroupContainer.GetComponentsInChildren<Toggle>().ToList();
        _allToggle.onValueChanged.AddListener(allSelectChanged);
        _toggleGroup.ForEach(e => e.onValueChanged.AddListener(multiSelectChanged));
    }

    public void allSelectChanged(bool input)
    {
        if (input)
        {
            if (_allToggle.isOn)
            {
                _toggleGroup.ForEach(e => e.isOn = false);
            }
            _allToggle.isOn = true;
        }

    }

    public void multiSelectChanged(bool input)
    {
        if (_toggleGroup.Any(e => e.isOn == true))
        {
            _allToggle.isOn = false;
        }

    }
}
