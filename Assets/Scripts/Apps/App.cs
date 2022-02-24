using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class App : MonoBehaviour
{

    [Tooltip("Parent Object for everything that should dissapear when the App is minimised.")]
    public GameObject content;
    [Tooltip("Put all the rect transforms you want to resize with the App here.")]
    public RectTransform[] resizeWithApp;

    [HideInInspector] //set by AppManager
    public Canvas parentCanvas;

    [Tooltip("Minimum width this App can be resized too.")]
    public float minimumWidth = 200;
    [Tooltip("Minimum height this App can be resized too.")]
    public float minimumHeight = 200;
    [Tooltip("The size of the region in the Bottom-Right corner you need to click on to resize this App.")]
    public float resizeClickAreaSize = 20;

    private RectTransform _transform;
    private float _currentHightMaximised;

    private const float MINIMISED_HEIGHT = 45.0f;

    private bool _minimised;
    private enum SelectedState
    {
        NotSelected,
        Moving,
        Resizing
    }

    private SelectedState _currentState = SelectedState.NotSelected;

    private Vector2 _mouseOffsetOnClick;

    // Start is called before the first frame update
    protected void Start()
    {
        _transform = GetComponent<RectTransform>();
        _currentHightMaximised = _transform.rect.height;
        _minimised = false;

        content.GetComponent<RectTransform>().sizeDelta = _transform.sizeDelta;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_currentState == SelectedState.Moving)
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePos);
            _transform.position = parentCanvas.transform.TransformPoint(mousePos + _mouseOffsetOnClick);
        }
        else if (_currentState == SelectedState.Resizing)
        {
            #region Resize App From MousePos
            Vector2 topLeft = _transform.anchoredPosition;
            topLeft.x -= _transform.sizeDelta.x / 2;
            topLeft.y += _transform.sizeDelta.y / 2;

            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePos);

            Vector2 distanceFromTopLeft = mousePos - topLeft;

            Vector2 newSize;
            newSize.x = Mathf.Max(minimumWidth, distanceFromTopLeft.x);
            newSize.y = Mathf.Max(minimumHeight, -distanceFromTopLeft.y);

            if (_minimised)
            {
                newSize.y = MINIMISED_HEIGHT;
            }

            _transform.sizeDelta = newSize;

            Vector2 newPos = topLeft;
            newPos.x += _transform.sizeDelta.x / 2;
            newPos.y -= _transform.sizeDelta.y / 2;

            _transform.anchoredPosition = newPos;

            if (!_minimised)
            {
                //update currentHeightMaximised
                _currentHightMaximised = _transform.rect.height;
            }
            #endregion

            #region Resize Rects In "resizeWithApp"
            foreach (RectTransform rectTransform in resizeWithApp)
            {
                //Assumes topLeft-anchor
                Vector2 newRectSize = _transform.sizeDelta;
                newRectSize.x -= rectTransform.anchoredPosition.x * 2; //Treat x-compoent of position as offset from both edges
                newRectSize.y += rectTransform.anchoredPosition.y; //Treat y as offset from top

                rectTransform.sizeDelta = newRectSize;
            }
            #endregion
        }
    }

    public void ToggleMinimised()
    {

        Vector2 newRectSize;
        newRectSize.x = _transform.rect.width;
        Vector2 newPosition;
        newPosition.x = _transform.anchoredPosition.x;

        //if minimised
        if (_minimised) //maximise
        {
            newRectSize.y = _currentHightMaximised;
            _transform.sizeDelta = newRectSize;

            newPosition.y = _transform.anchoredPosition.y - ((_currentHightMaximised - MINIMISED_HEIGHT) / 2);
            _transform.anchoredPosition = newPosition;

            content.SetActive(true);
            _minimised = false;

            foreach (RectTransform rectTransform in resizeWithApp)
            {
                rectTransform.sizeDelta = newRectSize;
            }
        }
        else //minimise
        {
            newRectSize.y = MINIMISED_HEIGHT;
            _transform.sizeDelta = newRectSize;

            newPosition.y = _transform.anchoredPosition.y + ((_currentHightMaximised - MINIMISED_HEIGHT) / 2);
            _transform.anchoredPosition = newPosition;

            content.SetActive(false);
            _minimised = true;


            foreach (RectTransform rectTransform in resizeWithApp)
            {
                rectTransform.sizeDelta = newRectSize;
            }
        }
    }

    public void OnClick()
    {
        //Find mouse position in UI space
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePos);
        //Find mouseOffset so we can maintain it while draging
        _mouseOffsetOnClick = _transform.position - parentCanvas.transform.TransformPoint(mousePos);

        //if clicked on the buttom right corner
        if (-_mouseOffsetOnClick.x > ((_transform.sizeDelta.x / 2) - resizeClickAreaSize) && _mouseOffsetOnClick.y > ((_transform.sizeDelta.y / 2) - resizeClickAreaSize))
        {
            _currentState = SelectedState.Resizing;
        }
        else
        {
            _currentState = SelectedState.Moving;
        }
        

    }

    public void OnRelease()
    {
        _currentState = SelectedState.NotSelected;
    }
}
