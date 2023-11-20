extends Control
var app_name = "WallE"
const UNTITLED = "Untitled"
var current_file = UNTITLED
var isSafeText : bool = true

# Called every frame. 'delta' is the elapsed time since the previous frame.
@warning_ignore("unused_parameter")
func _process(delta):
	pass

func _on_editor_text_changed():
	isSafeText = false
	pass
	
# Called when the node enters the scene tree for the first time.
func _ready():
	update_window_title()
	
	$menuContainer/MenuButtonFile.get_popup().connect("id_pressed", _on_menu_file_pressed)
	$menuContainer/MenuButtonEdit.get_popup().connect("id_pressed", _on_menu_edit_pressed)
	
	
func update_window_title():
	get_window().title = app_name + ' - ' + current_file

func _on_menu_file_pressed(id):
	var item_name = $menuContainer/MenuButtonFile.get_popup().get_item_text(id)
		
	match item_name:
		'New File':
			new_file()
		'Open File':
			$FileDialogOpen.popup()
		'Save':
			save_file()
		'Save as':
			$FileDialogSave.popup()
		'Quit':
			get_tree().quit()


func check_save():
	if !isSafeText:
		save_file()
		
func new_file(): 
	#check_save()
	$editorContainer/editor.text = ''
	current_file = UNTITLED
	update_window_title()	

func save_file():
	var path = current_file
	
	if path == UNTITLED:
		$FileDialogSave.popup()
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
		
