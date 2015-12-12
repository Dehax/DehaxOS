using DehaxOS.FileSystem;
using System;
using System.Text;
using System.Windows.Forms;

namespace DehaxOS
{
    public partial class MainForm : Form
    {
        private DehaxOS _dehaxOS;

        private bool _isCopied;
        private TreeNode _nodeCopyFrom;

        public MainForm()
        {
            InitializeComponent();

            _dehaxOS = new DehaxOS();
        }

        private void LoggedIn()
        {
            TreeNodeCollection tree = fileSystemTreeView.Nodes;
            tree.Clear();

            TreeNode rootNode = new TreeNode("/");
            rootNode.Name = "";
            rootNode.Nodes.Add("<пусто>");
            tree.Add(rootNode);
            //Directory rootDirectory = _dehaxOS.OpenDirectory("/");

            //for (int i = 0; i < rootDirectory.Count; i++)
            //{
            //    MetaFile metaFile
            //}
        }

        private string GetNodePath(TreeNode node)
        {
            string currentMetaFilePath = node.FullPath;
            currentMetaFilePath = (currentMetaFilePath != "/" ? currentMetaFilePath.Substring(1) : currentMetaFilePath);

            return currentMetaFilePath;
        }

        private void EditMetaFileProperties(TreeNode metaFileNode)
        {
            if (metaFileNode.Text == "<пусто>")
            {
                return;
            }

            string currentMetaFilePath = GetNodePath(metaFileNode);

            if (currentMetaFilePath == "/")
            {
                MessageBox.Show(this, "Невозможно изменить свойства корневого каталога!", "Невозможно изменить свойства", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentMetaFilePath.StartsWith(DehaxOS.SYSTEM_DIRECTORY_PATH))
            {
                MessageBox.Show(this, "Невозможно изменить свойства файлов системы!", "Невозможно изменить свойства", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PropertiesForm propertiesForm = new PropertiesForm();
            propertiesForm.Attributes = _dehaxOS.GetAttributes(currentMetaFilePath);
            propertiesForm.AccessRights = _dehaxOS.GetAccessRights(currentMetaFilePath);

            if (propertiesForm.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _dehaxOS.SetAttributes(currentMetaFilePath, propertiesForm.Attributes);
                    _dehaxOS.SetAccessRights(currentMetaFilePath, propertiesForm.AccessRights);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void OpenMetaFile(TreeNode metaFileNode)
        {
            if (metaFileNode.Text == "<пусто>")
            {
                return;
            }

            string metaFilePath = GetNodePath(metaFileNode);

            try
            {
                if (metaFileNode.GetNodeCount(true) > 0)
                {
                    Directory directory = _dehaxOS.OpenDirectory(metaFilePath);
                    TreeNodeCollection nodes = metaFileNode.Nodes;
                    nodes.Clear();

                    for (int i = 2; i < directory.Count; i++)
                    {
                        MetaFile metaFile = directory[i];

                        TreeNode node = new TreeNode(metaFile.FullName);
                        node.Name = metaFile.FullName;
                        if (metaFile is Directory)
                        {
                            node.Nodes.Add("<пусто>");
                        }
                        nodes.Add(node);
                    }

                    if (nodes.Count == 0)
                    {
                        nodes.Add("<пусто>");
                    }
                }
                else
                {
                    byte[] data = _dehaxOS.ReadFile(metaFilePath);
                    string content = Encoding.ASCII.GetString(data);

                    TextFileEditorForm editorForm = new TextFileEditorForm();
                    editorForm.FileContent = content;

                    if (editorForm.ShowDialog(this) == DialogResult.OK)
                    {
                        content = editorForm.FileContent;
                        data = Encoding.ASCII.GetBytes(content);

                        _dehaxOS.WriteFile(metaFilePath, data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void DeleteMetaFile(TreeNode metaFileNode)
        {
            if (metaFileNode.Text == "<пусто>")
            {
                return;
            }

            string currentMetaFilePath = GetNodePath(metaFileNode);

            if (currentMetaFilePath == "/")
            {
                MessageBox.Show(this, "Невозможно удалить корневой каталог!", "Невозможно удалить", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentMetaFilePath.StartsWith(DehaxOS.SYSTEM_DIRECTORY_PATH))
            {
                MessageBox.Show(this, "Невозможно удалить файлы системы!", "Невозможно удалить", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (metaFileNode.GetNodeCount(true) > 0)
                {
                    _dehaxOS.DeleteDirectory(currentMetaFilePath);
                }
                else
                {
                    _dehaxOS.DeleteFile(currentMetaFilePath);
                }

                TreeNode parentNode = metaFileNode.Parent;
                metaFileNode.Remove();

                if (parentNode.GetNodeCount(true) == 0)
                {
                    parentNode.Nodes.Add("<пусто>");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CreateDirectory(TreeNode node)
        {
            if (node.Text == "<пусто>")
            {
                return;
            }

            string currentMetaFilePath = GetNodePath(node);

            if (currentMetaFilePath.StartsWith(DehaxOS.SYSTEM_DIRECTORY_PATH))
            {
                MessageBox.Show(this, "Невозможно создать каталог в каталоге системы!", "Невозможно создать каталог", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EditNameForm editNameForm = new EditNameForm();
            
            if (editNameForm.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _dehaxOS.CreateDirectory((currentMetaFilePath != "/" ? currentMetaFilePath + "/" : currentMetaFilePath) + editNameForm.NewName);
                    if (node.FirstNode.Text == "<пусто>")
                    {
                        node.FirstNode.Remove();
                    }

                    TreeNode newNode = node.Nodes.Add(editNameForm.NewName);
                    newNode.Nodes.Add("<пусто>");
                }
                catch (Exception ex) when (ex is ArgumentException)
                {
                    MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void CreateFile(TreeNode node)
        {
            if (node.Text == "<пусто>")
            {
                return;
            }

            string currentMetaFilePath = GetNodePath(node);

            if (currentMetaFilePath.StartsWith(DehaxOS.SYSTEM_DIRECTORY_PATH))
            {
                MessageBox.Show(this, "Невозможно создать файл в каталоге системы!", "Невозможно создать файл", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EditNameForm editNameForm = new EditNameForm();

            if (editNameForm.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _dehaxOS.CreateFile((currentMetaFilePath != "/" ? currentMetaFilePath + "/" : currentMetaFilePath) + editNameForm.NewName);

                    if (node.FirstNode.Text == "<пусто>")
                    {
                        node.FirstNode.Remove();
                    }

                    TreeNode newNode = node.Nodes.Add(editNameForm.NewName);
                }
                catch (Exception ex) when (ex is ArgumentException)
                {
                    MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void RenameMetaFile(TreeNode node)
        {
            if (node.Text == "<пусто>")
            {
                return;
            }

            string currentMetaFilePath = GetNodePath(node);

            if (currentMetaFilePath == "/")
            {
                MessageBox.Show(this, "Невозможно переименовать корневой каталог!", "Невозможно переименовать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentMetaFilePath.StartsWith(DehaxOS.SYSTEM_DIRECTORY_PATH))
            {
                MessageBox.Show(this, "Невозможно переименовать файлы системы!", "Невозможно переименовать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EditNameForm editNameForm = new EditNameForm();

            if (editNameForm.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    if (node.GetNodeCount(true) > 0)
                    {
                        _dehaxOS.RenameDirectory(currentMetaFilePath, editNameForm.NewName);
                        node.Text = editNameForm.NewName;
                    }
                    else
                    {
                        _dehaxOS.RenameFile(currentMetaFilePath, editNameForm.NewName);
                        node.Text = editNameForm.NewName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void CopyMetaFile(TreeNode fromNode, TreeNode toNode)
        {
            if (fromNode.Text == "<пусто>")
            {
                return;
            }

            bool isFile = fromNode.GetNodeCount(true) <= 0;
            string currentFromMetaFilePath = GetNodePath(fromNode);

            if (currentFromMetaFilePath == "/")
            {
                MessageBox.Show(this, "Невозможно скопировать корневой каталог!", "Невозможно скопировать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentFromMetaFilePath.StartsWith(DehaxOS.SYSTEM_DIRECTORY_PATH))
            {
                MessageBox.Show(this, "Невозможно скопировать файлы системы!", "Невозможно скопировать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string currentToMetaFilePath = GetNodePath(toNode);

            if (currentToMetaFilePath == "/")
            {
                MessageBox.Show(this, "Невозможно копировать в корневой каталог!", "Невозможно копировать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentToMetaFilePath.StartsWith(DehaxOS.SYSTEM_DIRECTORY_PATH))
            {
                MessageBox.Show(this, "Невозможно копировать в файлы системы!", "Невозможно копировать", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (isFile)
                {
                    _dehaxOS.CopyFile(currentFromMetaFilePath, currentToMetaFilePath);

                    if (toNode.FirstNode.Text == "<пусто>")
                    {
                        toNode.FirstNode.Remove();
                    }

                    if (!toNode.Nodes.ContainsKey(fromNode.Text))
                    {
                        TreeNode newNode = toNode.Nodes.Add(fromNode.Text);
                    }
                }
                else
                {
                    _dehaxOS.CopyDirectory(currentFromMetaFilePath, currentToMetaFilePath);

                    if (toNode.FirstNode.Text == "<пусто>")
                    {
                        toNode.FirstNode.Remove();
                    }

                    if (!toNode.Nodes.ContainsKey(fromNode.Text))
                    {
                        TreeNode newNode = toNode.Nodes.Add(fromNode.Text);
                        newNode.Nodes.Add("<пусто>");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SwitchUser()
        {
            if (!_dehaxOS.Logout())
            {
                MessageBox.Show(this, "Невозможно сменить пользователя!", "Невозможно выйти", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_dehaxOS.IsLoggedIn)
            {
                LoginForm loginForm = new LoginForm();

                do
                {
                    if (loginForm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!_dehaxOS.Login(loginForm.UserName, loginForm.Password))
                        {
                            MessageBox.Show(this, "Имя пользователя или пароль не совпадают!", "Ошибка авторизации!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            systemStateStatusLabel.Text = "Вход в систему не выполнен!";
                        }
                        else
                        {
                            systemStateStatusLabel.Text = "Добро пожаловать, " + _dehaxOS.UserName + "!";

                            LoggedIn();
                        }
                    }
                    else
                    {
                        break;
                    }
                } while (!_dehaxOS.IsLoggedIn);

                if (!_dehaxOS.IsLoggedIn)
                {
                    Close();
                }
            }
        }

        private bool Quit()
        {
            bool result = _dehaxOS.Logout();

            if (result)
            {
                _dehaxOS.Dispose();
            }

            return result;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!_dehaxOS.IsLoggedIn)
            {
                LoginForm loginForm = new LoginForm();

                do
                {
                    if (loginForm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!_dehaxOS.Login(loginForm.UserName, loginForm.Password))
                        {
                            MessageBox.Show(this, "Имя пользователя или пароль не совпадают!", "Ошибка авторизации!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            systemStateStatusLabel.Text = "Вход в систему не выполнен!";
                        }
                        else
                        {
                            systemStateStatusLabel.Text = "Добро пожаловать, " + _dehaxOS.UserName + "!";

                            LoggedIn();
                        }
                    }
                    else
                    {
                        break;
                    }
                } while (!_dehaxOS.IsLoggedIn);

                if (!_dehaxOS.IsLoggedIn)
                {
                    Close();
                }
            }
        }

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FileSystemTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            OpenMetaFile(e.Node);
        }

        private void OpenMetaFileMenuItem_Click(object sender, EventArgs e)
        {
            if (fileSystemTreeView.SelectedNode.GetNodeCount(true) > 0)
            {
                fileSystemTreeView.SelectedNode.Expand();
            }
            else
            {
                OpenMetaFile(fileSystemTreeView.SelectedNode);
            }
        }

        private void propertiesMenuItem_Click(object sender, EventArgs e)
        {
            EditMetaFileProperties(fileSystemTreeView.SelectedNode);
        }

        private void deleteMetaFileMenuItem_Click(object sender, EventArgs e)
        {
            DeleteMetaFile(fileSystemTreeView.SelectedNode);
        }

        private void usersMenuItem_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm(_dehaxOS.UsersManager, _dehaxOS.GroupsManager);
            usersForm.ShowDialog(this);
        }

        private void changeUserMenuItem_Click(object sender, EventArgs e)
        {
            SwitchUser();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Quit())
            {
                e.Cancel = true;
            }
        }

        private void renameMenuItem_Click(object sender, EventArgs e)
        {
            RenameMetaFile(fileSystemTreeView.SelectedNode);
        }

        private void newDirectoryMenuItem_Click(object sender, EventArgs e)
        {
            if (fileSystemTreeView.SelectedNode.GetNodeCount(true) > 0)
            {
                CreateDirectory(fileSystemTreeView.SelectedNode);
            }
            else
            {
                CreateDirectory(fileSystemTreeView.SelectedNode.Parent);
            }
        }

        private void newTextFileMenuItem_Click(object sender, EventArgs e)
        {
            if (fileSystemTreeView.SelectedNode.GetNodeCount(true) > 0)
            {
                CreateFile(fileSystemTreeView.SelectedNode);
            }
            else
            {
                CreateFile(fileSystemTreeView.SelectedNode.Parent);
            }
        }

        private void formatMenuItem_Click(object sender, EventArgs e)
        {
            Program.FormatDisk = true;
            Close();
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            _isCopied = true;
            _nodeCopyFrom = fileSystemTreeView.SelectedNode;
            insertMenuItem.Enabled = true;
        }

        private void insertMenuItem_Click(object sender, EventArgs e)
        {
            if (fileSystemTreeView.SelectedNode.GetNodeCount(true) > 0)
            {
                CopyMetaFile(_nodeCopyFrom, fileSystemTreeView.SelectedNode);
            }
            else
            {
                CopyMetaFile(_nodeCopyFrom, fileSystemTreeView.SelectedNode.Parent);
            }

            _isCopied = false;
            _nodeCopyFrom = null;
            insertMenuItem.Enabled = false;
        }

        private void schedulerMenuItem_Click(object sender, EventArgs e)
        {
            SchedulerForm schedulerForm = new SchedulerForm(_dehaxOS);
            schedulerForm.ShowDialog(this);
        }
    }
}
