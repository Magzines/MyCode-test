using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckBox = System.Windows.Forms.CheckBox;
using Control = System.Windows.Forms.Control;
using Image = System.Drawing.Image;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace DevFrameWorkForm;
/// <summary>
/// "D.F.W.F" (Ver: 0.0.6P)
/// Console To Windows soon later done
/// It a buggy mess
/// Name Will be Change some time
/// </summary>
public class Ff
{
    private static int _mainId { get; set; } = 1;
    private static string _fullId = $"{_mainId}:Form.0";
    public Ff(int id=1)
    {
        if (ControlVar.IdStore.ContainsKey($"{id}:Form.0"))
        {
            var i = 0;
            while (true)
            {
                i++;
                if (ControlVar.IdStore.ContainsKey($"{i}:Form.0")) continue;
                _mainId = i;
                _fullId = $"{_mainId}:Form.0";
                break;
            }
        }
        else
        {
            _mainId = id;
            _fullId = $"{_mainId}:Form.0";
        }
    }
    /// <summary>
    /// Will Be Replace with Id System
    /// </summary>
    private abstract class ControlVar
    {
        public static Dictionary<string?, Control> IdStore = new Dictionary<string?, Control>();//Testing
    }
    public abstract class IdSystem
    {
        public static bool AddId(string? id,Control control)
        {
            if (id != null)
            {
                var idPart=id.Split(':','.');
                if (string.IsNullOrEmpty(id)||string.IsNullOrEmpty(idPart[3]))
                {
                    id = $"{_mainId}:{control.GetType().Name}.0";
                }
            }

            if (ControlVar.IdStore.ContainsKey(id)) return false;
            ControlVar.IdStore.Add(id,control);
            return true;
        }
        public static bool RemovedId(string? id)
        {
            if (!ControlVar.IdStore.ContainsKey(id)) return false;
            ControlVar.IdStore.Remove(id);
            return true;

        }
        public static Control? GetIdControl(string? id)
        {
            Control? controller=null;
            if (string.IsNullOrEmpty(id))
            {
                throw new NotImplementedException();
            }
            if (string.IsNullOrEmpty(id?.Split('.')[1]))
            {
                if (ControlVar.IdStore.Keys.Any(idPart => id == idPart))
                {
                    ControlVar.IdStore.TryGetValue(id,out controller);
                }
            }
            else
            {
                ControlVar.IdStore.TryGetValue(id, out controller);
            }
            return controller;
        }

        public static void GetID(string GetInfoID)
        {
            foreach (var infoIdList in ControlVar.IdStore.Keys.Select(IdListStore => string.Intern(IdListStore).StartsWith(GetInfoID)))
            {
                Console.WriteLine(infoIdList);
            }
        }
        //TODO: Nothing
        public static void AutoAdd(Control control, string? id =null)
        {
            var counter = 0;
            //Auto Add Id Here
            if (string.IsNullOrEmpty(id))
            {
                id = $"{_mainId}:{control.GetType().Name}.{counter}";
                while (ControlVar.IdStore.ContainsKey(id))
                {
                    id = $"{_mainId}:{control.GetType().Name}.{counter++}";
                }
            }
            if (!char.IsNumber(id[1]) || !char.IsNumber(id[3]))
            {
                while (ControlVar.IdStore.ContainsKey(id))
                {
                    id = $"{_mainId}:{control.GetType().Name}.{counter}";
                    while (ControlVar.IdStore.ContainsKey(id))
                    {
                        id = $"{_mainId}:{control.GetType().Name}.{counter++}";
                    }
                }
            }

            while (ControlVar.IdStore.ContainsKey(id))
            {
                id = $"{_mainId}:{control.GetType().Name}.{counter}";
                while (ControlVar.IdStore.ContainsKey(id))
                {
                    id = $"{_mainId}:{control.GetType().Name}.{counter++}";
                }
            }

            ControlVar.IdStore.Add(id,control);
        }

        public static void GetAllIdKey()
        {
            foreach (var keys in ControlVar.IdStore.Keys)
            {
                Console.WriteLine(keys);
            }
        }
    }
    public Form CreateForm(int width, int height, string title="")
    {
        var form = new Form()
        {
            Size = new Size(width, height),
            Text = title,
            AutoSize = true,
        };
        form.Closed += (object sender, EventArgs e) => Stop();
        IdSystem.AutoAdd(form);
        return form;
    }
    public Button CreateButton(int x, int y, string text, Action function = null!)
    {
        var action = function ?? (() => { });
        var button = new Button()
        {
            Location = new Point(x, y),
            Text = text,
            AutoSize = true,
            AutoEllipsis = true
        };
        button.Click += (sender, e) => action.Invoke();
        IdSystem.AutoAdd(button);
        IdSystem.GetIdControl(_fullId).Controls.Add(button);
        return button;
    }
    public Label CreateLabel(int x,int y,string text)
    {
        var label = new Label()
        {
            Location = new Point(x,y),
            Text = text,
            AutoSize = true,
            AutoEllipsis = true
        };
        IdSystem.AutoAdd(label);
        IdSystem.GetIdControl(_fullId)?.Controls.Add(label);
        return label;
    }
    public TextBox CreateTextBox(int x,int y,string text="",int width=80,int height=20)
    {
        var textBox = new TextBox()
        {
            Location = new Point(x,y),
            Size = new Size(width,height),
            Text = text,
            AutoSize = true
        };
        IdSystem.AutoAdd(textBox);
        IdSystem.GetIdControl(_fullId)?.Controls.Add(textBox);
        return textBox;
    }
    public TrackBar CreateTrackBar(int x,int y,int min=0,int max=100)
    {
        var trackBar = new TrackBar()
        {
            Location = new Point(x,y),
            Minimum = min,
            Maximum = max,
            AutoSize = true
        };
        IdSystem.AutoAdd(trackBar);
        IdSystem.GetIdControl(_fullId)?.Controls.Add(trackBar);
        return trackBar;
    }
    public ProgressBar CreateProgressBar(int x,int y,int min=0,int max=100)
    {
        var progressBar = new ProgressBar()
        {
            Location = new Point(x,y),
            Minimum = min,
            Maximum = max,
            AutoSize = true
        };
        IdSystem.AutoAdd(progressBar);
        IdSystem.GetIdControl(_fullId)?.Controls.Add(progressBar);
        return progressBar;
    }
    public CheckBox CreateCheckBox(int x,int y,string text="")
    {
        var checkBox = new CheckBox()
        {
            Location = new Point(x,y),
            Text = text,
            AutoCheck = true,
            AutoSize = true
        };
        IdSystem.AutoAdd(checkBox);
        IdSystem.GetIdControl(_fullId)?.Controls.Add(checkBox);
        return checkBox;
    }
    /// <summary>
    /// I Will add some Doc later
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="text"></param>
    /// <param name="link"></param>
    /// <returns></returns>
    public LinkLabel CreateLinkLabel(int x,int y,string text="",string? link="www.google.com")
    {
        var linkLabel = new LinkLabel()
        {
            Location = new Point(x,y),
            Text = text,
            AutoSize =true,
            AutoEllipsis = true
        };
        linkLabel.Click += (sender, e) =>
        {
            linkLabel.LinkVisited = true;
            Process.Start(!string.IsNullOrEmpty(link) ? link : "www.Google.com");
        };
        IdSystem.AutoAdd(linkLabel);
        IdSystem.GetIdControl(_fullId)?.Controls.Add(linkLabel);
        return linkLabel;
    }

    /// <summary>
    /// CreateMenuStrip It a List of Menu Stay on top of your windows
    /// Still Testing and Improve function
    /// </summary>
    /// <param name="y"></param>
    /// <param name="text"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public MenuStrip CreateMenuStrip(int x,int y,string? text)
    {
        var menuStrip = new MenuStrip()
        {
            Location = new Point(x,y),
            AutoSize = true,
            Text = text
        };
        IdSystem.AutoAdd(menuStrip);
        IdSystem.GetIdControl(_fullId)?.Controls.Add(menuStrip);
        return menuStrip;
    }
    /// <summary>
    /// Require Code/User Manually Add CreateMenuToolItem("PlaceHolder",Option).Add
    /// In Testing and Fix
    /// </summary>
    /// <param name="text"></param>
    /// <param name="function"></param>
    /// <returns>ToolStripMenuItem</returns>
    private ToolStripMenuItem CreateMenuToolItem(string text,Action? function=null)
    {
        var action = function ?? (() => { }); 
        var stripMenuItem = new ToolStripMenuItem(text)
        {
            AutoSize = true,
            AutoToolTip = true
        };
        stripMenuItem.Click += (sender, e) => {action.Invoke();};
        IdSystem.GetIdControl($"{_mainId}:MenuStrip");
        return stripMenuItem;
    }
    /// <summary>
    /// In Testing and Fix
    /// </summary>
    /// <param name="text"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    private ToolStripMenuItem CreateMenuItemList(string text,Action function)
    {
        var menuItem = new ToolStripMenuItem()
        {
            AutoSize = true,
            AutoToolTip = true
        };
        return menuItem;
    }
    /// <summary>
    /// Beta, In Testing And Fix
    /// Note: Too Buggy and cause Freeze to Form/UI
    /// </summary>
    /// <returns></returns>
    public async Task<OpenFileDialog?> PopupFileDialog(string text)
    {
        var fileDialog = new OpenFileDialog()
        {
            Title = text,
            AutoUpgradeEnabled = true,
            Multiselect = false,
            Filter = "Text Files|*.txt|All Files|*.*",
            InitialDirectory = @"C:\"
        };
        await Task.Run(() =>
        {
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Console.WriteLine("Hello");
            }
        });
        return fileDialog;
    }
    /// <summary>
    /// CODE LATER
    /// </summary>
    /// <returns></returns>
    public ToolStrip CreateToolStrip()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// CODE LATER
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ToolBar CreateToolBar()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Code in process
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="path"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public PictureBox CreateImage(int x,int y,string path,int width=0,int height=0)
    {
        if (!File.Exists(path))
        {
            return null!;
        }
        var pictureBox = new PictureBox()
        {
            Size = new Size(width,height),
            Image = Image.FromFile(path),
            Location = new Point(x,y),
            SizeMode = PictureBoxSizeMode.Zoom,
            AutoSize = (width==0||height==0)
        };
        return pictureBox;
    }
    //TODO: Update to Compilable with ID System and Fix ID
    
    private void Disposing(string? id=null)
    {
        if (!string.IsNullOrEmpty(id))
        {
            if (ControlVar.IdStore.ContainsKey(id))
            {
                IdSystem.GetIdControl(id)?.Dispose();
            }
        }
        else
        {
            foreach (var key in ControlVar.IdStore.Keys)
            {
                if (key[0] == _mainId)
                {
                    var control=IdSystem.GetIdControl(key);
                    using (control)
                    {
                        control.Dispose();
                    }
                }
            }
        }
    }
    public void Run(string? id=null)
    {
        if (!string.IsNullOrEmpty(id))
        {
            if (ControlVar.IdStore.ContainsKey(id))
            {
                Application.Run(IdSystem.GetIdControl(id) as Form);
            }
        }
        else
        {
            Application.Run(IdSystem.GetIdControl(_fullId) as Form);
        }
    }

    public void Run(Control? control)
    {
        Application.Run((Form)control);
    }
    public void Restart()
    {
        Disposing();
        Application.Restart();
    }

    public void Stop()
    {
        Disposing();
        Application.Exit();
    }
}