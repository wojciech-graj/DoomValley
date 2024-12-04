using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Minigames;

namespace DoomValley;

public class DoomMinigame : IMinigame
{
    private const int Width = 320;
    private const int Height = 200;
    private readonly Color[] _colors;

    private readonly DgDrawFrame _drawFrame;
    private readonly DgExit _exit;
    private readonly DgGetKey _getKey;
    private readonly DgGetTicksMs _getTicksMs;

    private readonly Queue<KeyEvent> _keyEvents;

    private readonly Texture2D _texture;

    public DoomMinigame(string modDirectory)
    {
        _colors = new Color[Width * Height];
        _texture = new Texture2D(Game1.graphics.GraphicsDevice, Width, Height);
        _keyEvents = new Queue<KeyEvent>();

        unsafe
        {
            _drawFrame = DrawFrame;
            _getTicksMs = GetTicksMs;
            _getKey = GetKey;
            _exit = Exit;

            doomgeneric_Create(modDirectory, _drawFrame, _getTicksMs,
                _getKey, _exit);
        }
    }

    public bool tick(GameTime time)
    {
        doomgeneric_Tick();
        return false;
    }

    public bool overrideFreeMouseMovement()
    {
        return true;
    }

    public bool doMainGameUpdates()
    {
        return false;
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public void leftClickHeld(int x, int y)
    {
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public void releaseLeftClick(int x, int y)
    {
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public void receiveKeyPress(Keys k)
    {
        var key = DoomKey.ToDoomKey(k);
        if (key == 0) return;
        _keyEvents.Enqueue(new KeyEvent(key, true));
    }

    public void receiveKeyRelease(Keys k)
    {
        var key = DoomKey.ToDoomKey(k);
        if (key == 0) return;
        _keyEvents.Enqueue(new KeyEvent(key, false));
    }

    public void draw(SpriteBatch b)
    {
        b.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp);
        
        var screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width;
        var screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height;
        const float textureAspect = (float)Width / Height;
        int newWidth = screenWidth, newHeight = (int)(screenWidth / textureAspect);
        if (newHeight > screenHeight)
        {
            newHeight = screenHeight;
            newWidth = (int)(screenHeight * textureAspect);
        }
        var posX = (screenWidth - newWidth) / 2;
        var posY = (screenHeight - newHeight) / 2;
        var position = new Rectangle(posX, posY, newWidth, newHeight);
        
        b.Draw(_texture, position, Color.White);
        
        Game1.mouseCursorTransparency = 0f;
        
        b.End();
    }

    public void changeScreenSize()
    {
    }

    public void unload()
    {
    }

    public void receiveEventPoke(int data)
    {
    }

    public string minigameId()
    {
        return "Doom";
    }

    public bool forceQuit()
    {
        unload();
        return true;
    }

    [DllImport("doomgeneric_stardew")]
    private static extern void doomgeneric_Create(string? dir,
        DgDrawFrame pDgDrawFrame,
        DgGetTicksMs pDgGetTicksMs,
        DgGetKey pDgGetKey,
        DgExit pDgExit);

    [DllImport("doomgeneric_stardew")]
    private static extern void doomgeneric_Tick();

    private unsafe void DrawFrame(byte* buf)
    {
        for (var i = 0; i < Width * Height; i++)
        {
            var pixel = buf + i * 4;
            _colors[i].R = pixel[2];
            _colors[i].G = pixel[1];
            _colors[i].B = pixel[0];
        }

        _texture.SetData(_colors);
    }

    private void Exit()
    {
        unload();
        Game1.currentMinigame = null;
    }

    private static uint GetTicksMs()
    {
        return (uint)Environment.TickCount;
    }

    private int GetKey(ref int pressed, ref byte key)
    {
        if (_keyEvents.Count == 0) return 0;
        var e = _keyEvents.Dequeue();
        pressed = Convert.ToInt32(e.Pressed);
        key = e.Key;
        return 1;
    }

    private unsafe delegate void DgDrawFrame(byte* buf);

    private delegate uint DgGetTicksMs();

    private delegate int DgGetKey(ref int pressed, ref byte key);

    private delegate void DgExit();
}

internal class KeyEvent
{
    public readonly byte Key;
    public readonly bool Pressed;

    public KeyEvent(byte key, bool pressed)
    {
        Key = key;
        Pressed = pressed;
    }
}

internal static class DoomKey
{
    private const byte KeyRightarrow = 0xae;
    private const byte KeyLeftarrow = 0xac;
    private const byte KeyUparrow = 0xad;
    private const byte KeyDownarrow = 0xaf;
    private const byte KeyStrafeL = 0xa0;
    private const byte KeyStrafeR = 0xa1;
    private const byte KeyUse = 0xa2;
    private const byte KeyFire = 0xa3;
    private const byte KeyEscape = 27;
    private const byte KeyEnter = 13;
    private const byte KeyTab = 9;
    private const byte KeyF1 = 0x80 + 0x3b;
    private const byte KeyF2 = 0x80 + 0x3c;
    private const byte KeyF3 = 0x80 + 0x3d;
    private const byte KeyF4 = 0x80 + 0x3e;
    private const byte KeyF5 = 0x80 + 0x3f;
    private const byte KeyF6 = 0x80 + 0x40;
    private const byte KeyF7 = 0x80 + 0x41;
    private const byte KeyF8 = 0x80 + 0x42;
    private const byte KeyF9 = 0x80 + 0x43;
    private const byte KeyF10 = 0x80 + 0x44;
    private const byte KeyF11 = 0x80 + 0x57;
    private const byte KeyF12 = 0x80 + 0x58;
    private const byte KeyBackspace = 0x7f;
    private const byte KeyPause = 0xff;
    private const byte KeyEquals = 0x3d;
    private const byte KeyMinus = 0x2d;
    private const byte KeyRshift = 0x80 + 0x36;
    private const byte KeyRctrl = 0x80 + 0x1d;
    private const byte KeyRalt = 0x80 + 0x38;
    private const byte KeyLalt = KeyRalt;
    private const byte KeyCapslock = 0x80 + 0x3a;
    private const byte KeyNumlock = 0x80 + 0x45;
    private const byte KeyScrlck = 0x80 + 0x46;
    private const byte KeyPrtscr = 0x80 + 0x59;
    private const byte KeyHome = 0x80 + 0x47;
    private const byte KeyEnd = 0x80 + 0x4f;
    private const byte KeyPgup = 0x80 + 0x49;
    private const byte KeyPgdn = 0x80 + 0x51;
    private const byte KeyIns = 0x80 + 0x52;
    private const byte KeyDel = 0x80 + 0x53;
    private const byte Keyp0 = 0;
    private const byte Keyp1 = KeyEnd;
    private const byte Keyp2 = KeyDownarrow;
    private const byte Keyp3 = KeyPgdn;
    private const byte Keyp4 = KeyLeftarrow;
    private const byte Keyp5 = (byte)'5';
    private const byte Keyp6 = KeyRightarrow;
    private const byte Keyp7 = KeyHome;
    private const byte Keyp8 = KeyUparrow;
    private const byte Keyp9 = KeyPgup;
    private const byte KeypDivide = (byte)'/';
    private const byte KeypPlus = (byte)'+';
    private const byte KeypMinus = (byte)'-';
    private const byte KeypMultiply = (byte)'*';
    private const byte KeypPeriod = 0;
    private const byte KeypEquals = KeyEquals;
    private const byte KeypEnter = KeyEnter;

    public static byte ToDoomKey(Keys key)
    {
        return key switch
        {
            Keys.Back => KeyBackspace,
            Keys.Tab => KeyTab,
            Keys.Enter => KeyEnter,
            Keys.CapsLock => KeyCapslock,
            Keys.Escape => KeyEscape,
            Keys.PageUp => KeyPgup,
            Keys.PageDown => KeyPgdn,
            Keys.End => KeyEnd,
            Keys.Home => KeyHome,
            Keys.Left => KeyLeftarrow,
            Keys.Up => KeyUparrow,
            Keys.Right => KeyRightarrow,
            Keys.Down => KeyDownarrow,
            Keys.PrintScreen => KeyPrtscr,
            Keys.Insert => KeyIns,
            Keys.Delete => KeyDel,
            // KeyFire and KeyUse have to be manually remapped. default.cfg doesn't seem to work. I suspect this is
            // related to some change in doomgeneric.
            Keys.Space => KeyFire,
            Keys.E => KeyUse,
            Keys.D0 or Keys.D1 or Keys.D2 or Keys.D3 or Keys.D4 or Keys.D5 or Keys.D6 or Keys.D7 or Keys.D8
                or Keys.D9 => (byte)key,
            Keys.A or Keys.B or Keys.C or Keys.D or Keys.F or Keys.G or Keys.H or Keys.I
                or Keys.J or Keys.K or Keys.L or Keys.M or Keys.N or Keys.O or Keys.P or Keys.Q or Keys.R or Keys.S
                or Keys.T or Keys.U or Keys.V or Keys.W or Keys.X or Keys.Y
                or Keys.Z => (byte)char.ToLower((char)key),
            Keys.NumPad0 => Keyp0,
            Keys.NumPad1 => Keyp1,
            Keys.NumPad2 => Keyp2,
            Keys.NumPad3 => Keyp3,
            Keys.NumPad4 => Keyp4,
            Keys.NumPad5 => Keyp5,
            Keys.NumPad6 => Keyp6,
            Keys.NumPad7 => Keyp7,
            Keys.NumPad8 => Keyp8,
            Keys.NumPad9 => Keyp9,
            Keys.Multiply => KeypMultiply,
            Keys.Add => KeypPlus,
            Keys.Subtract => KeypMinus,
            Keys.Decimal => KeypPeriod,
            Keys.Divide => KeypDivide,
            Keys.F1 => KeyF1,
            Keys.F2 => KeyF2,
            Keys.F3 => KeyF3,
            Keys.F4 => KeyF4,
            Keys.F5 => KeyF5,
            Keys.F6 => KeyF6,
            Keys.F7 => KeyF7,
            Keys.F8 => KeyF8,
            Keys.F9 => KeyF9,
            Keys.F10 => KeyF10,
            Keys.F11 => KeyF11,
            Keys.F12 => KeyF12,
            Keys.NumLock => KeyNumlock,
            Keys.Scroll => KeyScrlck,
            Keys.RightShift => KeyRshift,
            Keys.RightControl => KeyRctrl,
            Keys.LeftAlt => KeyLalt,
            Keys.RightAlt => KeyRalt,
            Keys.Pause => KeyPause,
            _ => 0
        };
    }
}