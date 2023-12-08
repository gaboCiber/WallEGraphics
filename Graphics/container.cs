using Godot;
using System;
using WallE;
using System.Collections.Generic;

public partial class container : Control
{
	readonly string AppName = "WallE";
	readonly string UNTITLED = "UNTITLED";
	string current_file;
	bool isSafeText;
	bool quit;
	bool newFile;
	IEnumerable<IFigure> figures;

	public override void _Ready()
	{
		// Inicializar las variables
		current_file = UNTITLED;
		isSafeText = true;
		quit = newFile = false;

		// Deshabilitar el botón de cerrar
		GetTree().AutoAcceptQuit = false;

		// Actualizar el titulo de la app
		UpdateWindowTitle();

		// Ubicar el foco en el editor
		var editorNode = GetNode<CodeEdit>("editorContainer/editor");
		editorNode.GrabFocus();

		// Adiocionar el evento Pressed al boton Compilar
		//GetNode<Button>("compilarContainer/compilarButton").Pressed += OnCompilarButtonPressed;
		
		// Configurar el menu File
		var menu = GetNode<MenuButton>("menuContainer/MenuButtonFile");
		CreateShortcut(menu, 0, "New File");
		CreateShortcut(menu, 1, "Open File");
		CreateShortcut(menu, 3, "Save");
		CreateShortcut(menu, 4, "Save as");
		CreateShortcut(menu, 6, "Quit");
		menu.GetPopup().IdPressed += OnMenuFilePressed;

		// Configurar el menu Edit
		menu = GetNode<MenuButton>("menuContainer/MenuButtonEdit");
		CreateShortcut(menu, 0, "Undo");
		CreateShortcut(menu, 1, "Redo");
		CreateShortcut(menu, 3, "Copy");
		CreateShortcut(menu, 4, "Cut");
		CreateShortcut(menu, 5, "Paste");
		CreateShortcut(menu, 7, "Select all");
		CreateShortcut(menu, 8, "Copy all");
		CreateShortcut(menu, 9, "Remove all");
		menu.GetPopup().IdPressed += OnMenuSavePressed;
	}

	public void UpdateWindowTitle()
	{
		GetWindow().Title = AppName + " - " + current_file;
	}

	public void CreateShortcut(MenuButton menu, int id, string name)
	{
		var evento = new InputEventAction();
		evento.Action = name;
		var shortcut = new Shortcut();
		shortcut.Events = new Godot.Collections.Array{evento};
		menu.GetPopup().SetItemShortcut(id, shortcut);
	}

	public void QuitRequest()
	{
		if (!isSafeText)
		{
			quit = true;
			GetNode<ConfirmationDialog>("ConfirmationDialogQuit").Show();
		}
		else 
			GetTree().Quit();
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
			QuitRequest();	
	}

	public void OnEditorTextChanged()
	{
		isSafeText = false;
		if (!GetWindow().Title.EndsWith('*'))
			GetWindow().Title = GetWindow().Title + '*';
	}

	private void OnMenuSavePressed(long id)
	{
		var editorInstance = GetNode<CodeEdit>("editorContainer/editor");
		switch(id)
		{
			case 0: // Undo
				editorInstance.Undo();
				break;
			case 1: // Redo
				editorInstance.Redo();
				break;
			case 3: // Copy
				editorInstance.Copy();
				break;
			case 4: // Cut
				editorInstance.Cut();
				break;
			case 5: // Paste
				editorInstance.Paste();
				break;
			case 7: // Select all
				editorInstance.SelectAll();
				break;
			case 8: // Copy all
				editorInstance.SelectAll();
				editorInstance.Copy();
				editorInstance.Deselect();
				break;
			case 9: // Remove all
				editorInstance.Text = string.Empty;
				break;
			default:
				break;
		}
	}

	private void OnMenuFilePressed(long id)
	{
		switch(id)
		{
			case 0: // New File
				newFile = true;
				NewFile();
				break;
			case 1: // Open File
				GetNode<FileDialog>("FileDialogOpen").Popup();
				break;
			case 3: // Save
				SaveFile();
				break;
			case 4: // Save as
				GetNode<FileDialog>("FileDialogSave").Popup();
				break;
			case 6: // Quit
				QuitRequest();
				break;
			default:
				break;
		}
	}

	private void CreateNewFile()
	{
		GetNode<CodeEdit>("editorContainer/editor").Text = string.Empty;
		current_file = UNTITLED;
		UpdateWindowTitle();
		isSafeText = true;
	}
	
	private void NewFile()
	{
		if (!isSafeText)
			GetNode<ConfirmationDialog>("ConfirmationDialogQuit").Show();
		else
			CreateNewFile();
	}
	
	private void SaveFile()
	{
		var path = current_file;
	
		if (path == UNTITLED)
			GetNode<FileDialog>("FileDialogSave").Show();
		else
			OnFileDialogSaveFileSelected(path);
		
		isSafeText = true;
	}
	
	private void OnFileDialogOpenFileSelected(string path)
	{
		var f = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		GetNode<CodeEdit>("editorContainer/editor").Text = f.GetAsText();
		f.Close();
		current_file = path;
		UpdateWindowTitle();
	}

	private void OnFileDialogSaveFileSelected(string path)
	{
		var f = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		f.StoreString(GetNode<CodeEdit>("editorContainer/editor").Text);
		f.Close();
		current_file = path;
		UpdateWindowTitle();
	}

	private void OnFileDialogSaveCanceled()
	{
		isSafeText = false;

	}

	private void OnFileDialogSaveConfirmed()
	{
		if (quit)
			GetTree().Quit();
		else if (newFile)
			CreateNewFile();
	}

	private void OnConfirmationDialogQuitCanceled()
	{
		if (quit)
			GetTree().Quit();
		else if (newFile)
			CreateNewFile();
	}

	private void OnConfirmationDialogQuitConfirmed()
	{
		SaveFile();
	}

    public void OnCompilarButtonPressed()
    {	
		Window compilar = new Window();
		compilar.Title = "Graficación";
		compilar.InitialPosition = Window.WindowInitialPosition.CenterPrimaryScreen;
		compilar.Size = 4*(Vector2I)this.Size / 5;
		compilar.Unresizable = true;
		compilar.CloseRequested += CompilarCloseRequest;
		this.AddChild(compilar);

		var scene = GD.Load<PackedScene>("res://Graphics/draw_node.tscn");
		compilar.AddChild(scene.Instantiate());


		CodeProcessor codeProcessor = new CodeProcessor(GetNode<CodeEdit>("editorContainer/editor").Text, compilar.Size);
		drawNode.AddFigures(codeProcessor.GetFigures());

		compilar.Show();

		void CompilarCloseRequest()
		{
			compilar.QueueFree();
		}

    }

}
