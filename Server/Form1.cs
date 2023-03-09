using Microsoft.Extensions.Logging;

namespace xemtest;

public partial class Form1 : Form, IUI
{  
    private readonly ILogger<Form1> logger;
    TextBox log;

    public Form1(ILogger<Form1> logger)
    {
        InitializeComponent();

        Button b = new Button();
        b.Location = new Point(10, 10);
        b.Size = new Size(150, 40);
        b.Text = "Run";
        b.Click += (s, a) =>
        {
            this.logger.LogInformation("SendToClient clicked");
            this._SendToClient?.Invoke(new MessageToClient() { Name = "Serv", ID = new Random(DateTime.Now.Millisecond).Next(1000) });
        };
        this.Controls.Add(b);

        log = new TextBox();
        log.Location = new Point(10, 70);
        log.Multiline = true;
        log.ReadOnly = true;
        log.Size = new Size(400, 200);
        this.Controls.Add(log);
        this.logger = logger;
    }

    private event Action<MessageToClient> _SendToClient = null;
    event Action<MessageToClient> IUI.SendToClient
    {
        add
        {
            this._SendToClient = value;
        }

        remove
        {
            this._SendToClient = null;
        }
    }

    private void Log(string text)
    {
        Action act = new Action(() => this.log.Text += $"{Environment.NewLine}{text}");

        if (this.InvokeRequired)
            this.Invoke(act);
        else
            act();
    }

    void IUI.Show()
    {
        this.Show();
    }

    void IDisposable.Dispose()
    {
        this.Close();
    }
}
