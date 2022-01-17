using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using OxyPlot;
using OxyPlot.Axes;
using RootsFinder.Methods;

namespace RootsFinder
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _functionText;
        private readonly FunctionExpression _functionExpression;

        private readonly FunctionGraphBuilder _graphBuilder;

        private readonly DichotomyMethod _dichotomyMethod;
        private readonly FunctionGraphConfiguration _dichotomyMethodGraphConfig;
        private readonly PlotModel _dichotomyMethodPlotModel;
        private readonly DispatcherTimer _dichotomyMethodPlotModelTimer;

        private readonly NewtonsMethod _newtonsMethod;
        private readonly FunctionGraphConfiguration _newtonsMethodGraphConfig;
        private readonly PlotModel _newtonsMethodPlotModel;
        private readonly DispatcherTimer _newtonsMethodPlotModelTimer;

        private readonly SecantMethod _secantMethod;
        private readonly FunctionGraphConfiguration _secantMethodGraphConfig;
        private readonly PlotModel _secantMethodPlotModel;
        private readonly DispatcherTimer _secantMethodPlotModelTimer;

        private readonly IterationsMethod _iterationsMethod;
        private readonly FunctionGraphConfiguration _iterationsMethodGraphConfig;
        private readonly PlotModel _iterationsMethodPlotModel;
        private readonly DispatcherTimer _iterationsMethodPlotModelTimer;

        private readonly BackgroundWorker _mainWorker;

        private readonly List<string> _helpInformation;

        public MainViewModel()
        {
            _functionExpression = new FunctionExpression();
            _functionExpression.CanCalculate = false;

            _graphBuilder = new FunctionGraphBuilder(_functionExpression);

            //Dichotomy method related structures
            _dichotomyMethod = new DichotomyMethod(_functionExpression);

            _dichotomyMethodGraphConfig = new FunctionGraphConfiguration();
            _dichotomyMethodGraphConfig.CanBeDrawn = false;
            _dichotomyMethodGraphConfig.RootsColor = OxyColors.Red;

            _dichotomyMethodPlotModel = new PlotModel();
            var xAxisDichotomyMethod = new LinearAxis { Title = "X", Position = AxisPosition.Bottom, Minimum = -10, Maximum = 10 };
            xAxisDichotomyMethod.AxisChanged += DichotomyMethodGraphXAxisChanged;
            _dichotomyMethodPlotModel.Axes.Add(xAxisDichotomyMethod);
            var yAxisDichotomyMethod = new LinearAxis { Title = "Y", Position = AxisPosition.Left, Minimum = -10, Maximum = 10 };
            _dichotomyMethodPlotModel.Axes.Add(yAxisDichotomyMethod);

            _dichotomyMethodPlotModelTimer = new DispatcherTimer();
            _dichotomyMethodPlotModelTimer.Interval = new TimeSpan(0, 0, 0, 0, 350);
            _dichotomyMethodPlotModelTimer.Tick += DichotomyMethodPlotModelTimerTick;

            //Newton's method related structures
            _newtonsMethod = new NewtonsMethod(_functionExpression);

            _newtonsMethodGraphConfig = new FunctionGraphConfiguration();
            _newtonsMethodGraphConfig.CanBeDrawn = false;
            _newtonsMethodGraphConfig.RootsColor = OxyColors.Blue;

            _newtonsMethodPlotModel = new PlotModel();
            var xAxisNewtonsMethod = new LinearAxis { Title = "X", Position = AxisPosition.Bottom, Minimum = -10, Maximum = 10 };
            xAxisNewtonsMethod.AxisChanged += NewtonsMethodGraphXAxisChanged;
            _newtonsMethodPlotModel.Axes.Add(xAxisNewtonsMethod);
            var yAxisNewtonsMethod = new LinearAxis { Title = "Y", Position = AxisPosition.Left, Minimum = -10, Maximum = 10 };
            _newtonsMethodPlotModel.Axes.Add(yAxisNewtonsMethod);

            _newtonsMethodPlotModelTimer = new DispatcherTimer();
            _newtonsMethodPlotModelTimer.Interval = new TimeSpan(0, 0, 0, 0, 350);
            _newtonsMethodPlotModelTimer.Tick += NewtonsMethodPlotModelTimerTick;


            //Secant method related structures
            _secantMethod = new SecantMethod(_functionExpression);

            _secantMethodGraphConfig = new FunctionGraphConfiguration();
            _secantMethodGraphConfig.CanBeDrawn = false;
            _secantMethodGraphConfig.RootsColor = OxyColors.Green;

            _secantMethodPlotModel = new PlotModel();
            var xAxisSecantMethod = new LinearAxis { Title = "X", Position = AxisPosition.Bottom, Minimum = -10, Maximum = 10 };
            xAxisSecantMethod.AxisChanged += SecantMethodGraphXAxisChanged;
            _secantMethodPlotModel.Axes.Add(xAxisSecantMethod);
            var yAxisSecantMethod = new LinearAxis { Title = "Y", Position = AxisPosition.Left, Minimum = -10, Maximum = 10 };
            _secantMethodPlotModel.Axes.Add(yAxisSecantMethod);

            _secantMethodPlotModelTimer = new DispatcherTimer();
            _secantMethodPlotModelTimer.Interval = new TimeSpan(0, 0, 0, 0, 350);
            _secantMethodPlotModelTimer.Tick += SecantMethodPlotModelTimerTick;

            //Iterations method related structures
            _iterationsMethod = new IterationsMethod(_functionExpression);

            _iterationsMethodGraphConfig = new FunctionGraphConfiguration();
            _iterationsMethodGraphConfig.CanBeDrawn = false;
            _iterationsMethodGraphConfig.RootsColor = OxyColors.DarkMagenta;

            _iterationsMethodPlotModel = new PlotModel();
            var xAxisIterationsMethod = new LinearAxis { Title = "X", Position = AxisPosition.Bottom, Minimum = -10, Maximum = 10 };
            xAxisIterationsMethod.AxisChanged += IterationsMethodGraphXAxisChanged;
            _iterationsMethodPlotModel.Axes.Add(xAxisIterationsMethod);
            var yAxisIterationsMethod = new LinearAxis { Title = "Y", Position = AxisPosition.Left, Minimum = -10, Maximum = 10 };
            _iterationsMethodPlotModel.Axes.Add(yAxisIterationsMethod);

            _iterationsMethodPlotModelTimer = new DispatcherTimer();
            _iterationsMethodPlotModelTimer.Interval = new TimeSpan(0, 0, 0, 0, 350);
            _iterationsMethodPlotModelTimer.Tick += IterationsMethodPlotModelTimerTick;

            _mainWorker = new BackgroundWorker();
            _mainWorker.WorkerReportsProgress = true;
            _mainWorker.WorkerSupportsCancellation = true;
            _mainWorker.DoWork += PerformCalculations;
            _mainWorker.ProgressChanged += ProgressChanged;
            _mainWorker.RunWorkerCompleted += CalculationCompleted;

            _helpInformation = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "RootsFinder.Resources.new_help.txt";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var line = "";
                while (line != null)
                {
                    line = reader.ReadLine();
                    _helpInformation.Add(line);
                }
            }
    
            ProgressBarMinimun = 0;
            ProgressBarMaximum = 8;
            CalculationProgress = 0;

            Function = "sin(4*pi*x)+0.5*sin(2*pi*x)";
            //Function = "x^2+4*sin(x)-16";
            //Function = "(1/x+1/(x+1)-2*x*x)/10";
            Start = "-5";
            End = "5";
            Eps = "0,01";
            CanCalculate = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public string Function
        {
            get { return _functionText; }
            set
            {
                _functionText = value;
                OnPropertyChanged(nameof(Function));
            }
        }

        private double _start;
        public string Start
        {
            get { return _start.ToString(); }
            set
            {
                if (double.TryParse(value, out double result))
                {
                    _start = result;
                    if (_start > _end)
                    {
                        var t = _start;
                        _start = _end;
                        _end = t;
                        OnPropertyChanged(nameof(End));
                    }

                    OnPropertyChanged(nameof(Start));
                }
            }
        }

        private double _end;
        public string End
        {
            get { return _end.ToString(); }
            set
            {
                if (double.TryParse(value, out double result))
                {
                    _end = result;
                    if (_end < _start)
                    {
                        var t = _end;
                        _end = _start;
                        _start = t;
                        OnPropertyChanged(nameof(Start));
                    }

                    OnPropertyChanged(nameof(End));
                }
            }
        }

        private double _eps;
        public string Eps
        {
            get { return _eps.ToString(); }
            set
            {
                if (double.TryParse(value, out double result) &&
                    result > 0)
                {
                    _eps = result;
                    OnPropertyChanged(nameof(Eps));
                }
            }
        }

        private bool _canCalculate;
        public bool CanCalculate
        {
            get
            {
                return _canCalculate;
            }
            private set
            {
                _canCalculate = value;
                OnPropertyChanged(nameof(CanCalculate));

                _canEnterData = !value;
                OnPropertyChanged(nameof(CanEnterData));
            }
        }

        private bool _canEnterData;
        public bool CanEnterData
        {
            get
            {
                return _canEnterData;
            }
            private set
            {
                _canEnterData = value;
                OnPropertyChanged(nameof(CanEnterData));

                _canCalculate = !value;
                OnPropertyChanged(nameof(CanCalculate));
            }
        }

        private int _progressBarMinimun;
        public double ProgressBarMinimun
        {
            get
            {
                return _progressBarMinimun;
            }
            private set
            {
                _progressBarMinimun = Convert.ToInt32(value);
                OnPropertyChanged(nameof(ProgressBarMinimun));
            }
        }

        private int _progressBarMaximum;
        public double ProgressBarMaximum
        {
            get
            {
                return _progressBarMaximum;
            }
            private set
            {
                _progressBarMaximum = Convert.ToInt32(value);
                OnPropertyChanged(nameof(ProgressBarMaximum));
            }
        }

        private double _calculationProgress;
        public double CalculationProgress
        {
            get
            {
                return _calculationProgress;
            }
            set
            {
                _calculationProgress = value;
                OnPropertyChanged(nameof(CalculationProgress));
            }
        }

        private bool _isCancelled;
        public bool IsCancelled
        {
            get
            {
                return _isCancelled;
            }
            set
            {
                _isCancelled = value;
                OnPropertyChanged(nameof(IsCancelled));
            }
        }

        public PlotModel DichotomyMethodPlotModel
        {
            get { return _dichotomyMethodPlotModel; }
        }

        public PlotModel NewtonsMethodPlotModel
        {
            get { return _newtonsMethodPlotModel; }
        }

        public PlotModel SecantMethodPlotModel
        {
            get { return _secantMethodPlotModel; }
        }

        public PlotModel IterationsMethodPlotModel
        {
            get { return _iterationsMethodPlotModel; }
        }

        public ObservableCollection<string> DichotomyMethodRoots
        {
            get
            {
                var rootsInfo = new List<string>();
                var i = 1;
                foreach (var rootPoint in _dichotomyMethod.LastCalculatedRoots)
                {
                    rootsInfo.Add(string.Format("{0}: {1}", i, rootPoint.X));
                    i += 1;
                }

                return new ObservableCollection<string>(rootsInfo);
            }
        }

        public ObservableCollection<string> NewtonsMethodRoots
        {
            get
            {
                var rootsInfo = new List<string>();
                var i = 1;
                foreach (var rootPoint in _newtonsMethod.LastCalculatedRoots)
                {
                    rootsInfo.Add(string.Format("{0}: {1}", i, rootPoint.X));
                    i += 1;
                }

                return new ObservableCollection<string>(rootsInfo);
            }
        }

        public ObservableCollection<string> SecantMethodRoots
        {
            get
            {
                var rootsInfo = new List<string>();
                var i = 1;
                foreach (var rootPoint in _secantMethod.LastCalculatedRoots)
                {
                    rootsInfo.Add(string.Format("{0}: {1}", i, rootPoint.X));
                    i += 1;
                }

                return new ObservableCollection<string>(rootsInfo);
            }
        }

        public ObservableCollection<string> IterationsMethodRoots
        {
            get
            {
                var rootsInfo = new List<string>();
                var i = 1;
                foreach (var rootPoint in _iterationsMethod.LastCalculatedRoots)
                {
                    rootsInfo.Add(string.Format("{0}: {1}", i, rootPoint.X));
                    i += 1;
                }

                return new ObservableCollection<string>(rootsInfo);
            }
        }

        private RelayCommand _getHelp;
        public RelayCommand GetHelp
        {
            get
            {
                return _getHelp ??= new RelayCommand(obj =>
                {
                    ShowHelp();
                });
            }
        }

        private RelayCommand _findRoots;
        public RelayCommand FindRoots
        {
            get
            {
                return _findRoots ??= new RelayCommand(obj =>
                {
                    Main();
                });
            }
        }

        private RelayCommand _cancelCalculations;
        public RelayCommand CancelCalculations
        {
            get
            {
                return _cancelCalculations ??= new RelayCommand(obj =>
                {
                    Cancel();
                });
            }
        }

        private void ShowHelp()
        {
            var listWindow = new ListWindow("Functions help list", _helpInformation);
            listWindow.ShowDialog();
        }

        private void Main()
        {
            _functionExpression.CanCalculate = false;
            CanCalculate = false;
            IsCancelled = false;
            CalculationProgress = 0;

            if (!_functionExpression.TryLoadFunction(Function))
            {
                MessageBox.Show("Function expression is not valid!",
                    "Function parsing error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                CanCalculate = true;
                return;
            }

            if (!_mainWorker.IsBusy)
            {
                PrepareForCalculations();
                _mainWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Calculation is already running",
                    "Calculation error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            _mainWorker.CancelAsync();
        }

        private void PrepareForCalculations()
        {
            _functionExpression.CanCalculate = true;

            _dichotomyMethodGraphConfig.CanBeDrawn = false;
            _dichotomyMethodGraphConfig.RootsStart = _start;
            _dichotomyMethodGraphConfig.RootsEnd = _end;
            _dichotomyMethodGraphConfig.Start = _dichotomyMethodPlotModel.Axes[0].ActualMinimum;
            _dichotomyMethodGraphConfig.End = _dichotomyMethodPlotModel.Axes[0].ActualMaximum;
            _dichotomyMethodGraphConfig.Bottom = _dichotomyMethodPlotModel.Axes[1].ActualMinimum;
            _dichotomyMethodGraphConfig.Top = _dichotomyMethodPlotModel.Axes[1].ActualMaximum;
            _dichotomyMethodGraphConfig.Precision = _eps;

            _newtonsMethodGraphConfig.CanBeDrawn = false;
            _newtonsMethodGraphConfig.RootsStart = _start;
            _newtonsMethodGraphConfig.RootsEnd = _end;
            _newtonsMethodGraphConfig.Start = _newtonsMethodPlotModel.Axes[0].ActualMinimum;
            _newtonsMethodGraphConfig.End = _newtonsMethodPlotModel.Axes[0].ActualMaximum;
            _newtonsMethodGraphConfig.Bottom = _newtonsMethodPlotModel.Axes[1].ActualMinimum;
            _newtonsMethodGraphConfig.Top = _newtonsMethodPlotModel.Axes[1].ActualMaximum;
            _newtonsMethodGraphConfig.Precision = _eps;

            _secantMethodGraphConfig.CanBeDrawn = false;
            _secantMethodGraphConfig.RootsStart = _start;
            _secantMethodGraphConfig.RootsEnd = _end;
            _secantMethodGraphConfig.Start = _secantMethodPlotModel.Axes[0].ActualMinimum;
            _secantMethodGraphConfig.End = _secantMethodPlotModel.Axes[0].ActualMaximum;
            _secantMethodGraphConfig.Bottom = _secantMethodPlotModel.Axes[1].ActualMinimum;
            _secantMethodGraphConfig.Top = _secantMethodPlotModel.Axes[1].ActualMaximum;
            _secantMethodGraphConfig.Precision = _eps;

            _iterationsMethodGraphConfig.CanBeDrawn = false;
            _iterationsMethodGraphConfig.RootsStart = _start;
            _iterationsMethodGraphConfig.RootsEnd = _end;
            _iterationsMethodGraphConfig.Start = _iterationsMethodPlotModel.Axes[0].ActualMinimum;
            _iterationsMethodGraphConfig.End = _iterationsMethodPlotModel.Axes[0].ActualMaximum;
            _iterationsMethodGraphConfig.Bottom = _iterationsMethodPlotModel.Axes[1].ActualMinimum;
            _iterationsMethodGraphConfig.Top = _iterationsMethodPlotModel.Axes[1].ActualMaximum;
            _iterationsMethodGraphConfig.Precision = _eps;
        }

        private void UpdateDichotomyMethodPlotModel()
        {
            if (!(_dichotomyMethodGraphConfig.CanBeDrawn && _functionExpression.CanCalculate && _functionExpression.IsSyntaxCorrect))
            {
                return;
            }

            var graph = _graphBuilder.GetFunctionGraph(_dichotomyMethodGraphConfig);
            _dichotomyMethodPlotModel.Series.Clear();
            foreach (var lineSeries in graph)
            {
                _dichotomyMethodPlotModel.Series.Add(lineSeries);
            }

            _dichotomyMethodPlotModel.InvalidatePlot(true);
        }

        private void DichotomyMethodPlotModelTimerTick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            UpdateDichotomyMethodPlotModel();
        }

        private void DichotomyMethodGraphXAxisChanged(object sender, OxyPlot.Axes.AxisChangedEventArgs e)
        {
            _dichotomyMethodGraphConfig.MoveHorizontally(_dichotomyMethodPlotModel.Axes[0].ActualMinimum, _dichotomyMethodPlotModel.Axes[0].ActualMaximum);
            _dichotomyMethodGraphConfig.MoveVertically(_dichotomyMethodPlotModel.Axes[1].ActualMinimum, _dichotomyMethodPlotModel.Axes[1].ActualMaximum);

            _dichotomyMethodPlotModelTimer.Stop();
            _dichotomyMethodPlotModelTimer.Start();
        }

        private void UpdateNewtonsMethodPlotModel()
        {
            if (!(_newtonsMethodGraphConfig.CanBeDrawn && _functionExpression.CanCalculate && _functionExpression.IsSyntaxCorrect))
            {
                return;
            }

            var graph = _graphBuilder.GetFunctionGraph(_newtonsMethodGraphConfig);
            _newtonsMethodPlotModel.Series.Clear();
            foreach (var lineSeries in graph)
            {
                _newtonsMethodPlotModel.Series.Add(lineSeries);
            }

            _newtonsMethodPlotModel.InvalidatePlot(true);
        }

        private void NewtonsMethodPlotModelTimerTick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            UpdateNewtonsMethodPlotModel();
        }

        private void NewtonsMethodGraphXAxisChanged(object sender, OxyPlot.Axes.AxisChangedEventArgs e)
        {
            _newtonsMethodGraphConfig.MoveHorizontally(_newtonsMethodPlotModel.Axes[0].ActualMinimum, _newtonsMethodPlotModel.Axes[0].ActualMaximum);
            _newtonsMethodGraphConfig.MoveVertically(_newtonsMethodPlotModel.Axes[1].ActualMinimum, _newtonsMethodPlotModel.Axes[1].ActualMaximum);

            _newtonsMethodPlotModelTimer.Stop();
            _newtonsMethodPlotModelTimer.Start();
        }

        private void UpdateSecantMethodPlotModel()
        {
            if (!(_secantMethodGraphConfig.CanBeDrawn && _functionExpression.CanCalculate && _functionExpression.IsSyntaxCorrect))
            {
                return;
            }

            var graph = _graphBuilder.GetFunctionGraph(_secantMethodGraphConfig);
            _secantMethodPlotModel.Series.Clear();
            foreach (var lineSeries in graph)
            {
                _secantMethodPlotModel.Series.Add(lineSeries);
            }

            _secantMethodPlotModel.InvalidatePlot(true);
        }

        private void SecantMethodPlotModelTimerTick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            UpdateSecantMethodPlotModel();
        }

        private void SecantMethodGraphXAxisChanged(object sender, OxyPlot.Axes.AxisChangedEventArgs e)
        {
            _secantMethodGraphConfig.MoveHorizontally(_secantMethodPlotModel.Axes[0].ActualMinimum, _secantMethodPlotModel.Axes[0].ActualMaximum);
            _secantMethodGraphConfig.MoveVertically(_secantMethodPlotModel.Axes[1].ActualMinimum, _secantMethodPlotModel.Axes[1].ActualMaximum);

            _secantMethodPlotModelTimer.Stop();
            _secantMethodPlotModelTimer.Start();
        }

        private void UpdateIterationsMethodPlotModel()
        {
            if (!(_iterationsMethodGraphConfig.CanBeDrawn && _functionExpression.CanCalculate && _functionExpression.IsSyntaxCorrect))
            {
                return;
            }

            var graph = _graphBuilder.GetFunctionGraph(_iterationsMethodGraphConfig);
            _iterationsMethodPlotModel.Series.Clear();
            foreach (var lineSeries in graph)
            {
                _iterationsMethodPlotModel.Series.Add(lineSeries);
            }

            _iterationsMethodPlotModel.InvalidatePlot(true);
        }

        private void IterationsMethodPlotModelTimerTick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            UpdateIterationsMethodPlotModel();
        }

        private void IterationsMethodGraphXAxisChanged(object sender, OxyPlot.Axes.AxisChangedEventArgs e)
        {
            _iterationsMethodGraphConfig.MoveHorizontally(_iterationsMethodPlotModel.Axes[0].ActualMinimum, _iterationsMethodPlotModel.Axes[0].ActualMaximum);
            _iterationsMethodGraphConfig.MoveVertically(_iterationsMethodPlotModel.Axes[1].ActualMinimum, _iterationsMethodPlotModel.Axes[1].ActualMaximum);

            _iterationsMethodPlotModelTimer.Stop();
            _iterationsMethodPlotModelTimer.Start();
        }

        private bool CheckIfCancellationRequested()
        {
            return _mainWorker.CancellationPending;
        }

        private void PerformCalculations(object sender, DoWorkEventArgs e)
        {
            var progress = Convert.ToInt32(ProgressBarMinimun);

            _dichotomyMethod.CalculateRoots(_dichotomyMethodGraphConfig.RootsStart, _dichotomyMethodGraphConfig.RootsEnd, _dichotomyMethodGraphConfig.Precision, CheckIfCancellationRequested);
            _dichotomyMethodGraphConfig.Roots = _dichotomyMethod.LastCalculatedRoots;
            OnPropertyChanged(nameof(DichotomyMethodRoots));
            _mainWorker.ReportProgress(++progress);

            if (CheckIfCancellationRequested())
            {
                e.Cancel = true;
                return;
            }

            _dichotomyMethodGraphConfig.CanBeDrawn = true;
            UpdateDichotomyMethodPlotModel();
            _mainWorker.ReportProgress(++progress);

            if (CheckIfCancellationRequested())
            {
                e.Cancel = true;
                return;
            }

            _newtonsMethod.CalculateRoots(_newtonsMethodGraphConfig.RootsStart, _newtonsMethodGraphConfig.RootsEnd, _newtonsMethodGraphConfig.Precision, CheckIfCancellationRequested);
            _newtonsMethodGraphConfig.Roots = _newtonsMethod.LastCalculatedRoots;
            OnPropertyChanged(nameof(NewtonsMethodRoots));
            _mainWorker.ReportProgress(++progress);

            if (CheckIfCancellationRequested())
            {
                e.Cancel = true;
                return;
            }

            _newtonsMethodGraphConfig.CanBeDrawn = true;
            UpdateNewtonsMethodPlotModel();
            _mainWorker.ReportProgress(++progress);

            if (CheckIfCancellationRequested())
            {
                e.Cancel = true;
                return;
            }

            _secantMethod.CalculateRoots(_secantMethodGraphConfig.RootsStart, _secantMethodGraphConfig.RootsEnd, _secantMethodGraphConfig.Precision, CheckIfCancellationRequested);
            _secantMethodGraphConfig.Roots = _secantMethod.LastCalculatedRoots;
            OnPropertyChanged(nameof(SecantMethodRoots));
            _mainWorker.ReportProgress(++progress);

            if (CheckIfCancellationRequested())
            {
                e.Cancel = true;
                return;
            }

            _secantMethodGraphConfig.CanBeDrawn = true;
            UpdateSecantMethodPlotModel();
            _mainWorker.ReportProgress(++progress);

            if (CheckIfCancellationRequested())
            {
                e.Cancel = true;
                return;
            }

            _iterationsMethod.CalculateRoots(_iterationsMethodGraphConfig.RootsStart, _iterationsMethodGraphConfig.RootsEnd, _iterationsMethodGraphConfig.Precision, CheckIfCancellationRequested);
            _iterationsMethodGraphConfig.Roots = _iterationsMethod.LastCalculatedRoots;
            OnPropertyChanged(nameof(IterationsMethodRoots));
            _mainWorker.ReportProgress(++progress);

            if (CheckIfCancellationRequested())
            {
                e.Cancel = true;
                return;
            }

            _iterationsMethodGraphConfig.CanBeDrawn = true;
            UpdateIterationsMethodPlotModel();
            _mainWorker.ReportProgress(++progress);
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CalculationProgress = e.ProgressPercentage;
        }

        private void CalculationCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                IsCancelled = true;

                //prevent drawing
                _dichotomyMethodGraphConfig.CanBeDrawn = false;
                _newtonsMethodGraphConfig.CanBeDrawn = false;
                _secantMethodGraphConfig.CanBeDrawn = false;
                _iterationsMethodGraphConfig.CanBeDrawn = false;

                //clear plots
                _dichotomyMethodPlotModel.Series.Clear();
                _dichotomyMethodPlotModel.InvalidatePlot(true);

                _newtonsMethodPlotModel.Series.Clear();
                _newtonsMethodPlotModel.InvalidatePlot(true);

                _secantMethodPlotModel.Series.Clear();
                _secantMethodPlotModel.InvalidatePlot(true);

                _iterationsMethodPlotModel.Series.Clear();
                _iterationsMethodPlotModel.InvalidatePlot(true);
            }

            CanCalculate = true;
        }
    }
}
