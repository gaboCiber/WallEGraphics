global using System;
global using System.Collections.Generic;
global using WallE.FigureGraphics;
using System.Text;
using Godot;
using WallE;
using WallE.Graphics;

public partial class container : Control
{
	readonly string AppName = "WallE";
	readonly string UNTITLED = "UNTITLED";
	string current_file;
	bool isSafeText;
	bool quit;
	bool newFile;
	bool importFile;
	List<string> ErrorList, OutputList;
	List<List<FigureBase>> GraphicsWindowsHistory;
	IEnumerable<FigureBase> figures;

	public override void _Ready()
	{
		// Inicializar las variables
		current_file = UNTITLED;
		isSafeText = true;
		quit = newFile = importFile = false;
		ErrorList = new List<string>();
		OutputList = new List<string>();
		GraphicsWindowsHistory = new List<List<FigureBase>>();
		GetNode<AcceptDialog>("AcceptDialogError").Canceled += OnAcceptDialogErrorClose;
		GetNode<AcceptDialog>("AcceptDialogError").Confirmed += OnAcceptDialogErrorClose;
		
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
		CreateShortcut(menu, 2, "Add Import");
		CreateShortcut(menu, 4, "Save");
		CreateShortcut(menu, 5, "Save as");
		CreateShortcut(menu, 7, "Quit");
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
			case 2: // Add import
				importFile = true;
				GetNode<FileDialog>("FileDialogOpen").Popup();
				break;
			case 4: // Save
				SaveFile();
				break;
			case 5: // Save as
				newFile = false;
				GetNode<FileDialog>("FileDialogSave").Popup();
				break;
			case 7: // Quit
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
		if(importFile)
		{
			string import = $"import \"{path}\" ; \n";
			GetNode<CodeEdit>("editorContainer/editor").Text = import + GetNode<CodeEdit>("editorContainer/editor").Text;
			importFile = false;
		}
		else
		{
			var f = FileAccess.Open(path, FileAccess.ModeFlags.Read);
			GetNode<CodeEdit>("editorContainer/editor").Text = f.GetAsText();
			f.Close();
			current_file = path;
			UpdateWindowTitle();
		}
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

	private void OnTabBarTabSelected(int tab)
	{
		GetNode<RichTextLabel>("editorContainer/OutErrRect/RichTextLabel").Text = "";
		StringBuilder textBuilder = new StringBuilder();
		
		switch (tab)
		{
			case 0:
				OutputList.ForEach( i => textBuilder.Append(i + "\n"));
				LabelComplete(textBuilder.ToString(), 0);
				break;
			case 1:
				ErrorList.ForEach( i => textBuilder.Append(i + "\n"));
				LabelComplete(textBuilder.ToString(), 1);
				break;
			case 2:
				GetNode<ItemList>("editorContainer/OutErrRect/ItemList").Visible = true;
				GetNode<RichTextLabel>("editorContainer/OutErrRect/RichTextLabel").Visible = false;
				break;
			default:
				break;
		}

		void LabelComplete(string text, int color)
		{
			Theme theme = new Theme();
			GetNode<ItemList>("editorContainer/OutErrRect/ItemList").Visible = false;
			GetNode<RichTextLabel>("editorContainer/OutErrRect/RichTextLabel").Visible = true;
			GetNode<RichTextLabel>("editorContainer/OutErrRect/RichTextLabel").Text = text;
			theme.SetColor("default_color", "RichTextLabel", (color == 0) ? Colors.Green : Colors.Red);
			GetNode<RichTextLabel>("editorContainer/OutErrRect/RichTextLabel").Theme = theme;
		}
		
	}

	private void OnAcceptDialogErrorClose()
	{
		GetNode<AcceptDialog>("AcceptDialogError").Visible = false;
	}

    public void OnCompilarButtonPressed()
    {	
		ErrorList = new List<string>();
		OutputList = new List<string>();
		Window grafica = CreateGraficaWindow();

		if(current_file != UNTITLED)
		{
			OnFileDialogSaveFileSelected(current_file);
			isSafeText = true;
		}
		
		Depurate codeDepurate = new Depurate(GetNode<CodeEdit>("editorContainer/editor").Text);
		
		if(codeDepurate.IsThereAnyError)
		{
			ErrorList.Add(codeDepurate.Error);
			GetNode<AcceptDialog>("AcceptDialogError").Show();
			GetNode<TabBar>("ColorRect/TabBar").CurrentTab = 1;
			return;
		}

		StringBuilder totalCode = new StringBuilder(); 
		if(codeDepurate.ImportFiles.Count != 0)
		{
			foreach (var file in codeDepurate.ImportFiles)
			{
				var f = FileAccess.Open(file, FileAccess.ModeFlags.Read);

				if(f is null)
				{
					ErrorList.Add($"Runtime Error: El archivo '{file}' no existe");
					GetNode<AcceptDialog>("AcceptDialogError").Show();
					GetNode<TabBar>("ColorRect/TabBar").CurrentTab = 1;
					return;
				}

				totalCode.Append(f.GetAsText());
			}
		}
		
		totalCode.Append(codeDepurate.Output);
		
		CodeProcessor codeProcessor = new CodeProcessor(totalCode.ToString(), grafica.Size);
		
		if(codeProcessor.IsThereAnyErrors)
		{
			codeProcessor.GetErrors().ForEach( i => ErrorList.Add(i));
			GetNode<AcceptDialog>("AcceptDialogError").Show();
			GetNode<TabBar>("ColorRect/TabBar").CurrentTab = 1;
			return; 
		}
		
		OutputList = codeProcessor.GetOutput();
		//OutputList.Add(codeDepurate.Output);

		drawNode.AddFigures(codeProcessor.GetFigures());
		this.AddChild(grafica);
		grafica.Show();
		GraphicsWindowsHistory.Add(codeProcessor.GetFigures());
		GetNode<ItemList>("editorContainer/OutErrRect/ItemList").AddItem("Gráfico " + GraphicsWindowsHistory.Count);

		GetNode<TabBar>("ColorRect/TabBar").CurrentTab = (OutputList.Count == 0) ? 2 : 0;
    }

	private void SelectGrahicItem(int index)
	{
		Window grafica = CreateGraficaWindow();
		drawNode.AddFigures(GraphicsWindowsHistory[index]);
		this.AddChild(grafica);
		grafica.CloseRequested += CompilarCloseRequest;
		grafica.FocusExited += CompilarFocusExited;
		grafica.Show();

		void CompilarCloseRequest()
		{
			grafica.QueueFree();
		}

		void CompilarFocusExited()
		{
			grafica.QueueFree();
		}
	}

	private Window CreateGraficaWindow()
	{
		Window compilar = new Window();
		compilar.Title = "Graficación";
		compilar.InitialPosition = Window.WindowInitialPosition.CenterPrimaryScreen;
		compilar.Size = 4*(Vector2I)this.Size / 5;
		compilar.Unresizable = true;
		compilar.CloseRequested += CompilarCloseRequest;
		compilar.FocusExited += CompilarFocusExited;

		var scene = GD.Load<PackedScene>("res://Graphics/color_rect.tscn");
		compilar.AddChild(scene.Instantiate());

		return compilar;

		void CompilarCloseRequest()
		{
			compilar.QueueFree();
		}

		void CompilarFocusExited()
		{
			compilar.QueueFree();
		}
	}

}
