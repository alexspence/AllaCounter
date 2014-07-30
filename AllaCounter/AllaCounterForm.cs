using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllaCounter {
	public partial class AllaCounterForm : Form
	{


		public AllaCounterForm() {
			InitializeComponent();

			// Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8
			// Compute the addition of each combination of the keys you want to be pressed
			// ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...

			_counter = Counter.Init("counter.json");
			_counter.OnChange += OnCounterChange;

			OnCounterChange(_counter);
			RegisterHotKey(this.Handle, MyactionHotkeyId, 0, (int)Keys.F7);

		}

		private void OnCounterChange(Counter counter)
		{
			label1.Text = counter.CurrentCount.ToString();
		}


		protected override void WndProc(ref Message m) {
			if (m.Msg == 0x0312 && m.WParam.ToInt32() == MyactionHotkeyId) {
				// My hotkey has been typed

				_counter.IncrementCounter();

			}
			base.WndProc(ref m);
		}

	}
}
