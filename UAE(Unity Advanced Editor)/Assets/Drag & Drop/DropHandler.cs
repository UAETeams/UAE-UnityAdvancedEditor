using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class DropHandler : MonoBehaviour
{
    private DragDropController _dragDropController;

    public GameObject DragLoader;

    public Load load;


    [UsedImplicitly]
	private void Awake()
    {
        _dragDropController = gameObject.AddComponent<DragDropController>();
        if (!_dragDropController.Register())
        {
            Debug.Log("Failed to register drag & drop.");
        }

        _dragDropController.OnDropped += OnDrop;
        _dragDropController.OnDragEnter += OnDragEnter;
        _dragDropController.OnDragMove += OnDragMove;
        _dragDropController.OnDragExit += OnDragExit;
        _dragDropController.OnDroppedData += OnDropData;
	}
	
	[UsedImplicitly]
	private void OnDestroy()
	{
		if (_dragDropController == null)
		{
			return;
		}
		
		_dragDropController.OnDropped = null;
        _dragDropController.OnDragEnter = null;
        _dragDropController.OnDragMove = null;
        _dragDropController.OnDragExit = null;
        _dragDropController.OnDroppedData = null;
		_dragDropController = null;
	}

    private void OnDragEnter(string fileName, int x, int y)
    {
        Debug.Log("OnDragEnter - File name: " + fileName);
        Debug.Log("X: " + x + ", Y: " + y);
        NativeWinAlert.Show("works", "works", "OK");
    }

    public void OnDrop(string[] files, int x, int y)
    {
        NativeWinAlert.Show("OK", "OK", "OK");
        if (DragLoader.active == true)
        {
            DragLoader.SetActive(false);
            load.LoadDrag(files);
        }
    }

    private void OnDragMove(string fileName, int x, int y)
    {
        Debug.Log("OnDragMove - File name: " + fileName);
        Debug.Log("X: " + x + ", Y: " + y);
    }

    private void OnDragExit(string fileName, int x, int y)
    {
        Debug.Log("OnDragExit - File name: " + fileName);
        Debug.Log("X: " + x + ", Y: " + y);
    }

    private void OnDropData(string fileName, int x, int y, byte[] data)
    {
        Debug.Log("OnDropData - File name: " + fileName);
        Debug.Log("X: " + x + ", Y: " + y);

        if (Path.GetExtension(fileName).Equals(".txt", StringComparison.OrdinalIgnoreCase))
        {
            string textData = Encoding.UTF8.GetString(data);
            //FileContentsLabel.text = textData;
        }
    }
}
