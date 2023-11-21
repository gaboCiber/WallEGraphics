extends Control
var app_name = "WallE"
const UNTITLED = "Untitled"
var current_file = UNTITLED
var isSafeText : bool = true
var quit : bool = false
var newFile : bool = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
@warning_ignore("unused_parameter")
func _process(delta):
	pass

# Called when the node enters the scene tree for the first time.
func _ready():
	get_tree().set_auto_accept_quit(false)
	update_window_title()
	
	$menuContainer/MenuButtonFile.get_popup().connect("id_pressed", _on_menu_file_pressed)
	create_shortcut(0, "New File", $menuContainer/MenuButtonFile)
	create_shortcut(1, "Open File", $menuContainer/MenuButtonFile)
	create_shortcut(3, "Save", $menuContainer/MenuButtonFile)
	create_shortcut(4, "Save as", $menuContainer/MenuButtonFile)
	create_shortcut(6, "Quit", $menuContainer/MenuButtonFile)
	
	$menuContainer/MenuButtonEdit.get_popup().connect("id_pressed", _on_menu_edit_pressed)
	create_shortcut(0, "Undo", $menuContainer/MenuButtonEdit)
	create_shortcut(1, "Redo", $menuContainer/MenuButtonEdit)
	create_shortcut(3, "Copy", $menuContainer/MenuButtonEdit)
	create_shortcut(4, "Cut", $menuContainer/MenuButtonEdit)
	create_shortcut(5, "Paste", $menuContainer/MenuButtonEdit)
	create_shortcut(7, "Select all", $menuContainer/MenuButtonEdit)
	create_shortcut(8, "Copy all", $menuContainer/MenuButtonEdit)
	create_shortcut(9, "Remove all", $menuContainer/MenuButtonEdit)
	
func create_shortcut(id, name, menu):
	var event := InputEventAction.new()
	event.set_action(name)
	var shortcut := Shortcut.new()
	shortcut.set_events([event])
	menu.get_popup().set_item_shortcut(id, shortcut)
	
func _notification(what):
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
		if !isSafeText:
			quit = true
			$ConfirmationDialogQuit.show()
		else: 
			get_tree().quit()

func update_window_title():
	get_window().title = app_name + ' - ' + current_file

func _on_editor_text_changed():
	isSafeText = false
	if !get_window().title.ends_with('*'):
		get_window().title = get_window().title + '*'

func _on_menu_file_pressed(id):
	var item_name = $menuContainer/MenuButtonFile.get_popup().get_item_text(id)
		
	match item_name:
		'New File':
			newFile = true
			new_file()
		'Open File':
			$FileDialogOpen.popup()
		'Save':
			save_file()
		'Save as':
			$FileDialogSave.popup()
		'Quit':	
			if !isSafeText:
				quit = true
				$ConfirmationDialogQuit.show()
			else: 
				get_tree().quit()


func create_new_file():
	$editorContainer/editor.text = ''
	current_file = UNTITLED
	update_window_title()
		
func new_file(): 
	if !isSafeText:
		$ConfirmationDialogQuit.show()
	else:
		create_new_file()

func save_file():
	var path = current_file
	
	if path == UNTITLED:
		$FileDialogSave.show()
	else:
		_on_file_dialog_save_file_selected(path) 
	
	isSafeText = true
	
func _on_file_dialog_open_file_selected(path):
	var f = FileAccess.open(path, FileAccess.READ)
	$editorContainer/editor.text = f.get_as_text()
	f.close()
	current_file = path
	update_window_title()

func _on_file_dialog_save_file_selected(path):
	var f = FileAccess.open(path, FileAccess.WRITE)
	f.store_string($editorContainer/editor.text)
	f.close()
	current_file = path
	update_window_title()

func _on_file_dialog_save_confirmed():
	if quit:
		get_tree().quit()
	elif newFile:
		create_new_file()

func _on_file_dialog_save_canceled():
	isSafeText = false


func _on_confirmation_dialog_quit_confirmed():
	save_file()
	
	
func _on_confirmation_dialog_quit_canceled():
	if quit:
		get_tree().quit()
	elif newFile:
		create_new_file()
	
	
func _on_menu_edit_pressed(id):
	var item_name = $menuContainer/MenuButtonEdit.get_popup().get_item_text(id)
		
	match item_name:
		'Undo':
			$editorContainer/editor.undo()
		'Redo':
			$editorContainer/editor.redo()
		'Copy':
			$editorContainer/editor.copy()
		'Cut':
			$editorContainer/editor.cut()
		'Paste':
			$editorContainer/editor.paste()
		'Select all':
			$editorContainer/editor.select_all()
		'Copy all':
			$editorContainer/editor.select_all()
			$editorContainer/editor.copy()
			$editorContainer/editor.deselect()
		'Remove all':
			$editorContainer/editor.text = ''

