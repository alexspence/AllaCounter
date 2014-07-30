using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using NLog;

namespace AllaCounter {


	public class Counter : IDisposable {

		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
		private int _currentCount;

		public string CounterFileLocation { get; set; }

		public int CurrentCount {
			get { return _currentCount; }
			set { _currentCount = value; }
		}

		public event Action<Counter> OnChange;

		private Counter() {

		}

		public void InitializeCounter() {
			Log.Info("Counter Initialized to value: {0}", CurrentCount);
			InvokeOnChanged();
		}

		public void Save() {
			var json = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(CounterFileLocation, json);
			Log.Debug("Saved Counter File with value: {0}", CurrentCount);
			InvokeOnChanged();
		}

		public void IncrementCounter() {
			Interlocked.Increment(ref _currentCount);
			Save();
		}

		private void InvokeOnChanged() {
			if (OnChange != null) {
				OnChange(this);
			}
		}

		public static Counter Init(string counterFileLocation)
		{

			if (File.Exists(counterFileLocation)) {
				try {
					var json = File.ReadAllText(counterFileLocation);
					return JsonConvert.DeserializeObject<Counter>(json);
				} catch (Exception e) {
					Log.Error("Error loading the counter.. ", e);
					throw;
				}
			}

			// New counter

			var counter = new Counter {
				CounterFileLocation = counterFileLocation,
				CurrentCount = 0
			};

			counter.Save();

			return counter;
		}

		public void Dispose() {
			Save();
		}
	}
}
