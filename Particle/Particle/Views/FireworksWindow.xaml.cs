using Particle.Enums;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Particle.Models
{
    /// <summary>
    /// FireworksWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FireworksWindow : Window
    {
        protected const int GWL_EXSTYLE = (-20);
        protected const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32")]
        protected static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        protected static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwLong);

        private readonly ParticleSystemManager _pm;
        private readonly Random _rand;
        private int _currentTick;
        private double _elapsed;
        private int _frameCount;
        private double _frameCountTime;
        private int _frameRate;
        private int _lastTick;
        private Point3D _spawnPoint;
        private double _totalElapsed;

        private DispatcherTimer _timer;
        private ParticleType _particleType;
        private TimeSpan _autoHiddenTime;
        private double _hiddenCountTime;

        public FireworksWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Tick += OnFrame;
            _timer.Interval = TimeSpan.FromSeconds(1.0 / 60.0);

            _spawnPoint = new Point3D(0.0, 0.0, 0.0);
            _pm = new ParticleSystemManager();

            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Red));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Orange));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Silver));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Gray));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Yellow));

            //WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Red));
            //WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Orange));
            //WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Yellow));
            //WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Green));
            //WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.SkyBlue));
            //WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Blue));
            //WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Magenta));

            _rand = new Random(GetHashCode());
            Cursor = Cursors.None;
        }

        public void TimerStart(ParticleType type, TimeSpan autoHiddenTime)
        {
            _particleType = type;
            _lastTick = Environment.TickCount;
            _autoHiddenTime = autoHiddenTime;
            _hiddenCountTime = 0d;
            _timer.Start();
        }

        public void TimerEnd()
        {
            _timer.Stop();
            Visibility = Visibility.Collapsed;
            //Close();
        }

        private void OnFrame(object sender, EventArgs e)
        {
            // Calculate frame time;
            _currentTick = Environment.TickCount;
            _elapsed = (_currentTick - _lastTick) / 1000.0;
            _totalElapsed += _elapsed;
            _lastTick = _currentTick;

            _frameCount++;
            _frameCountTime += _elapsed;

            if(_autoHiddenTime != null && _autoHiddenTime.TotalSeconds > 0)
            {
                _hiddenCountTime += _elapsed;
                if (_hiddenCountTime >= _autoHiddenTime.TotalSeconds)
                {
                    TimerEnd();
                    return;
                }
            }

            if (_frameCountTime >= 1.0)
            {
                _frameCountTime -= 1.0;
                _frameRate = _frameCount;
                _frameCount = 0;
                //FrameRateLabel.Content = "FPS: " + _frameRate + "  Particles: " + _pm.ActiveParticleCount + " E:" + _elapsed + " TE:" +_totalElapsed;
            }

            _pm.Update((float)_elapsed);

            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Red, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Orange, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Silver, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Gray, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Yellow, _rand.NextDouble(), 5 * _rand.NextDouble());

            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Red, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Orange, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Silver, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Gray, _rand.NextDouble(), 5 * _rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Yellow, _rand.NextDouble(), 5 * _rand.NextDouble());

            //_pm.SpawnParticle(_spawnPoint, 10.0, Colors.Red, _rand.NextDouble(), 5 * _rand.NextDouble());
            //_pm.SpawnParticle(_spawnPoint, 10.0, Colors.Orange, _rand.NextDouble(), 5 * _rand.NextDouble());
            //_pm.SpawnParticle(_spawnPoint, 10.0, Colors.Yellow, _rand.NextDouble(), 5 * _rand.NextDouble());
            //_pm.SpawnParticle(_spawnPoint, 10.0, Colors.Green, _rand.NextDouble(), 5 * _rand.NextDouble());
            //_pm.SpawnParticle(_spawnPoint, 10.0, Colors.SkyBlue, _rand.NextDouble(), 5 * _rand.NextDouble());
            //_pm.SpawnParticle(_spawnPoint, 10.0, Colors.Blue, _rand.NextDouble(), 5 * _rand.NextDouble());
            //_pm.SpawnParticle(_spawnPoint, 10.0, Colors.Magenta, _rand.NextDouble(), 5 * _rand.NextDouble());

            SetPointfromType(_particleType);
        }

        private void SetPointfromType(ParticleType type)
        {
            var c = Math.Cos(_totalElapsed);
            var s = Math.Sin(_totalElapsed);
            var t = Math.Tan(_totalElapsed);

            switch (type)
            {
                case ParticleType.Rocket:
                    _spawnPoint = new Point3D(0, t * 32, t * 32);
                    break;
                case ParticleType.ShootingStar:
                    _spawnPoint = new Point3D(t * 32, t * 32, 0);
                    break;
                case ParticleType.Circle:
                    _spawnPoint = new Point3D(s * 32, c * 32, 0);
                    break;
                case ParticleType.Infinity:
                    _spawnPoint = new Point3D(t * c * 60, s * c * 32, 0);
                    break;
                case ParticleType.Sinkansen:
                    _spawnPoint = new Point3D(t * 60, 30, 0);
                    break;
                case ParticleType.Blast:
                    _spawnPoint = new Point3D(-10 + _rand.NextDouble() * 32, _rand.NextDouble() * 32, 0.0);
                    break;
                case ParticleType.LefttoRightCurve:
                    _spawnPoint = new Point3D(t * 32, s * 32, 0);
                    break;
                case ParticleType.ButtomtoTopCurve:
                    _spawnPoint = new Point3D(s * 32, t * 32, 0);
                    break;
                case ParticleType.CenterFlower:
                    _spawnPoint = new Point3D(0, 0, 0);
                    break;
                default:
                    break;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {

            base.OnSourceInitialized(e);

            //WindowHandle(Win32) を取得
            var handle = new WindowInteropHelper(this).Handle;

            //クリックをスルー
            int extendStyle = GetWindowLong(handle, GWL_EXSTYLE);
            extendStyle |= WS_EX_TRANSPARENT; //フラグの追加
            SetWindowLong(handle, GWL_EXSTYLE, extendStyle);

        }
    }
}
